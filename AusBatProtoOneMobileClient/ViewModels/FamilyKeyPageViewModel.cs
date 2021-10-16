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
using static AusBatProtoOneMobileClient.Models.KeyTree;
using AusBatProtoOneMobileClient.Models.Touch;

namespace DocGenOneMobileClient.Views
{
    public class FamilyKeyPageViewModel : ViewModelBase
    {
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
            public ICommand OnSearch { get; set; }
            public override bool HasEntry() => RegionIds?.Count > 0;

        }

        public ObservableCollection<CharacterDisplayItemBase> CharacterDisplayItems { get; set; }

        public string FilterResult { get; set; }
        public string Title { get; set; }

        public ICommand InvalidateMenu { get; set; }

        CommandHelper commandHelper = new CommandHelper();

        CancellationTokenSource cts;


        #region *// Menu related 

        public bool IsSelected { get; set; }

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
            Title = "Filter";
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
                FilterResult = $"(0) results";

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
            return $"({CurrentTriggeredKeyTreeNodes.Count}) results";
        }

        public ObservableCollection<CharacterDisplayItemBase> UpdateCharacterDisplay(KeyTreeNodeBase currentPromptKeyTreeNode)
        {
            var displayItems = new ObservableCollection<CharacterDisplayItemBase>();

            #region *// Determine which keys needs to be displayed
            List<CharacterPromptBase> AllPromptCharacters = currentPromptKeyTreeNode.PromptCharactersForNextLevel;
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


            #region *// Create display items
            foreach (var character in BestPromptCharacters.OrderBy(o=>o.DisplayOrder))
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
                    OnChanged = OnSpecifyRegionClicked
                });
            } 
            #endregion

            displayItems.OrderBy(o => o.DisplayOrder);
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

        public ICommand OnSpecifyRegionClicked => commandHelper.ProduceDebouncedCommand(async () =>
        {
            try
            {
                var viewModel = new SelectBatRegionsPageViewModel() { SelectedMapRegions = new ObservableCollection<MapRegion>(App.dbase.MapRegions.Where(o=>CurrentRegionIds.Contains(o.Id))) };
                var page = new SelectBatRegionsPage(viewModel);
                var returnType = await NavigateToPageAsync(page, viewModel);
                if (returnType == NavigateReturnType.IsCancelled) return;
                if (returnType == NavigateReturnType.GotoRoot) { NavigateBack(NavigateReturnType.GotoRoot); return; }
                CurrentRegionIds = viewModel.SelectedMapRegions.Select(o=>o.Id).ToList();
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
                ResetFilter(KeyTreeFilter.Current.GetFilterResetNode());

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

        private void ResetFilter(KeyTreeNodeBase rootNode )
        {
            CurrentPromptKeyTreeNode = rootNode;
            UsedPromptCharacters.Clear();
            CurrentTriggeredKeyTreeNodes.Clear();

            CharacterDisplayItems = UpdateCharacterDisplay(CurrentPromptKeyTreeNode);
            CurrentRegionIds.Clear();
            HasRegionFilterBeenUsed = false;
            FilterResult = $"(0) results";
            Title = "Filter";
        }
        public ICommand OnFilterClicked => commandHelper.ProduceDebouncedCommand(async () =>
        {
            try
            {
                ActivityIndicatorStart();

                var activeCharacterDisplayItem = CharacterDisplayItems.FirstOrDefault(o => o.HasEntry());
                ConductSearch(CurrentPromptKeyTreeNode, activeCharacterDisplayItem, ref CurrentTriggeredKeyTreeNodes, ref UsedPromptCharacters, ref HasRegionFilterBeenUsed);
                CharacterDisplayItems = UpdateCharacterDisplay(CurrentPromptKeyTreeNode);
                FilterResult = UpdateFilterResults();

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

            List<KeyTreeNodeBase> allTriggeredKeyTreeNodes = new List<KeyTreeNodeBase>();

            if (activeCharacterDisplayItem == null) return;

            if (activeCharacterDisplayItem is PickerDisplayItem pdi)
            {
                var selectedOptionId = pdi.OptionIds[pdi.Options.IndexOf(pdi.SelectedOption)];
                var activeCharacter = activeCharacterDisplayItem.Content as PickerCharacterPrompt;
                activeCharacter.EntryOptionId = selectedOptionId;
                List<KeyTreeNodeBase> triggeredKeyTreeNodes = currentPromptKeyTreeNode.GetTriggeredNodesUsingEntries(currentTriggeredKeyTreeNodes, activeCharacter);
                usedPromptCharacters = KeyTree.AddCharacterUnique(usedPromptCharacters, activeCharacterDisplayItem.Content);
                allTriggeredKeyTreeNodes = KeyTree.AddNodesRangeUnique(currentTriggeredKeyTreeNodes, triggeredKeyTreeNodes);
            }
            else if (activeCharacterDisplayItem is NumericDisplayItem ndi)
            {
                var activeCharacter = activeCharacterDisplayItem.Content as NumericCharacterPrompt;
                activeCharacter.Entry = float.Parse(ndi.Value);
                List<KeyTreeNodeBase> triggeredKeyTreeNodes = currentPromptKeyTreeNode.GetTriggeredNodesUsingEntries(currentTriggeredKeyTreeNodes, activeCharacter);
                usedPromptCharacters = KeyTree.AddCharacterUnique(usedPromptCharacters, activeCharacterDisplayItem.Content);
                allTriggeredKeyTreeNodes = KeyTree.AddNodesRangeUnique(currentTriggeredKeyTreeNodes, triggeredKeyTreeNodes);
            }
            else if (activeCharacterDisplayItem is MapRegionsDisplayItem mrdi)
            {
                List<KeyTreeNodeBase> triggeredKeyTreeNodes = currentPromptKeyTreeNode.GetTriggeredNodesUsingRegions(currentTriggeredKeyTreeNodes, mrdi.RegionIds);
                allTriggeredKeyTreeNodes = KeyTree.AddNodesRangeUnique(currentTriggeredKeyTreeNodes, triggeredKeyTreeNodes);
                hasRegionFilterBeenUsed = true;
            }
            else throw new ApplicationException("Unknown displayItem encountered");

            currentTriggeredKeyTreeNodes =  allTriggeredKeyTreeNodes;
        }



        public ICommand OnViewResultsClicked => commandHelper.ProduceDebouncedCommand(async () =>
        {
            try
            {
                ActivityIndicatorStart();


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
                if (resultType == NavigateReturnType.IsCancelled)
                {
                    if (viewModel.IsFiterReset)
                    {
                        ResetFilter(KeyTreeFilter.Current.GetFilterResetNode());
                        return;
                    }
                    return;
                };

                #region *// User requested to go one level down
                var rootKeyTreeNode = viewModel.SelectedDisplayItem.Content as KeyTreeNodeBase;
                ResetFilter(rootKeyTreeNode);
                Title = $"Filter: {rootKeyTreeNode.NodeId}";
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
                ActivityIndicatorStart();

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


    }


}


