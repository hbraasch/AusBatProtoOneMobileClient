using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using static DocGenOneMobileClient.Views.TemplateListPage;
using static DocGenOneMobileClient.Views.TemplateListPage.ListViewDataTemplate;
using System.Threading.Tasks;
using AusBatProtoOneMobileClient;
using AusBatProtoOneMobileClient.ViewModels;
using Mobile.ViewModels;
using Mobile.Helpers;
using TreeApp.Helpers;
using AusBatProtoOneMobileClient.Models;
using AusBatProtoOneMobileClient.Data;

namespace DocGenOneMobileClient.Views
{
    public class DisplayFamilyPageViewModel : ViewModelBase
    {
        List<Classification> selectedFamilies = null;
        public class DisplayItemBase { }
        public class FamilyDisplayItem: DisplayItemBase
        {
            public string FamilyName { get; set; }
            public string ImageSource { get; set; }
            public Classification Family { get; set; }
        }


        public ObservableCollection<DisplayItemBase> DisplayItems { get; set; }

        public DisplayItemBase SelectedItem { get; set; }

        public void OnSelectedItemChanged()
        {
            IsSelected = SelectedItem != null;
            InvalidateMenu.Execute(null);
        }

       
        public bool IsDirty { get; set; }

        public ICommand InvalidateMenu { get; set; }

        CommandHelper commandHelper = new CommandHelper();

        CancellationTokenSource cts;


        #region *// Menu related 

        public bool IsSelected { get; set; }

        #endregion
          

        public DisplayFamilyPageViewModel()
        {
            DisplayItems = new ObservableCollection<DisplayItemBase>();
        }

        public DisplayFamilyPageViewModel(List<Classification> selectedFamilies)
        {
            this.selectedFamilies = selectedFamilies;
            DisplayItems = new ObservableCollection<DisplayItemBase>();
        }

        public ICommand OnFirstAppearance => new Command(async () =>
        {
            try
            {

                cts = new CancellationTokenSource();

                ActivityIndicatorStart(() => {
                    cts.Cancel();
                    ActivityIndicatorStop();
                });

                UpdateDisplay();


            }
            catch (Exception ex) when (ex is TaskCanceledException ext)
            {
                Debug.Write("Cancelled by user");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error",ex.CompleteMessage(), "Ok");
            }
            finally
            {
                ActivityIndicatorStop();
            }
        });

        public void UpdateDisplay()
        {

            DisplayItems = new ObservableCollection<DisplayItemBase>();
            var selectedFamilyIds = selectedFamilies?.Select(o => o.Id) ?? null;
            foreach (var family in App.dbase.Classifications.Where(o => o.Type == Classification.ClassificationType.Family))
            {
                if (selectedFamilies == null)
                {
                    DisplayItems.Add(new FamilyDisplayItem { FamilyName = family.Id, ImageSource = "", Family = family });
                }
                else if (selectedFamilies.Count > 0)
                {
                    if (selectedFamilyIds.Contains(family.Id))
                    {
                        DisplayItems.Add(new FamilyDisplayItem { FamilyName = family.Id, ImageSource = "", Family = family });
                    }
                }
            }

            Debug.WriteLine($"Data count: {DisplayItems.Count}"); 

        }


        public ICommand OnSubsequentAppearance => new Command(() =>
        {
            try
            {
                ActivityIndicatorStart();
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

        public ICommand OnBackMenuPressed => new Command(() =>
        {
            NavigateBack(NavigateReturnType.IsCancelled);
        });

        public bool IsHomeEnabled { get; set; }
        public ICommand OnHomeMenuPressed => new Command(() =>
        {
            NavigateBack(NavigateReturnType.GotoRoot);
        });

        public bool isBackCancelled = false;
        public ICommand OnBackButtonPressed => new Command(() =>
        {
            isBackCancelled = true;
        });


        public ICommand OnSelectMenuPressed => commandHelper.ProduceDebouncedCommand(async () =>
        {

            try
            {
                if (SelectedItem == null) return;
                var viewModel = new GenusKeyPageViewModel((SelectedItem as FamilyDisplayItem).Family) { IsHomeEnabled = true };
                var page = new GenusKeyPage(viewModel);
                var resultType = await NavigateToPageAsync(page, viewModel);
                if (resultType == NavigateReturnType.GotoRoot) NavigateBack(NavigateReturnType.GotoRoot);
            }
            catch (Exception ex) when (ex is BusinessException exb)
            {
                await Application.Current.MainPage.DisplayAlert("Notification", exb.CompleteMessage(), "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Problem: ", ex.CompleteMessage(), "OK");
            }
            finally
            {
                ActivityIndicatorStop();
            }
        });
    }
}
    

