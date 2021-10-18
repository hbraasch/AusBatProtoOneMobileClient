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
    public class SpeciesByFamilyPageViewModel : ViewModelBase
    {
        public class SpeciesDisplayItem
        {
            public string SpeciesName { get; set; }
            public string FriendlyName { get; set; }
            public string ImageSource { get; set; }
            public Species Species { get; set; }
        }
        public class GroupedSpeciesDisplayItem : ObservableCollection<SpeciesDisplayItem>
        {
            public string FamilyName { get; set; }
            public string ImageSource { get; set; }
        }

        public ObservableCollection<GroupedSpeciesDisplayItem> FamilyGroupDisplayItems { get; set; }

        public GroupedSpeciesDisplayItem pageTypeGroup {get;set;}

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
          

        public SpeciesByFamilyPageViewModel()
        {
            FamilyGroupDisplayItems = new ObservableCollection<GroupedSpeciesDisplayItem>();
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

            FamilyGroupDisplayItems = new ObservableCollection<GroupedSpeciesDisplayItem>();

            foreach (var family in App.dbase.Classifications.Where(o => o.Type == Classification.ClassificationType.Family))
            {
                var genusesInFamily = App.dbase.Classifications.Where(o => o.Parent == family.Id).ToList();
                var speciesesInFamily = App.dbase.GetAllSpecies(genusesInFamily);
                var familyGroupDisplayItem = new GroupedSpeciesDisplayItem { FamilyName = family.Id };
                foreach (var species in speciesesInFamily)
                {
                    if (species != null)
                    {
                        var imageSource = (species.Images.Count > 0) ? species.Images.First() : "";
                        familyGroupDisplayItem.Add(new SpeciesDisplayItem
                        {
                            SpeciesName = $"{species.GenusId.ToUpperFirstChar()} {species.SpeciesId.ToLower()}",
                            FriendlyName = species.Name,
                            Species = species,
                            ImageSource = imageSource
                        }); 
                    }
                }
                FamilyGroupDisplayItems.Add(familyGroupDisplayItem);
            }
            Debug.WriteLine($"Data count: {FamilyGroupDisplayItems.Count}"); 

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

        public bool isBackCancelled = false;
        public ICommand OnBackButtonPressed => new Command(() =>
        {
            isBackCancelled = true;
        });


        public ICommand OnSelectPressed => commandHelper.ProduceDebouncedCommand(async () =>
        {

            try
            {
                if (SelectedItem == null) return;
                var viewModel = new DisplaySpeciesTabbedPageViewModel(SelectedItem.Species);
                var page = new DisplaySpeciesTabbedPage(viewModel);
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
    

