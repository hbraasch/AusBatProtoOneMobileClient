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
    public class GenusKeyPageViewModel : ViewModelBase
    {
        Classification family;
        public class DisplayItemBase { }
        public class GenusDisplayItem: DisplayItemBase
        {
            public string GenusName { get; set; }
            public Classification Genus { get; set; }

        }

        public class BarDisplayItem : DisplayItemBase { }

        public class SpeciesDisplayItem : DisplayItemBase
        {
            public string SpeciesName { get; set; }
            public string ImageSource { get; set; }
            public Bat Bat { get; set; }
        }

        public class MapRegionsDisplayItem : DisplayItemBase
        {
            public string Description { get; set; } = "Select regions";
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
          

        public GenusKeyPageViewModel(Classification family)
        {
            this.family = family;
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

                UpdateDisplay(new List<MapRegion>());


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

        public void UpdateDisplay(List<MapRegion> selectedRegion)
        {

            DisplayItems = new ObservableCollection<DisplayItemBase>();

            foreach (var genus in App.dbase.Classifications.Where(o => o.Type == Classification.ClassificationType.Genus))
            {
                if (genus.Parent != family.Id) continue;

                List<Bat> specieses = App.dbase.GetAllSpecies(genus, selectedRegion);
                if (specieses.Count > 1) DisplayItems.Add(new GenusDisplayItem { GenusName = $"{genus.Id} ({specieses.Count})", Genus =  genus });
                if (specieses.Count == 1) DisplayItems.Add(new SpeciesDisplayItem { SpeciesName = $"{genus.Id} {specieses[0].SpeciesId.ToLower()}", ImageSource = specieses[0].Images[0]??"", Bat = specieses[0] });
            }
            DisplayItems.Add(new BarDisplayItem());
            DisplayItems.Add(new MapRegionsDisplayItem { Description = "Select regions", MapRegions = selectedRegion, SelectionAmount = $"({selectedRegion.Count} selected)", OnSearch = OnRegionSelectButtonPressed });
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
                GetSelectedRegions(ref mapRegionsDisplayItem, ref regions);
                var selectedRegions = (regions == null) ? new ObservableCollection<MapRegion>() : new ObservableCollection<MapRegion>(regions);
                var viewModel = new SelectBatRegionsPageViewModel() { SelectedMapRegions = selectedRegions };
                var page = new SelectBatRegionsPage(viewModel);
                var accept = await NavigateToPageAsync(page, viewModel);
                if (!accept) return;

                UpdateDisplay(viewModel.SelectedMapRegions.ToList());
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

        private void GetSelectedRegions(ref MapRegionsDisplayItem mapRegionsDisplayItem, ref List<MapRegion> regions)
        {
            foreach (var displayItem in DisplayItems)
            {
                if (displayItem is MapRegionsDisplayItem)
                {
                    mapRegionsDisplayItem = (MapRegionsDisplayItem)displayItem;
                    regions = (displayItem as MapRegionsDisplayItem).MapRegions;
                }
            }
        }

        private List<MapRegion> GetSelectedRegions()
        {
            MapRegionsDisplayItem mapRegionsDisplayItem = null;
            List<MapRegion> regions = new List<MapRegion>();
            GetSelectedRegions(ref mapRegionsDisplayItem, ref regions);
            return regions;
        }

        public ICommand OnSelectMenuPressed => commandHelper.ProduceDebouncedCommand(async () =>
        {

            try
            {
                if (SelectedItem == null) return;
                if (SelectedItem is MapRegionsDisplayItem)
                {
                    return;
                }
                if (SelectedItem is SpeciesDisplayItem)
                {
                    var viewModel = new DisplayBatTabbedPageViewModel((SelectedItem as SpeciesDisplayItem).Bat);
                    var page = new DisplayBatTabbed(viewModel);
                    await NavigateToPageAsync(page, viewModel);
                }
                else
                {
                    MapRegionsDisplayItem mapRegionsDisplayItem = null;
                    List<MapRegion> regions = new List<MapRegion>();
                    GetSelectedRegions(ref mapRegionsDisplayItem, ref regions);


                    var genusDisplayItem = SelectedItem as GenusDisplayItem;
                    var viewModel = new SpeciesKeyPageViewModel(genusDisplayItem.Genus, regions);
                    var page = new SpeciesKeyPage(viewModel);
                    await NavigateToPageAsync(page, viewModel);
                }


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
    

