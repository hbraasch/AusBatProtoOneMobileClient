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
    public class FamilyKeyPageViewModel : ViewModelBase
    {

        public class DisplayItemBase { }
        public class FamilyDisplayItem: DisplayItemBase
        {
            public string FamilyName { get; set; }
            public string ImageSource { get; set; }
            public Classification Family { get; set; }
        }

        public class MapRegionsDisplayItem : DisplayItemBase
        {
            public string Description { get; set; } = "Regions";
            public List<MapRegion> MapRegions { get; set; }
            public string SelectionAmount { get; set; }
            public ICommand OnSearch { get; set; }

            public List<Bat> ConductSearch()
            {
                return App.dbase.Bats.Where(o => o.MapRegions.Intersect(MapRegions, new RegionComparer()).Count() > 0).ToList();
            }

            public bool HasEntry() => MapRegions?.Count > 0;
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
          

        public FamilyKeyPageViewModel()
        {
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

            foreach (var family in App.dbase.Classifications.Where(o => o.Type == Classification.ClassificationType.Family))
            {
                DisplayItems.Add(new FamilyDisplayItem { FamilyName = family .Id, ImageSource =  "", Family = family});
            }
            // DisplayItems.Add(new MapRegionsDisplayItem() { OnSearch = OnRegionSelectButtonPressed });
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
            NavigateBack(true);
        });

        public bool isBackCancelled = false;
        public ICommand OnBackButtonPressed => new Command(() =>
        {
            isBackCancelled = true;
        });

        public ICommand OnRegionSelectButtonPressed => commandHelper.ProduceDebouncedCommand(async () =>
        {
            try
            {
                MapRegionsDisplayItem mapRegionsDisplayItem = null;
                List<MapRegion> regions = new List<MapRegion>();
                foreach (var displayItem in DisplayItems)
                {
                    if (displayItem is MapRegionsDisplayItem)
                    {
                        mapRegionsDisplayItem = (MapRegionsDisplayItem)displayItem;
                        regions = (displayItem as MapRegionsDisplayItem).MapRegions;
                    }
                }
                var selectedRegions = (regions == null) ? new ObservableCollection<MapRegion>() : new ObservableCollection<MapRegion>(regions);
                var viewModel = new SelectBatRegionsPageViewModel() { SelectedMapRegions = selectedRegions };
                var page = new SelectBatRegionsPage(viewModel);
                var accept = await NavigateToPageAsync(page, viewModel);
                if (!accept) return;

                var updatedDisplayItem = new MapRegionsDisplayItem { Description = mapRegionsDisplayItem.Description, MapRegions = viewModel.SelectedMapRegions.ToList(), SelectionAmount = $"{viewModel.SelectedMapRegions.Count} selected", OnSearch = OnRegionSelectButtonPressed };
                DisplayItems[DisplayItems.IndexOf(mapRegionsDisplayItem)] = updatedDisplayItem;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Problem: ", ex.Message, "OK");
            }
            finally
            {
                ActivityIndicatorStop();
            }
        });
        public ICommand OnSelectMenuPressed => commandHelper.ProduceDebouncedCommand(async () =>
        {

            try
            {
                if (SelectedItem == null) return;
                var viewModel = new GenusKeyPageViewModel((SelectedItem as FamilyDisplayItem).Family);
                var page = new GenusKeyPage(viewModel);
                await NavigateToPageAsync(page, viewModel);

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
    

