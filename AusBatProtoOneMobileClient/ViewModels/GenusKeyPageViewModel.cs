﻿using System;
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
        public Classification Family { get; set; }
        public ObservableCollection<MapRegion> MapRegions { get; set; } = new ObservableCollection<MapRegion>();
        public abstract class CharacteristicDisplayItemBase
        {
            public int Id { get; set; }
            public int DisplayOrder { get; set; }
            public Action<CharacteristicDisplayItemBase> OnChanged { get; set; }

            public abstract bool HasEntry();
            public abstract List<Bat> ConductSearch(List<Bat> source);
        }

        public class ForeArmLengthDisplayItem : CharacteristicDisplayItemBase
        {
            public string Description { get; set; } = "Forearm length (mm)";
            public string Value { get; set; }

            public override List<Bat> ConductSearch(List<Bat> source)
            {
                float value = float.Parse(Value);
                return App.dbase.Bats.Where(o => o.ForeArmLength.Min <= value && value <= o.ForeArmLength.Max).ToList();
            }

            public override bool HasEntry() => !string.IsNullOrEmpty(Value);
        }
        public class OuterCanineWidthDisplayItem : CharacteristicDisplayItemBase
        {
            public string Description { get; set; } = "Outer canine width (mm)";
            public string Value { get; set; }
            public override bool HasEntry() => !string.IsNullOrEmpty(Value);
            public override List<Bat> ConductSearch(List<Bat> source)
            {
                float value = float.Parse(Value);
                return App.dbase.Bats.Where(o => o.OuterCanineWidth.Min <= value && value <= o.OuterCanineWidth.Max).ToList();
            }
        }
        public class TailLengthDisplayItem : CharacteristicDisplayItemBase
        {
            public string Description { get; set; } = "Tail length (mm)";
            public string Value { get; set; }
            public override bool HasEntry() => !string.IsNullOrEmpty(Value);
            public override List<Bat> ConductSearch(List<Bat> source)
            {
                float value = float.Parse(Value);
                return App.dbase.Bats.Where(o => o.TailLength.Min <= value && value <= o.TailLength.Max).ToList();
            }
        }
        public class FootWithClawLengthDisplayItem : CharacteristicDisplayItemBase
        {
            public string Description { get; set; } = "Foot with claw length (mm)";
            public string Value { get; set; }
            public override bool HasEntry() => !string.IsNullOrEmpty(Value);
            public override List<Bat> ConductSearch(List<Bat> source)
            {
                float value = float.Parse(Value);
                return App.dbase.Bats.Where(o => o.FootWithClawLength.Min <= value && value <= o.FootWithClawLength.Max).ToList();
            }
        }

        public class PenisLengthDisplayItem : CharacteristicDisplayItemBase
        {
            public string Description { get; set; } = "Penis length (mm)";
            public string Value { get; set; }
            public override bool HasEntry() => !string.IsNullOrEmpty(Value);
            public override List<Bat> ConductSearch(List<Bat> source)
            {
                float value = float.Parse(Value);
                return App.dbase.Bats.Where(o => o.PenisLength.Min <= value && value <= o.PenisLength.Max).ToList();
            }
        }
        public class HeadToBodyLengthDisplayItem : CharacteristicDisplayItemBase
        {
            public string Description { get; set; } = "Head to body length (mm)";
            public string Value { get; set; }
            public override bool HasEntry() => !string.IsNullOrEmpty(Value);
            public override List<Bat> ConductSearch(List<Bat> source)
            {
                float value = float.Parse(Value);
                return App.dbase.Bats.Where(o => o.HeadToBodyLength.Min <= value && value <= o.HeadToBodyLength.Max).ToList();
            }
        }
        public class WeightDisplayItem : CharacteristicDisplayItemBase
        {
            public string Description { get; set; } = "Weight (g)";
            public string Value { get; set; }
            public override bool HasEntry() => !string.IsNullOrEmpty(Value);
            public override List<Bat> ConductSearch(List<Bat> source)
            {
                float value = float.Parse(Value);
                return App.dbase.Bats.Where(o => o.Weight.Min <= value && value <= o.Weight.Max).ToList();
            }
        }
        public class ThreeMetDisplayItem : CharacteristicDisplayItemBase
        {
            public string Description { get; set; } = "3-MET (mm)";
            public string Value { get; set; }
            public override bool HasEntry() => !string.IsNullOrEmpty(Value);
            public override List<Bat> ConductSearch(List<Bat> source)
            {
                float value = float.Parse(Value);
                return App.dbase.Bats.Where(o => o.ThreeMet.Min <= value && value <= o.ThreeMet.Max).ToList();
            }
        }

        public class IsGularPoachPresentDisplayItem : CharacteristicDisplayItemBase
        {
            public string Description { get; set; } = "Gular Poach present?";
            public string Value { get; set; } 
            public List<string> Values { get; set; } = new List<string> { IsCharacteristicPresent.Do_not_care.ToString(), IsCharacteristicPresent.Is_present.ToString(), IsCharacteristicPresent.Is_not_present.ToString() };
            public override bool HasEntry() => Value != null && Value != IsCharacteristicPresent.Do_not_care.ToString();
            public override List<Bat> ConductSearch(List<Bat> source)
            {
                if (Value == IsCharacteristicPresent.Do_not_care.ToString())
                {
                    return App.dbase.Bats;
                }
                var value = (IsCharacteristicPresent)Enum.Parse(typeof(IsCharacteristicPresent), Value);
                return App.dbase.Bats.Where(o => o.IsGularPoachPresent == value).ToList();
            }
        }
        public class HasFleshyGenitalProjectionsDisplayItem : CharacteristicDisplayItemBase
        {
            public string Description { get; set; } = "Has fleshy genital projections?";
            public string Value { get; set; }
            public List<string> Values { get; set; } = new List<string> { IsCharacteristicPresent.Do_not_care.ToString(), IsCharacteristicPresent.Is_present.ToString(), IsCharacteristicPresent.Is_not_present.ToString() };
            public override bool HasEntry() => Value != null && Value != IsCharacteristicPresent.Do_not_care.ToString();
            public override List<Bat> ConductSearch(List<Bat> source)
            {
                if (Value == IsCharacteristicPresent.Do_not_care.ToString())
                {
                    return App.dbase.Bats;
                }
                var value = (IsCharacteristicPresent) Enum.Parse(typeof(IsCharacteristicPresent), Value);
                return App.dbase.Bats.Where(o => o.BothSexesHasFleshyGenitalProjections == value).ToList();
            }
        }

        public ObservableCollection<CharacteristicDisplayItemBase> CharacteristicDisplayItems { get; set; }

        public CharacteristicDisplayItemBase CharacteristicSelectedItem { get; set; }


        public void OnSelectedItemChanged()
        {
            IsSelected = CharacteristicSelectedItem != null;
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
            Family = family;
            CharacteristicDisplayItems = new ObservableCollection<CharacteristicDisplayItemBase>();
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

                CharacteristicDisplayItems = GenerateCharacteristicDisplay();
                
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

        public ObservableCollection<CharacteristicDisplayItemBase> GenerateCharacteristicDisplay()
        {
            var displayItems = new ObservableCollection<CharacteristicDisplayItemBase>();
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

        private void OnCharacteristicChanged(CharacteristicDisplayItemBase characteristic)
        {
            switch (characteristic)
            {
                default:
                    break;
            }
            throw new NotImplementedException();
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
            NavigateBack( NavigateReturnType.IsCancelled);
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


        public ICommand OnSpecifyRegionClicked => commandHelper.ProduceDebouncedCommand(async () =>
        {
            try
            {
                var viewModel = new SelectBatRegionsPageViewModel() { SelectedMapRegions = MapRegions };
                var page = new SelectBatRegionsPage(viewModel);
                var returnType = await NavigateToPageAsync(page, viewModel);
                if (returnType == NavigateReturnType.IsCancelled) return;
                MapRegions = viewModel.SelectedMapRegions;
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

        public ICommand OnClearFiltersClicked => commandHelper.ProduceDebouncedCommand(async () =>
        {
            try
            {
                CharacteristicDisplayItems = GenerateCharacteristicDisplay();
                MapRegions.Clear();

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
        public ICommand OnFilterNowClicked => commandHelper.ProduceDebouncedCommand(async () =>
        {
            try
            {
                ActivityIndicatorStart();

                var speciesInFamily = App.dbase.GetAllSpeciesInFamily(Family);

                List<Bat> searchResult = ConductSearch(speciesInFamily);
                var viewModel = new DisplayFilteredSpeciesPageViewModel(searchResult) { IsHomeEnabled = true };
                var page = new DisplayFilteredSpeciesPage(viewModel);
                var resultType = await NavigateToPageAsync(page, viewModel);
                if (resultType == NavigateReturnType.GotoRoot) NavigateBack(NavigateReturnType.GotoRoot);
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

        private List<Bat> ConductSearch(List<Bat> source)
        {
            List<CharacteristicDisplayItemBase> activeCharacteristic = new List<CharacteristicDisplayItemBase>();
            foreach (var displayItem in CharacteristicDisplayItems)
            {
                if (displayItem.HasEntry()) activeCharacteristic.Add(displayItem);
            }

            if (activeCharacteristic.Count == 0) return source;

            List<Bat> currentResults = source;
            foreach (var characteristic in activeCharacteristic)
            {
                currentResults = characteristic.ConductSearch(currentResults);
                if (currentResults == null) return new List<Bat>();
            }
            if (MapRegions.Count > 0)
            {
                currentResults = Dbase.Filter(currentResults, MapRegions.ToList());
                if (currentResults.IsEmpty()) return new List<Bat>();
            }
            return currentResults;
        }

        private ObservableCollection<CharacteristicDisplayItemBase> UpdateCharacteristicDisplay()
        {
            return CharacteristicDisplayItems;
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


