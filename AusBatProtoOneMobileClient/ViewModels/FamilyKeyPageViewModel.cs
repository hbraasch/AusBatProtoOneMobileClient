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
        public ObservableCollection<MapRegion> MapRegions { get; set; } = new ObservableCollection<MapRegion>();
        public abstract class CharacterDisplayItemBase
        {
            public int Id { get; set; }
            public int DisplayOrder { get; set; }
            public Action<CharacterDisplayItemBase> OnChanged { get; set; }
            public abstract List<Classification> ConductSearch(List<Classification> source);
        }
        public class TailPresentCharacterDisplayItem : CharacterDisplayItemBase
        {
            public string Description { get; set; } = "Tail present/absent";

            public TailPresentCharacter Value { get; set; }
            public List<string> Values { get; set; } = TailPresentCharacter.Prompts;
            public List<string> ImageSources { get; set; } = TailPresentCharacter.ImageSources;

            public override List<Classification> ConductSearch(List<Classification> source)
            {
                var result = new List<Classification>();
                if (Value.Key == TailPresentCharacter.TailPresentEnum.Undefined) return source;
                foreach (var family in source)
                {
                    if (Value.ExistsIn(family.Characters)) result.Add(family);
                }
                return result;
            }

        }
        public class TailMembraneStructureCharacterDisplayItem : CharacterDisplayItemBase
        {
            public string Description { get; set; } = "Tail/Membrane structure";
            public TailMembraneStructureCharacter Value { get; set; }
            public List<string> Values { get; set; } = TailMembraneStructureCharacter.Prompts;

            public override List<Classification> ConductSearch(List<Classification> source)
            {
                var result = new List<Classification>();
                if (Value.Key == TailMembraneStructureCharacter.TailMembraneStructureEnum.Undefined) return source;
                foreach (var family in source)
                {
                    if (Value.ExistsIn(family.Characters)) result.Add(family);
                }
                return result;
            }

        }
        public class SecondFingerClawCharacterDisplayItem : CharacterDisplayItemBase
        {
            public string Description { get; set; } = "Second finger claw";
            public SecondFingerClawCharacter Value { get; set; }
            public List<string> Values { get; set; } = SecondFingerClawCharacter.Prompts;

            public override List<Classification> ConductSearch(List<Classification> source)
            {
                var result = new List<Classification>();
                if (Value.Key == SecondFingerClawCharacter.SecondFingerClawEnum.Undefined) return source;
                foreach (var family in source)
                {
                    if (Value.ExistsIn(family.Characters)) result.Add(family);
                }
                return result;
            }

        }
        public class FaceStructureNoseLeafCharacterDisplayItem : CharacterDisplayItemBase
        {
            public string Description { get; set; } = "Face structure / NoseLeaf";
            public FaceStructureNoseLeafCharacter Value { get; set; }
            public List<string> Values { get; set; } = FaceStructureNoseLeafCharacter.Prompts;

            public override List<Classification> ConductSearch(List<Classification> source)
            {
                var result = new List<Classification>();
                if (Value.Key == FaceStructureNoseLeafCharacter.FaceStructureNoseLeafEnum.Undefined) return source;
                foreach (var family in source)
                {
                    if (Value.ExistsIn(family.Characters)) result.Add(family);
                }
                return result;
            }

        }
        public class WingThirdFingerCharacterDisplayItem : CharacterDisplayItemBase
        {
            public string Description { get; set; } = "Wing - third finger";
            public WingThirdFingerCharacter Value { get; set; }
            public List<string> Values { get; set; } = WingThirdFingerCharacter.Prompts;

            public override List<Classification> ConductSearch(List<Classification> source)
            {
                var result = new List<Classification>();
                if (Value.Key == WingThirdFingerCharacter.WingThirdFingerEnum.Undefined) return source;
                foreach (var family in source)
                {
                    if (Value.ExistsIn(family.Characters)) result.Add(family);
                }
                return result;
            }

        }
        public class TragusCharacterDisplayItem : CharacterDisplayItemBase
        {
            public string Description { get; set; } = "Tragus";
            public TragusCharacter Value { get; set; }
            public List<string> Values { get; set; } = TragusCharacter.Prompts;
            public List<string> ImageSources { get; set; } = TragusCharacter.ImageSources;

            public override List<Classification> ConductSearch(List<Classification> source)
            {
                var result = new List<Classification>();
                if (Value.Key == TragusCharacter.TragusEnum.Undefined) return source;
                foreach (var family in source)
                {
                    if (Value.ExistsIn(family.Characters)) result.Add(family);
                }
                return result;
            }

        }
        public ObservableCollection<CharacterDisplayItemBase> CharacterDisplayItems { get; set; }



        public ICommand InvalidateMenu { get; set; }

        CommandHelper commandHelper = new CommandHelper();

        CancellationTokenSource cts;


        #region *// Menu related 

        public bool IsSelected { get; set; }

        #endregion


        public FamilyKeyPageViewModel()
        {
            CharacterDisplayItems = new ObservableCollection<CharacterDisplayItemBase>();
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

                CharacterDisplayItems = GenerateCharacterDisplay();
                

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

        public ObservableCollection<CharacterDisplayItemBase> GenerateCharacterDisplay()
        {
            var displayItems = new ObservableCollection<CharacterDisplayItemBase>();
            displayItems.Add(new TailPresentCharacterDisplayItem());
            displayItems.Add(new TailMembraneStructureCharacterDisplayItem());
            displayItems.Add(new SecondFingerClawCharacterDisplayItem());
            displayItems.Add(new FaceStructureNoseLeafCharacterDisplayItem());
            displayItems.Add(new WingThirdFingerCharacterDisplayItem());
            displayItems.Add(new TragusCharacterDisplayItem());

            displayItems.OrderBy(o=>o.DisplayOrder);
            return displayItems;
        }

        private void OnCharacterChanged(CharacterDisplayItemBase Character)
        {
            switch (Character)
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
            NavigateBack(NavigateReturnType.IsCancelled);
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
                CharacterDisplayItems = GenerateCharacterDisplay();
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

                List<Classification> searchResult = ConductSearch();
                var viewModel = new DisplayFamilyPageViewModel(searchResult) { IsHomeEnabled = true };
                var page = new DisplayFamilyPage(viewModel);
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

        private List<Classification> ConductSearch()
        {

            List<Classification> currentFamilyResults = App.dbase.Classifications.Where(o=>o.Type == Classification.ClassificationType.Family).ToList();
            foreach (var Character in CharacterDisplayItems)
            {
                currentFamilyResults = Character.ConductSearch(currentFamilyResults);
                if (currentFamilyResults.Count == 0) return new List<Classification>();                
            }
            if (MapRegions.Count > 0)
            {
                var familiesInRegions = new List<Classification>();
                // Get all bats in these regions
                var speciesesInRegions = App.dbase.GetAllSpecies(MapRegions.ToList());
                foreach (var species in speciesesInRegions)
                {
                    #region // Get the species family 
                    var speciesFamily = species.GetFamily(App.dbase);
                    #endregion
                    if (!familiesInRegions.Exists(o=>o.Id == speciesFamily.Id))
                    {
                        familiesInRegions.Add(speciesFamily);
                    }
                }
                currentFamilyResults = currentFamilyResults.Intersect(familiesInRegions, new ClassificationComparer()).ToList();
                if (currentFamilyResults.IsEmpty()) return new List<Classification>();
            }
            return currentFamilyResults;
        }

        private ObservableCollection<CharacterDisplayItemBase> UpdateCharacterDisplay()
        {
            return CharacterDisplayItems;
        }


    }


    internal class ClassificationComparer : IEqualityComparer<Classification>
    {
        public bool Equals(Classification x, Classification y)
        {
            return x.Id == y.Id ;
        }

        public int GetHashCode(Classification obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}


