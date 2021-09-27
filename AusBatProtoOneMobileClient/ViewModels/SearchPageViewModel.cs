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
    public class SearchPageViewModel : ViewModelBase
    {

        public string SearchBarText { get; set; }
        public abstract class CriteriaDisplayItemBase
        {
            public int Id { get; set; }
            public int DisplayOrder { get; set; }
            public Action<CriteriaDisplayItemBase> OnChanged { get; set; }

            public abstract bool HasEntry();
            public abstract List<Bat> ConductSearch();
        }
        public class MapRegionsDisplayItem: CriteriaDisplayItemBase
        {
            public string Description { get; set; } = "Regions";
            public List<MapRegion> MapRegions { get; set; }
            public string SelectionAmount { get; set; }
            public ICommand OnSearch { get; set; }

            public override List<Bat> ConductSearch()
            {
                return App.dbase.Bats.Where(o => o.MapRegions.Intersect(MapRegions, new RegionComparer()).Count() > 0).ToList();
            }

            public override bool HasEntry() => MapRegions?.Count > 0;
        }
        public class ForeArmLengthDisplayItem : CriteriaDisplayItemBase
        {
            public string Description { get; set; } = "Forearm length (mm)";
            public string Value { get; set; }

            public override List<Bat> ConductSearch()
            {
                float value = float.Parse(Value);
                return App.dbase.Bats.Where(o => o.ForeArmLength.Min <= value && value <= o.ForeArmLength.Max).ToList();
            }

            public override bool HasEntry() => Value != "";
        }
        public class OuterCanineWidthDisplayItem : CriteriaDisplayItemBase
        {
            public string Description { get; set; } = "Outer canine width (mm)";
            public string Value { get; set; }
            public override bool HasEntry() => Value != "";
            public override List<Bat> ConductSearch()
            {
                float value = float.Parse(Value);
                return App.dbase.Bats.Where(o => o.OuterCanineWidth.Min <= value && value <= o.OuterCanineWidth.Max).ToList();
            }
        }
        public class TailLengthDisplayItem : CriteriaDisplayItemBase
        {
            public string Description { get; set; } = "Tail length (mm)";
            public string Value { get; set; }
            public override bool HasEntry() => Value != "";
            public override List<Bat> ConductSearch()
            {
                float value = float.Parse(Value);
                return App.dbase.Bats.Where(o => o.TailLength.Min <= value && value <= o.TailLength.Max).ToList();
            }
        }
        public class FootWithClawLengthDisplayItem : CriteriaDisplayItemBase
        {
            public string Description { get; set; } = "Foot with claw length (mm)";
            public string Value { get; set; }
            public override bool HasEntry() => Value != "";
            public override List<Bat> ConductSearch()
            {
                float value = float.Parse(Value);
                return App.dbase.Bats.Where(o => o.FootWithClawLength.Min <= value && value <= o.FootWithClawLength.Max).ToList();
            }
        }

        public class PenisLengthDisplayItem : CriteriaDisplayItemBase
        {
            public string Description { get; set; } = "Penis length (mm)";
            public string Value { get; set; }
            public override bool HasEntry() => Value != "";
            public override List<Bat> ConductSearch()
            {
                float value = float.Parse(Value);
                return App.dbase.Bats.Where(o => o.PenisLength.Min <= value && value <= o.PenisLength.Max).ToList();
            }
        }
        public class HeadToBodyLengthDisplayItem : CriteriaDisplayItemBase
        {
            public string Description { get; set; } = "Head to body length (mm)";
            public string Value { get; set; }
            public override bool HasEntry() => Value != "";
            public override List<Bat> ConductSearch()
            {
                float value = float.Parse(Value);
                return App.dbase.Bats.Where(o => o.HeadToBodyLength.Min <= value && value <= o.HeadToBodyLength.Max).ToList();
            }
        }
        public class WeightDisplayItem : CriteriaDisplayItemBase
        {
            public string Description { get; set; } = "Weight (g)";
            public string Value { get; set; }
            public override bool HasEntry() => Value != "";
            public override List<Bat> ConductSearch()
            {
                float value = float.Parse(Value);
                return App.dbase.Bats.Where(o => o.Weight.Min <= value && value <= o.Weight.Max).ToList();
            }
        }
        public class ThreeMetDisplayItem : CriteriaDisplayItemBase
        {
            public string Description { get; set; } = "3-MET (mm)";
            public string Value { get; set; }
            public override bool HasEntry() => Value != "";
            public override List<Bat> ConductSearch()
            {
                float value = float.Parse(Value);
                return App.dbase.Bats.Where(o => o.ThreeMet.Min <= value && value <= o.ThreeMet.Max).ToList();
            }
        }

        public class IsGularPoachPresentDisplayItem : CriteriaDisplayItemBase
        {
            public string Description { get; set; } = "Gular Poach present?";
            public IsPresent Value { get; set; } = IsPresent.DoNotCare;
            public List<IsPresent> Values { get; set; } = new List<IsPresent> { IsPresent.DoNotCare, IsPresent.IsPresent, IsPresent.IsNotPresent };
            public override bool HasEntry() => Value != IsPresent.DoNotCare;
            public override List<Bat> ConductSearch()
            {
                if (Value == IsPresent.DoNotCare)
                {
                    return App.dbase.Bats;
                }
                return App.dbase.Bats.Where(o => o.IsGularPoachPresent == Value).ToList();
            }
        }
        public class HasFleshyGenitalProjectionsDisplayItem : CriteriaDisplayItemBase
        {
            public string Description { get; set; } = "Has fleshy genital projections?";
            public IsPresent Value { get; set; } = IsPresent.DoNotCare;
            public List<IsPresent> Values { get; set; } = new List<IsPresent> { IsPresent.DoNotCare, IsPresent.IsPresent, IsPresent.IsNotPresent };
            public override bool HasEntry() => Value != IsPresent.DoNotCare;
            public override List<Bat> ConductSearch()
            {
                if (Value == IsPresent.DoNotCare)
                {
                    return App.dbase.Bats;
                }
                return App.dbase.Bats.Where(o => o.HasFleshyGenitalProjections == Value).ToList();
            }
        }

        public ObservableCollection<CriteriaDisplayItemBase> CriteriaDisplayItems { get; set; }

        public CriteriaDisplayItemBase CriteriaSelectedItem { get; set; }


        public class ResultDisplayItem
        {
            public string Description { get; set; }
            public object Content { get; set; }
        }
        public ObservableCollection<ResultDisplayItem> ResultsDisplayItems { get; set; }

        public ResultDisplayItem ResultsSelectedItem { get; set; }

        public void OnSelectedItemChanged()
        {
            IsSelected = CriteriaSelectedItem != null;
            InvalidateMenu.Execute(null);
        }


        public bool IsDirty { get; set; }

        public ICommand InvalidateMenu { get; set; }

        CommandHelper commandHelper = new CommandHelper();

        CancellationTokenSource cts;


        #region *// Menu related 

        public bool IsSelected { get; set; }

        #endregion


        public SearchPageViewModel()
        {
            CriteriaDisplayItems = new ObservableCollection<CriteriaDisplayItemBase>();
            ResultsDisplayItems = new ObservableCollection<ResultDisplayItem>();
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

                CriteriaDisplayItems = GenerateCriteriaDisplay();
                
                ResultsDisplayItems = UpdateResultsDisplay();

            }
            catch (Exception ex) when (ex is TaskCanceledException ext)
            {
                Debug.Write("Cancelled by user");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.CompleteMessage(), "Ok");
            }
            finally
            {
                ActivityIndicatorStop();
            }
        });

        public ObservableCollection<CriteriaDisplayItemBase> GenerateCriteriaDisplay()
        {
            var displayItems = new ObservableCollection<CriteriaDisplayItemBase>();
            displayItems.Add(new MapRegionsDisplayItem() { OnSearch = OnRegionSelectButtonPressed });
            displayItems.Add(new ForeArmLengthDisplayItem());
            displayItems.Add(new OuterCanineWidthDisplayItem());
            displayItems.Add(new TailLengthDisplayItem());
            displayItems.Add(new FootWithClawLengthDisplayItem());
            displayItems.Add(new PenisLengthDisplayItem());
            displayItems.Add(new HeadToBodyLengthDisplayItem());
            displayItems.Add(new WeightDisplayItem());
            displayItems.Add(new ThreeMetDisplayItem());
            displayItems.Add(new IsGularPoachPresentDisplayItem());
            displayItems.Add(new HasFleshyGenitalProjectionsDisplayItem()); 

            displayItems.OrderBy(o=>o.DisplayOrder);
            return displayItems;
        }

        private void OnCriteriaChanged(CriteriaDisplayItemBase criteria)
        {
            switch (criteria)
            {
                default:
                    break;
            }
            throw new NotImplementedException();
        }

        public ObservableCollection<ResultDisplayItem> UpdateResultsDisplay(List<Bat> searchResults = null)
        {
            var displayItems = new ObservableCollection<ResultDisplayItem>();
            if (searchResults == null) return displayItems;

            foreach (var searchResult in searchResults)
            {
                displayItems.Add(new ResultDisplayItem { Description = searchResult.Name, Content = searchResult });
            }

            return displayItems;
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
                foreach (var displayItem in CriteriaDisplayItems)
                {
                    if (displayItem is MapRegionsDisplayItem)
                    {
                        mapRegionsDisplayItem = (MapRegionsDisplayItem) displayItem;
                        regions = (displayItem as MapRegionsDisplayItem).MapRegions;
                    }
                }
                var selectedRegions = (regions == null) ? new ObservableCollection<MapRegion>(): new ObservableCollection<MapRegion>(regions) ;
                var viewModel = new SelectBatRegionsPageViewModel() { SelectedMapRegions = selectedRegions };
                var page = new SelectBatRegionsPage(viewModel);
                var accept = await NavigateToPageAsync(page, viewModel);
                if (!accept) return;

                var updatedDisplayItem = new MapRegionsDisplayItem { Description = mapRegionsDisplayItem.Description, MapRegions = viewModel.SelectedMapRegions.ToList(), SelectionAmount = $"{viewModel.SelectedMapRegions.Count} selected", OnSearch = OnRegionSelectButtonPressed };
                CriteriaDisplayItems[CriteriaDisplayItems.IndexOf(mapRegionsDisplayItem)] = updatedDisplayItem;
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

        public ICommand OnSearchButtonPressed => commandHelper.ProduceDebouncedCommand(async () =>
        {
            try
            {
                ActivityIndicatorStart();

                List<Bat> searchResult = ConductSearch();
                ResultsDisplayItems = UpdateResultsDisplay(searchResult);
                CriteriaDisplayItems = UpdateCriteriaDisplay();
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

        private List<Bat> ConductSearch()
        {
            List<CriteriaDisplayItemBase> activeCriteria = new List<CriteriaDisplayItemBase>();
            foreach (var displayItem in CriteriaDisplayItems)
            {              
                if (displayItem is MapRegionsDisplayItem)
                {
                    if (displayItem.HasEntry()) activeCriteria.Add(displayItem); 
                }
            }

            List<Bat> currentResults = new List<Bat>();
            foreach (var criteria in activeCriteria)
            {
                var interimResult = criteria.ConductSearch();
                if (interimResult.Count == 0) return new List<Bat>();
                if (currentResults.Count == 0)
                {
                    currentResults = interimResult;
                }
                else
                {
                    currentResults = (List<Bat>)currentResults.Intersect(interimResult, new BatComparer());
                    if (currentResults.Count == 0) return new List<Bat>();
                }
            }
            return currentResults;
        }

        private ObservableCollection<CriteriaDisplayItemBase> UpdateCriteriaDisplay()
        {
            return CriteriaDisplayItems;
        }

        public ICommand OnEditMenuPressed => commandHelper.ProduceDebouncedCommand(async () =>
        {
            try
            {
                if (CriteriaSelectedItem == null) return;
                ActivityIndicatorStart();


                GenerateCriteriaDisplay();

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


        public ICommand OnDeleteMenuPressed => commandHelper.ProduceDebouncedCommand(async () =>
        {
            try
            {

                GenerateCriteriaDisplay();

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

        public ICommand OnResultsListTapped => commandHelper.ProduceDebouncedCommand(async () =>
        {

            try
            {
                var bat = ResultsSelectedItem.Content as Bat;
                var viewModel = new DisplayBatTabbedPageViewModel(bat);
                var page = new DisplayBatTabbed(viewModel);
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

    internal class RegionComparer : IEqualityComparer<MapRegion>
    {
        public bool Equals(MapRegion x, MapRegion y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(MapRegion obj)
        {
            return obj.Id.GetHashCode();
        }
    }

    internal class BatComparer : IEqualityComparer<Bat>
    {
        public bool Equals(Bat x, Bat y)
        {
            return x.Name == y.Name ;
        }

        public int GetHashCode(Bat obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}


