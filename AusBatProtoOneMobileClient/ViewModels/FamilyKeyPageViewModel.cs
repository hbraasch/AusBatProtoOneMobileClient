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
        public KeyTreeNodeBase CurrentPromptKeyTreeNodes;
        public List<KeyTreeNodeBase> CurrentTriggeredKeyTreeNodes = new List<KeyTreeNodeBase>();

        public List<CharacterPromptBase> UsedPromptCharacters = new List<CharacterPromptBase>();
        public List<CharacterPromptBase> BestPromptCharacters = new List<CharacterPromptBase>();
        public List<CharacterPromptBase> RemovedPromptCharacters = new List<CharacterPromptBase>();


        public List<int> CurrentRegionIds { get; set; } = new List<int>();
        public abstract class CharacterDisplayItemBase
        {
            public virtual string Prompt { get; set; }
            public int DisplayOrder { get; set; }
            public ICommand OnChanged { get; set; }
            public CharacterPromptBase Content;

            public List<KeyTreeNodeBase> ConductSearch(List<KeyTreeNodeBase> currentKeyTreeNodes)
            {
                throw new NotImplementedException();
            }

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
            CurrentPromptKeyTreeNodes = KeyTree.Clone(currentPromptKeyTreeNode);
            CurrentTriggeredKeyTreeNodes = new List<KeyTreeNodeBase>();
            CurrentRegionIds = currentRegionIds;
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

                CharacterDisplayItems = UpdateCharacterDisplay(CurrentPromptKeyTreeNodes);
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

        public ObservableCollection<CharacterDisplayItemBase> UpdateCharacterDisplay(KeyTreeNodeBase promptKeyTreeNode)
        {
            var displayItems = new ObservableCollection<CharacterDisplayItemBase>();
            foreach (var keyTreeNode in promptKeyTreeNode)
            {
                foreach (var character in keyTreeNode.PromptCharactersForNextLevel)
                {
                    if (character is NumericCharacterPrompt ncp)
                    {
                        displayItems.Add(new NumericDisplayItem {
                            Prompt = ncp.Prompt,
                            Value = "",
                            OnChanged = OnFilterClicked,
                            Content = character
                        });
                    }
                    else if (character is PickerCharacterPrompt pcp)
                    {
                        displayItems.Add(new PickerDisplayItem
                        {
                            Prompt = pcp.Prompt,
                            SelectedOption = "",
                            Options = pcp.Options.Select(o => o.OptionPrompt).ToList(),
                            OptionIds = pcp.Options.Select(o => o.OptionId).ToList(),
                            OnChanged = OnFilterClicked,
                            Content = character
                        });
                    }
                }
            }
            displayItems.Add(new MapRegionsDisplayItem
            {
                RegionIds = CurrentRegionIds,
                OnChanged = OnSpecifyRegionClicked
            });

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
                CurrentPromptKeyTreeNodes = KeyTreeFilter.Current.GetFilterResetNode();
                CharacterDisplayItems = UpdateCharacterDisplay(CurrentPromptKeyTreeNodes);
                CurrentRegionIds.Clear();
                FilterResult = $"(0) results";

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
        public ICommand OnFilterClicked => commandHelper.ProduceDebouncedCommand(async () =>
        {
            try
            {
                ActivityIndicatorStart();

                ConductSearch();
                CharacterDisplayItems = UpdateCharacterDisplay(CurrentPromptKeyTreeNodes);
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

        private void ConductSearch()
        {
            var sourceKeyTreeNodes = CurrentPromptKeyTreeNodes;
            List<KeyTreeNodeBase> allTriggeredKeyTreeNodes = new List<KeyTreeNodeBase>();

            var activeCharacterDisplayItem = CharacterDisplayItems.FirstOrDefault(o => o.HasEntry());
            if (activeCharacterDisplayItem == null) return;

            if (activeCharacterDisplayItem is PickerDisplayItem pdi)
            {
                foreach (var sourceKeyTreeNode in sourceKeyTreeNodes)
                {
                    var selectedOptionId = pdi.OptionIds[pdi.Options.IndexOf(pdi.SelectedOption)];
                    (activeCharacterDisplayItem.Content as PickerCharacterPrompt).EntryOptionId = selectedOptionId;
                    List<KeyTreeNodeBase> triggeredKeyTreeNodes = sourceKeyTreeNode.GetEntryTriggeredNodes(activeCharacterDisplayItem.Content);
                    allTriggeredKeyTreeNodes = KeyTree.AddRangeUnique(allTriggeredKeyTreeNodes, triggeredKeyTreeNodes);
                }
            }
            else if (activeCharacterDisplayItem is NumericDisplayItem ndi)
            {
                foreach (var sourceKeyTreeNode in sourceKeyTreeNodes)
                {
                    (activeCharacterDisplayItem.Content as NumericCharacterPrompt).Entry = float.Parse(ndi.Value);
                    List<KeyTreeNodeBase> triggeredKeyTreeNodes = sourceKeyTreeNode.GetEntryTriggeredNodes(activeCharacterDisplayItem.Content);
                    allTriggeredKeyTreeNodes = KeyTree.AddRangeUnique(allTriggeredKeyTreeNodes, triggeredKeyTreeNodes);
                }
            }
            else if (activeCharacterDisplayItem is MapRegionsDisplayItem mrdi)
            {
                foreach (var sourceKeyTreeNode in sourceKeyTreeNodes)
                {
                    List<KeyTreeNodeBase> triggeredKeyTreeNodes = sourceKeyTreeNode.GetRegionTriggeredNodes(mrdi.RegionIds);
                    allTriggeredKeyTreeNodes = KeyTree.AddRangeUnique(allTriggeredKeyTreeNodes, triggeredKeyTreeNodes);
                }
            }
            else throw new ApplicationException("Unknown displayItem encountered");

            CurrentTriggeredKeyTreeNodes =  allTriggeredKeyTreeNodes;
        }



        public ICommand OnViewResultsClicked => commandHelper.ProduceDebouncedCommand(async () =>
        {
            try
            {
                ActivityIndicatorStart();

                var viewModel = new FamilyKeyResultPageViewModel(CurrentPromptKeyTreeNodes) { IsHomeEnabled = true };
                var page = new FamilyKeyResultPage(viewModel);
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


    }


}


