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
    public class DisplayFilteredSpeciesPageViewModel : ViewModelBase
    {
        List<Species> FilteredSpecieses;

        public class SpeciesDisplayItem
        {
            public string SpeciesName { get; set; }
            public string FriendlyName { get; set; }
            public string ImageSource { get; set; }
            public Species Species { get; set; }
        }
        public class GroupedSpeciesDisplayItem : ObservableCollection<SpeciesDisplayItem>
        {
            public string Alphabet { get; set; }
        }

        public ObservableCollection<GroupedSpeciesDisplayItem> SpeciesGroupDisplayItems { get; set; }

        public SpeciesDisplayItem SelectedItem { get; set; }

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

        public DisplayFilteredSpeciesPageViewModel(List<Species> specieses)
        {
            this.FilteredSpecieses = specieses;
            SpeciesGroupDisplayItems = new ObservableCollection<GroupedSpeciesDisplayItem>();
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

                SpeciesGroupDisplayItems = UpdateDisplay();

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

        public ObservableCollection<GroupedSpeciesDisplayItem> UpdateDisplay()
        {
            var list = new ObservableCollection<GroupedSpeciesDisplayItem>();

            var specieses = App.dbase.Species.OrderBy(species => $"{species.GenusId} {species.SpeciesId}");
            GroupedSpeciesDisplayItem familyGroupDisplayItem = null;
            foreach (var species in FilteredSpecieses)
            {
                var alphabet = species.GenusId.Substring(0, 1).ToUpper();
                if (!list.ToList().Exists(o => o.Alphabet == alphabet))
                {
                    familyGroupDisplayItem = new GroupedSpeciesDisplayItem { Alphabet = alphabet.ToUpper() };
                    list.Add(familyGroupDisplayItem);
                }
                var imageSource = (species.Images.Count > 0) ? species.Images.First() : "bat.png";
                familyGroupDisplayItem.Add(new SpeciesDisplayItem
                {
                    SpeciesName = $"{species.GenusId.ToUpperFirstChar()} {species.SpeciesId.ToLower()}",
                    FriendlyName = species.Name,
                    ImageSource = imageSource,
                    Species = species
                });
            }
            return list;
        }


        public ICommand OnSubsequentAppearance => new Command(async () =>
        {
            try
            {
                await ActivityIndicatorStart();
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
            NavigateBack(NavigateReturnType.GotoHome);
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
                var viewModel = new DisplaySpeciesTabbedPageViewModel((SelectedItem as SpeciesDisplayItem).Species) { IsHomeEnabled = IsHomeEnabled };
                var page = new DisplaySpeciesTabbedPage(viewModel);
                var resultType = await NavigateToPageAsync(page, viewModel);
                if (resultType == NavigateReturnType.GotoHome) NavigateBack(NavigateReturnType.GotoHome);
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
    

