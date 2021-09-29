using AusBatProtoOneMobileClient.Models;
using Mobile.Helpers;
using Mobile.Models;
using Mobile.ViewModels;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TreeApp.Helpers;
using TreeApps.Models;
using Xamarin.Forms;

namespace AusBatProtoOneMobileClient.ViewModels
{
    public class ClassificationPageViewModel : ViewModelBase
    {
        CommandHelper commandHelper = new CommandHelper();

        public class DisplayItem : ITreeviewItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public ITreeviewItem Parent { get; set; }
            public List<ITreeviewItem> Children { get; set; } = new List<ITreeviewItem>();
            public string ImageFilename { get; set; }
            public Classification Content { get; set; }
        }
        public ObservableCollection<DisplayItem> DisplayItems { get; set; }

        [DoNotCheckEquality]
        public DisplayItem SelectedItem { get; set; }
        public void OnSelectedItemChanged()
        {
            IsSpeciesSelected = (SelectedItem.Content as Classification).Type == Classification.ClassificationType.Species;
            InvalidateMenu?.Execute(null);
        }
        public ObservableCollection<DisplayItem> ExpandedItems { get; set; }
        public ICommand InvalidateMenu { get; set; }
        public Action<bool> InvalidateTreeviewCommand { get; set; }

        public bool IsSpeciesSelected { get; set; }


        #region *// Menu related
        public ICommand InvalidateMenuCommand { get; set; }
        public bool IsLoggedIn { get; set; } = false;
        #endregion
        public ClassificationPageViewModel()
        {
            DisplayItems = new ObservableCollection<DisplayItem>();
            ExpandedItems = new ObservableCollection<DisplayItem>();
            IsSpeciesSelected = false;
        }

        public ICommand OnFirstAppearance => commandHelper.ProduceDebouncedCommand(() => {
            try
            {
                ActivityIndicatorStart();
                DisplayItems = UpdateDisplayItems();
                // ExpandedItems = GetExpandedItemsFromPeristentStore();

                InvalidateTreeviewCommand.Invoke(true);
                InvalidateMenu?.Execute(null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.CompleteMessage());
            }
            finally
            {
                ActivityIndicatorStop();
            }
        });

        private ObservableCollection<DisplayItem> UpdateDisplayItems()
        {
            var displayItems = new ObservableCollection<DisplayItem>();
            int id = 0;
            foreach (var classification in App.dbase.Classifications.Where(o => o.Parent == null))
            {
                var displayItem = GenerateDisplayDataItem(classification);
                displayItems.Add(displayItem);
            }
            return displayItems;

            // Helper
            DisplayItem GenerateDisplayDataItem(Classification sourceItem)
            {
                var displayItem = new DisplayItem { Id = id++, Name = sourceItem.Id, Content = sourceItem };
                var children = App.dbase.Classifications.Where(o => o.Parent == sourceItem.Id);
                foreach (var sourceItemChild in children)
                {
                    var displayItemChild = GenerateDisplayDataItem(sourceItemChild);
                    displayItemChild.Parent = displayItem;
                    displayItem.Children.Add(displayItemChild);
                }
                return displayItem;
            }
        }

        public ICommand OnSubsequentAppearance => commandHelper.ProduceDebouncedCommand(() => { });

        public ICommand OnBackMenuPressed => new Command(() =>
        {
            NavigateBack(true);
        });

        public bool isBackCancelled = false;
        public ICommand OnBackButtonPressed => new Command(() =>
        {
            NavigateBack(true);
            isBackCancelled = true;
        });

        public ICommand OnClassificationMenuPressed => commandHelper.ProduceDebouncedCommand(async () => {
            try
            {
                var cts = new CancellationTokenSource();
                ActivityIndicatorStart("Starting ...", () =>
                {
                    cts.Cancel();
                    ActivityIndicatorStop();
                });

                // Do work here
            }
            catch (Exception ex) when (ex is TaskCanceledException ext)
            {
                Debug.Write("Cancelled by user");
            }
            catch (Exception ex) when (ex is BusinessException exb)
            {
                await DisplayAlert("Notification", exb.CompleteMessage(), "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Problem: ", ex.CompleteMessage(), "OK");
            }
            finally
            {
                ActivityIndicatorStop();
            }
        });

        public ICommand OnSelectClicked => commandHelper.ProduceDebouncedCommand(async () => {
            try
            {
                var classification = (SelectedItem.Content as Classification);
                var bat = App.dbase.Bats.FirstOrDefault(o=>o.GenusId == classification.Id);
                if (bat == null) throw new BusinessException("Select a [species]");

                var viewModel = new DisplayBatTabbedPageViewModel(bat);
                var page = new DisplayBatTabbed(viewModel);
                await NavigateToPageAsync(page, viewModel);
            }
            catch (Exception ex) when (ex is TaskCanceledException ext)
            {
                Debug.Write("Cancelled by user");
            }
            catch (Exception ex) when (ex is BusinessException exb)
            {
                await DisplayAlert("Notification", exb.CompleteMessage(), "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Problem: ", ex.CompleteMessage(), "OK");
            }
            finally
            {
                ActivityIndicatorStop();
            }
        });
        

        public ICommand OnTestMenuPressed => commandHelper.ProduceDebouncedCommand(async () => {
            try
            {
                await DisplayAlert("Alert", "Next page", "Ok");
                var cts = new CancellationTokenSource();
                ActivityIndicatorStart("Starting ...", () =>
                {
                    cts.Cancel();
                    ActivityIndicatorStop();
                    Debug.WriteLine("Cancel pressed");
                });
            }
            catch (Exception ex) when (ex is TaskCanceledException ext)
            {
                Debug.Write("Cancelled by user"); 
            }
            catch (Exception ex) when (ex is BusinessException exb)
            {
                await DisplayAlert("Notification", exb.CompleteMessage(), "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Problem: ", ex.CompleteMessage(), "OK");
            }
            finally
            {
                ActivityIndicatorStop();
            }
        });
    }
}
