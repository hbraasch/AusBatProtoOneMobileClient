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
using static DocGenOneMobileClient.Views.FilterSnapShots;

namespace DocGenOneMobileClient.Views
{
    public partial class KeyPageViewModel : ViewModelBase
    {
        const string FILTER_TITLE = "Filter";
        const string NO_RESULTS_YET = "(0) taxa";

        public enum FilterState
        {
            StartFromScratch, StartNextLevel, HasMatches, NoMoreMatches, 
        }
        public FilterState State = FilterState.StartFromScratch;

        public KeyTreeNodeBase RootKeyTreeNode;
        public KeyTreeNodeBase CurrentPromptKeyTreeNode;
        public List<KeyTreeNodeBase> CurrentTriggeredKeyTreeNodes = new List<KeyTreeNodeBase>();

        public List<CharacterPromptBase> UsedPromptCharacters = new List<CharacterPromptBase>();
        public List<CharacterPromptBase> BestPromptCharacters = new List<CharacterPromptBase>();
        public List<CharacterPromptBase> RemovedPromptCharacters = new List<CharacterPromptBase>();
        public bool HasRegionFilterBeenUsed = false;

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

        private FilterSnapShots SnapShots = new FilterSnapShots();


        #region *// Menu related 

        public bool IsResetFilterEnabled => UsedPromptCharacters.Count > 0 || CurrentPromptKeyTreeNode.NodeId != RootKeyTreeNode.NodeId;
        public bool CanUndo => SnapShots.Count  > 0 || State == FilterState.StartNextLevel;
        #endregion

        public KeyPageViewModel()
        {

        }

        public KeyPageViewModel(KeyTreeNodeBase currentPromptKeyTreeNode, List<int> currentRegionIds)
        {
            RootKeyTreeNode = currentPromptKeyTreeNode;
            CurrentPromptKeyTreeNode = currentPromptKeyTreeNode;
            CurrentTriggeredKeyTreeNodes = new List<KeyTreeNodeBase>();
            CurrentRegionIds = currentRegionIds;
            CharacterDisplayItems = new ObservableCollection<CharacterDisplayItemBase>();
            Title = $"{FILTER_TITLE}";
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


            if (!HasRegionFilterBeenUsed)
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

            FilterHint = (HasRegionFilterBeenUsed) ? "": CurrentPromptKeyTreeNode.FilterHint;

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
                ResetFilter(FilterState.StartFromScratch, RootKeyTreeNode);

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
                if (snapshot == null)
                {
                    // Need to return one level up
                    NavigateBack(NavigateReturnType.UndoFilter);
                    return;
                }
                UndoFilter(snapshot);

                CharacterDisplayItems = UpdateCharacterDisplay(CurrentPromptKeyTreeNode);

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

        private void UndoFilter(KeyFilterSnapShot snapshot)
        {
            State = snapshot.BeforeFilterState;
            CurrentPromptKeyTreeNode = snapshot.BeforePromptKeyTreeNode;
            UsedPromptCharacters = snapshot.BeforeUsedPromptCharacters;
            CurrentTriggeredKeyTreeNodes = snapshot.BeforeTriggeredKeyTreeNodes;
            HasRegionFilterBeenUsed = snapshot.BeforeHasRegionFilterBeenUsed;
            CurrentRegionIds = snapshot.BeforeRegionIds;
        }


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

            Title = $"{FILTER_TITLE}";

            InvalidateMenuCommand.Execute(null);
        }
        public ICommand OnFilterClicked => commandHelper.ProduceDebouncedCommand(async () =>
        {
            try
            {
                await ActivityIndicatorStart();

                var activeCharacterDisplayItem = CharacterDisplayItems.FirstOrDefault(o => o.HasEntry());
                var snapShot = new KeyFilterSnapShot {
                    BeforePromptKeyTreeNode = CurrentPromptKeyTreeNode,
                    BeforeTriggeredKeyTreeNodes = CurrentTriggeredKeyTreeNodes.ToList(),
                    BeforeUsedPromptCharacters = UsedPromptCharacters.ToList(),
                    BeforeHasRegionFilterBeenUsed = HasRegionFilterBeenUsed,
                    BeforeFilterState = State,
                    BeforeRegionIds = CurrentRegionIds
                };
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
                currentTriggeredKeyTreeNodes = currentPromptKeyTreeNode.GetTriggeredNodesUsingRegions(currentTriggeredKeyTreeNodes, mrdi.RegionIds);
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


                var snapShot = new KeyFilterSnapShot
                {
                    BeforePromptKeyTreeNode = CurrentPromptKeyTreeNode,
                    BeforeTriggeredKeyTreeNodes = CurrentTriggeredKeyTreeNodes.ToList(),
                    BeforeUsedPromptCharacters = UsedPromptCharacters.ToList(),
                    BeforeHasRegionFilterBeenUsed = HasRegionFilterBeenUsed,
                    BeforeFilterState = State,
                    BeforeRegionIds = CurrentRegionIds
                };

                var viewModel = new KeyResultPageViewModel(simplifiedTreeNodes) { IsHomeEnabled = true };
                var page = new KeyResultPage(viewModel);
                var resultType = await NavigateToPageAsync(page, viewModel);
                if (resultType == NavigateReturnType.GotoRoot)
                {
                    NavigateBack(NavigateReturnType.GotoRoot);
                }
                else if (resultType == NavigateReturnType.GotoFilterReset) {
                    ResetFilter(FilterState.StartFromScratch, RootKeyTreeNode);
                    return;
                }
                else if (resultType == NavigateReturnType.IsCancelled)
                {
                    if (viewModel.IsFilterReset)
                    {
                        // Reset called
                        ResetFilter(FilterState.StartFromScratch, RootKeyTreeNode);
                        return;
                    }
                    else
                    {
                        // Just going back
#if false
                        UndoFilter(snapShot);
                        CharacterDisplayItems = UpdateCharacterDisplay(CurrentPromptKeyTreeNode);
                        InvalidateMenuCommand.Execute(null); 
#endif
                        return;
                    }
                }
                else
                {
                    throw new ApplicationException($"Unexpected enum [{resultType}] returned");
                }

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
    }
}


