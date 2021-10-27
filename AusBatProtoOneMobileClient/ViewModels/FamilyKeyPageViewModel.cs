using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using AusBatProtoOneMobileClient;
using AusBatProtoOneMobileClient.ViewModels;
using Mobile.ViewModels;
using Mobile.Helpers;
using TreeApp.Helpers;
using AusBatProtoOneMobileClient.Models;
using static AusBatProtoOneMobileClient.Models.KeyTree;
using AusBatProtoOneMobileClient.Models.Touch;
using static DocGenOneMobileClient.Views.FamilyKeyPageViewModel.FilterSnapShots;

namespace DocGenOneMobileClient.Views
{
    public class FamilyKeyPageViewModel : ViewModelBase
    {
        const string FILTER_TITLE = "Filter";
        const string NO_RESULTS_YET = "(0) taxa";

        public enum FilterState
        {
            StartFromScratch, StartNextLevel, HasMatches, NoMoreMatches, 
        }
        private FilterState State = FilterState.StartFromScratch;

        public KeyTreeNodeBase CurrentPromptKeyTreeNode;
        public List<KeyTreeNodeBase> CurrentTriggeredKeyTreeNodes = new List<KeyTreeNodeBase>();

        public List<CharacterPromptBase> UsedPromptCharacters = new List<CharacterPromptBase>();
        public List<CharacterPromptBase> BestPromptCharacters = new List<CharacterPromptBase>();
        public List<CharacterPromptBase> RemovedPromptCharacters = new List<CharacterPromptBase>();
        public bool HasRegionFilterBeenUsed = false;
        private FilterSnapShots SnapShots { get; set; } = new FilterSnapShots();

        public List<int> CurrentRegionIds { get; set; } = new List<int>();
        public abstract class CharacterDisplayItemBase
        {
            public virtual string Prompt { get; set; }
            public int DisplayOrder { get; set; }
            public string ImageSource { get; set; }
            public Action<ImageSource> OnImageClicked { get; set; }
            public ICommand OnChanged { get; set; }
            public CharacterPromptBase Content;
            public abstract bool HasEntry();
        }
        public class PickerDisplayItem : CharacterDisplayItemBase
        {
            public string SelectedOption { get; set; }
            public List<string> Options { get; set; } = new List<string>();
            public List<string> OptionIds { get; set; } = new List<string>();
            public List<string> ImageSources { get; set; } = new List<string>();

            public override bool HasEntry()
            {
                return SelectedOption != "";
            }
        }
        public class NumericDisplayItem : CharacterDisplayItemBase
        {
            public string Value { get; set; }

            public bool IsImageVisible { get; set; }

            public override bool HasEntry()
            {
                return Value != "";
            }
        }
        public class MapRegionsDisplayItem : CharacterDisplayItemBase
        {
            public override string Prompt { get; set; } = "Regions";
            public List<int> RegionIds { get; set; } = new List<int>();
            public string SelectionAmount { get; set; }
            public ICommand OnSelectRegionClicked { get; set; }
            public override bool HasEntry() => RegionIds?.Count > 0;

        }

        public ObservableCollection<CharacterDisplayItemBase> CharacterDisplayItems { get; set; }
        public string FilterHint { get; set; }
        public string FilterResult { get; set; }
        public string Title { get; set; }

        public ICommand InvalidateMenuCommand { get; set; }

        CommandHelper commandHelper = new CommandHelper();

        CancellationTokenSource cts;


        #region *// Menu related 

        public bool IsResetFilterEnabled => UsedPromptCharacters.Count > 0 || CurrentPromptKeyTreeNode.NodeId != App.dbase.KeyTree.RootNode.NodeId;
        public bool CanUndo => SnapShots.Count  > 0;
        #endregion

        public FamilyKeyPageViewModel()
        {

        }

        public FamilyKeyPageViewModel(KeyTreeNodeBase currentPromptKeyTreeNode, List<int> currentRegionIds)
        {
            CurrentPromptKeyTreeNode = currentPromptKeyTreeNode;
            CurrentTriggeredKeyTreeNodes = new List<KeyTreeNodeBase>();
            CurrentRegionIds = currentRegionIds;
            CharacterDisplayItems = new ObservableCollection<CharacterDisplayItemBase>();
            Title = $"{FILTER_TITLE}:";
            State = FilterState.StartFromScratch;
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

                CharacterDisplayItems = UpdateCharacterDisplay(CurrentPromptKeyTreeNode);
                FilterResult = NO_RESULTS_YET;
                FilterHint = CurrentPromptKeyTreeNode.FilterHint;

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

        private string UpdateFilterResults()
        {
            return $"({CurrentTriggeredKeyTreeNodes.Count}) taxa";
        }

        public ObservableCollection<CharacterDisplayItemBase> UpdateCharacterDisplay(KeyTreeNodeBase currentPromptKeyTreeNode)
        {
            var displayItems = new ObservableCollection<CharacterDisplayItemBase>();

            switch (State)
            {
                case FilterState.StartFromScratch:
                    BestPromptCharacters = currentPromptKeyTreeNode.PromptCharactersForNextLevel.ToList();
                    HasRegionFilterBeenUsed = false;
                    SnapShots.Clear();
                    break;
                case FilterState.StartNextLevel:
                    SnapShots.Clear();
                    #region *// Filter on region if regions selected
                    if (CurrentRegionIds.Count > 0)
                    {
                        ConductRegionSearch(currentPromptKeyTreeNode, CurrentRegionIds, ref CurrentTriggeredKeyTreeNodes);
                        HasRegionFilterBeenUsed = true;
                    }
                    #endregion
                    BestPromptCharacters = currentPromptKeyTreeNode.PromptCharactersForNextLevel.ToList();
                    break;
                case FilterState.HasMatches:
                    #region *// Determine which keys needs to be displayed
                    List<CharacterPromptBase> AllPromptCharacters = currentPromptKeyTreeNode.PromptCharactersForNextLevel.ToList();
                    var inEffectivePromptCharacters = KeyTree.GetIneffectivePromptCharacters(CurrentPromptKeyTreeNode, CurrentTriggeredKeyTreeNodes, UsedPromptCharacters);
                    RemovedPromptCharacters.Clear();
                    RemovedPromptCharacters.AddRange(UsedPromptCharacters);
                    RemovedPromptCharacters.AddRange(inEffectivePromptCharacters);

                    BestPromptCharacters.Clear();
                    foreach (var allPromptCharacter in AllPromptCharacters)
                    {
                        if (!RemovedPromptCharacters.Exists(o => o.KeyId == allPromptCharacter.KeyId))
                        {
                            BestPromptCharacters.Add(allPromptCharacter);
                        }
                    }
                    #endregion
                    break;
                case FilterState.NoMoreMatches:
                    BestPromptCharacters.Clear();
                    HasRegionFilterBeenUsed = true;
                    break;
                default:
                    break;
            }




            #region *// Create display items

            foreach (var character in BestPromptCharacters.OrderBy(o => o.DisplayOrder))
            {
                if (character is NumericCharacterPrompt ncp)
                {
                    displayItems.Add(new NumericDisplayItem
                    {
                        Prompt = ncp.Prompt,
                        ImageSource = ncp.ImageSource,
                        Value = "",
                        OnChanged = OnFilterClicked,
                        OnImageClicked = OnImageClicked,
                        Content = character
                    });
                }
                else if (character is PickerCharacterPrompt pcp)
                {
                    displayItems.Add(new PickerDisplayItem
                    {
                        Prompt = pcp.Prompt,
                        ImageSource = pcp.ImageSource,
                        SelectedOption = "",
                        Options = pcp.Options.Select(o => o.OptionPrompt).ToList(),
                        OptionIds = pcp.Options.Select(o => o.OptionId).ToList(),
                        ImageSources = pcp.Options.Select(o => o.OptionImageSource).ToList(),
                        OnChanged = OnFilterClicked,
                        OnImageClicked = OnImageClicked,
                        Content = character
                    });
                }
            } 


            if (!HasRegionFilterBeenUsed && CurrentTriggeredKeyTreeNodes.Count != 1)
            {
                displayItems.Add(new MapRegionsDisplayItem
                {
                    RegionIds = CurrentRegionIds,
                    OnChanged = OnFilterClicked,
                    OnSelectRegionClicked = OnGetRegionsFromUserClicked
                });
            } 
            #endregion

            displayItems.OrderBy(o => o.DisplayOrder);

            FilterResult = UpdateFilterResults();

            return displayItems;
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

        public bool isBackCancelled = false;
        public ICommand OnBackButtonPressed => new Command(() =>
        {
            isBackCancelled = true;
        });

        public bool IsHomeEnabled { get; set; }
        public ICommand OnHomeMenuPressed => new Command(() =>
        {
            NavigateBack(NavigateReturnType.GotoRoot);
        });

        public ICommand OnGetRegionsFromUserClicked => commandHelper.ProduceDebouncedCommand(async () =>
        {
            try
            {
                var viewModel = new SelectBatRegionsPageViewModel() { SelectedMapRegions = new ObservableCollection<MapRegion>(App.dbase.MapRegions.Where(o=>CurrentRegionIds.Contains(o.Id))) };
                var page = new SelectBatRegionsPage(viewModel);
                var returnType = await NavigateToPageAsync(page, viewModel);
                if (returnType == NavigateReturnType.IsCancelled) return;
                if (returnType == NavigateReturnType.GotoRoot) { NavigateBack(NavigateReturnType.GotoRoot); return; }

                #region *// Return result
                var regionDisplayItem = CharacterDisplayItems.FirstOrDefault(o => o is MapRegionsDisplayItem) as MapRegionsDisplayItem;
                #endregion
                regionDisplayItem.RegionIds =  CurrentRegionIds = viewModel.SelectedMapRegions.Select(o=>o.Id).ToList();
                OnFilterClicked.Execute(null);
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

        public ICommand OnResetFiltersClicked => commandHelper.ProduceDebouncedCommand(async () =>
        {
            try
            {
                ResetFilter(FilterState.StartFromScratch, KeyTreeFilter.Current.GetFilterResetNode());

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

        public ICommand OnUndoFilterActionClicked => commandHelper.ProduceDebouncedCommand(async () =>
        {
            try
            {
                var snapshot = SnapShots.Pop();
                State = snapshot.BeforeFilterState;
                CurrentPromptKeyTreeNode = snapshot.BeforePromptKeyTreeNode;
                UsedPromptCharacters = snapshot.BeforeUsedPromptCharacters;
                CurrentTriggeredKeyTreeNodes = snapshot.BeforeTriggeredKeyTreeNodes;
                HasRegionFilterBeenUsed = snapshot.BeforeHasRegionFilterBeenUsed;

                CharacterDisplayItems = UpdateCharacterDisplay(CurrentPromptKeyTreeNode);
                FilterHint = CurrentPromptKeyTreeNode.FilterHint;

                InvalidateMenuCommand.Execute(null);
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

        

        private void ResetFilter(FilterState state, KeyTreeNodeBase rootNode )
        {
            State = state;
            CurrentPromptKeyTreeNode = rootNode;
            UsedPromptCharacters.Clear();
            CurrentTriggeredKeyTreeNodes.Clear();
            if (state == FilterState.StartFromScratch)
            {
                CurrentRegionIds.Clear();
                HasRegionFilterBeenUsed = false;
                FilterResult = NO_RESULTS_YET;
            }
            SnapShots.Clear();

            CharacterDisplayItems = UpdateCharacterDisplay(CurrentPromptKeyTreeNode);
            FilterHint = CurrentPromptKeyTreeNode.FilterHint;
            Title = $"{FILTER_TITLE}:";

            InvalidateMenuCommand.Execute(null);
        }
        public ICommand OnFilterClicked => commandHelper.ProduceDebouncedCommand(async () =>
        {
            try
            {
                await ActivityIndicatorStart();

                var activeCharacterDisplayItem = CharacterDisplayItems.FirstOrDefault(o => o.HasEntry());
                var snapShot = new FilterSnapShot { BeforePromptKeyTreeNode = CurrentPromptKeyTreeNode, BeforeTriggeredKeyTreeNodes = CurrentTriggeredKeyTreeNodes.ToList(), BeforeUsedPromptCharacters = UsedPromptCharacters.ToList(), BeforeHasRegionFilterBeenUsed = HasRegionFilterBeenUsed, BeforeFilterState = State };
                ConductSearch(CurrentPromptKeyTreeNode, activeCharacterDisplayItem, ref CurrentTriggeredKeyTreeNodes, ref UsedPromptCharacters, ref HasRegionFilterBeenUsed);
                SnapShots.Push(snapShot);
                CharacterDisplayItems = UpdateCharacterDisplay(CurrentPromptKeyTreeNode);
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

        private void ConductSearch(KeyTreeNodeBase currentPromptKeyTreeNode, CharacterDisplayItemBase activeCharacterDisplayItem, ref List<KeyTreeNodeBase> currentTriggeredKeyTreeNodes, ref List<CharacterPromptBase>usedPromptCharacters, ref bool hasRegionFilterBeenUsed)
        {
            if (activeCharacterDisplayItem == null) return;

            if (activeCharacterDisplayItem is PickerDisplayItem pdi)
            {
                var selectedOptionId = pdi.OptionIds[pdi.Options.IndexOf(pdi.SelectedOption)];
                var activeCharacter = activeCharacterDisplayItem.Content as PickerCharacterPrompt;
                activeCharacter.EntryOptionId = selectedOptionId;
                currentTriggeredKeyTreeNodes = currentPromptKeyTreeNode.GetTriggeredNodesUsingEntries(currentTriggeredKeyTreeNodes, activeCharacter);
                usedPromptCharacters = KeyTree.AddCharacterUnique(usedPromptCharacters, activeCharacterDisplayItem.Content);

            }
            else if (activeCharacterDisplayItem is NumericDisplayItem ndi)
            {
                var activeCharacter = activeCharacterDisplayItem.Content as NumericCharacterPrompt;
                activeCharacter.Entry = float.Parse(ndi.Value);
                currentTriggeredKeyTreeNodes = currentPromptKeyTreeNode.GetTriggeredNodesUsingEntries(currentTriggeredKeyTreeNodes, activeCharacter);
                usedPromptCharacters = KeyTree.AddCharacterUnique(usedPromptCharacters, activeCharacterDisplayItem.Content);
            }
            else if (activeCharacterDisplayItem is MapRegionsDisplayItem mrdi)
            {
                List<KeyTreeNodeBase> triggeredKeyTreeNodes = currentPromptKeyTreeNode.GetTriggeredNodesUsingRegions(currentTriggeredKeyTreeNodes, mrdi.RegionIds);
                currentTriggeredKeyTreeNodes = KeyTree.AddNodesRangeUnique(currentTriggeredKeyTreeNodes, triggeredKeyTreeNodes);
                hasRegionFilterBeenUsed = true;
            }
            else throw new ApplicationException("Unknown displayItem encountered");

            currentTriggeredKeyTreeNodes = currentTriggeredKeyTreeNodes.Distinct(new TreeNodeComparer()).ToList();

            if (currentTriggeredKeyTreeNodes.Count > 0)
                State = FilterState.HasMatches;
            else
                State = FilterState.NoMoreMatches;

            InvalidateMenuCommand.Execute(null);
        }

        private void ConductRegionSearch(KeyTreeNodeBase currentPromptKeyTreeNode, List<int> regionIds, ref List<KeyTreeNodeBase> currentTriggeredKeyTreeNodes)
        {
            if (regionIds.Count == 0)
            {
                currentTriggeredKeyTreeNodes = currentPromptKeyTreeNode.Children;
            }
            else
            {
                currentTriggeredKeyTreeNodes = currentPromptKeyTreeNode.GetTriggeredNodesUsingRegions(currentTriggeredKeyTreeNodes, regionIds);
            }
            currentTriggeredKeyTreeNodes = currentTriggeredKeyTreeNodes.Distinct(new TreeNodeComparer()).ToList();

            if (currentTriggeredKeyTreeNodes.Count > 0)
                State = FilterState.HasMatches;
            else
                State = FilterState.NoMoreMatches;

            InvalidateMenuCommand.Execute(null);
        }

        public ICommand OnViewResultsClicked => commandHelper.ProduceDebouncedCommand(async () =>
        {
            try
            {
                await ActivityIndicatorStart();


                #region *// Replace genus nodes with only single species to species node 
                var simplifiedTreeNodes = new List<KeyTreeNodeBase>();
                foreach (var triggeredKeyTreenode in CurrentTriggeredKeyTreeNodes)
                {
                    var simplifiedTreeNode = triggeredKeyTreenode;
                    if (triggeredKeyTreenode is KeyTreeNode ktn)
                    {
                        if (ktn.Children.Count == 1 && ktn.Children[0] is LeafKeyTreeNode)
                        {
                            simplifiedTreeNode = ktn.Children[0];
                        }
                    }
                    simplifiedTreeNodes.Add(simplifiedTreeNode);
                }
                #endregion


                var viewModel = new FamilyKeyResultPageViewModel(simplifiedTreeNodes) { IsHomeEnabled = true };
                var page = new FamilyKeyResultPage(viewModel);
                var resultType = await NavigateToPageAsync(page, viewModel);
                if (resultType == NavigateReturnType.GotoRoot) NavigateBack(NavigateReturnType.GotoRoot);
                if (resultType == NavigateReturnType.GotoFilterReset) {
                    ResetFilter(FilterState.StartFromScratch, KeyTreeFilter.Current.GetFilterResetNode());
                    return;
                };
                if (resultType == NavigateReturnType.IsCancelled)
                {
                    if (viewModel.IsFiterReset)
                    {
                        ResetFilter(FilterState.StartFromScratch, KeyTreeFilter.Current.GetFilterResetNode());
                        return;
                    }
                    return;
                };

                #region *// User requested to go one level down
                State = FilterState.StartNextLevel;
                var rootKeyTreeNode = viewModel.SelectedDisplayItem.Content as KeyTreeNodeBase;
                ResetFilter(FilterState.StartNextLevel, rootKeyTreeNode);
                Title = $"{FILTER_TITLE} {rootKeyTreeNode.NodeId}:";
                #endregion
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

        public Action<ImageSource> OnImageClicked => async (imageSource) =>
        {
            try
            {
                await ActivityIndicatorStart();

                var viewModel = new DisplayImagePageViewModel(imageSource);
                var page = new DisplayImagePage(viewModel);
                await NavigateToPageAsync(page, viewModel);

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Problem: ", ex.Message, "OK");
            }
            finally
            {
                ActivityIndicatorStop();
            }
        };

        private class CompareTreeNode : IEqualityComparer<KeyTreeNodeBase>
        {
            public bool Equals(KeyTreeNodeBase x, KeyTreeNodeBase y)
            {
                throw new NotImplementedException();
            }

            public int GetHashCode(KeyTreeNodeBase obj)
            {
                throw new NotImplementedException();
            }
        }

        public class FilterSnapShots: List<FilterSnapShot>
        {

            public class FilterSnapShot
            {
                public KeyTreeNodeBase BeforePromptKeyTreeNode;
                public bool BeforeHasRegionFilterBeenUsed;
                public List<KeyTreeNodeBase> BeforeTriggeredKeyTreeNodes;
                public List<CharacterPromptBase> BeforeUsedPromptCharacters;
                public FilterState BeforeFilterState;
                public FilterAction AppliedFilterAction;

                public enum FilterAction
                {
                    NoFilter, FilteredOnCharacter, FilteredOnRegion
                }
            }

            public void Push(FilterSnapShot snapshot)
            {
                Add(snapshot);
            }

            public FilterSnapShot Pop()
            {
                if (Count > 0)
                {
                    var snapShot = this.Last();
                    this.Remove(snapShot);
                    return snapShot;
                }
                return null;
            }
        }
    }
}


