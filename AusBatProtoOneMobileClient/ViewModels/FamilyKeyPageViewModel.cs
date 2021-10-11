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
        public List<KeyTreeNode> CurrentKeyTreeNodes = new List<KeyTreeNode>();
        public List<int> CurrentRegionIds { get; set; } = new List<int>();
        public abstract class CharacterDisplayItemBase
        {
            public virtual string Prompt { get; set; }
            public int DisplayOrder { get; set; }
            public Action<CharacterDisplayItemBase> OnChanged { get; set; }

            internal List<KeyTreeNode> ConductSearch(List<KeyTreeNode> currentKeyTreeNodes)
            {
                throw new NotImplementedException();
            }
        }
        public class PickerDisplayItem : CharacterDisplayItemBase
        {
            public string SelectedOptionId { get; set; }
            public List<string> Options { get; set; }
            public List<string> ImageSources { get; set; }
            public PickerCharacter Content { get; set; }
        }
        public class NumericDisplayItem : CharacterDisplayItemBase
        {
            public float Value { get; set; }

        }
        public class MapRegionsDisplayItem : CharacterDisplayItemBase
        {
            public override string Prompt { get; set; } = "Regions";
            public List<int> RegionIds { get; set; }
            public string SelectionAmount { get; set; }
            public ICommand OnSearch { get; set; }
            public bool HasEntry() => RegionIds?.Count > 0;
        }

        public ObservableCollection<CharacterDisplayItemBase> CharacterDisplayItems { get; set; }

        public string FilterResult { get; set; }

        public ICommand InvalidateMenu { get; set; }

        CommandHelper commandHelper = new CommandHelper();

        CancellationTokenSource cts;


        #region *// Menu related 

        public bool IsSelected { get; set; }

        #endregion


        public FamilyKeyPageViewModel(List<KeyTreeNode> currentKeyTreeNodes, List<int> currentRegionIds)
        {
            CurrentKeyTreeNodes = currentKeyTreeNodes;
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

                CharacterDisplayItems = UpdateCharacterDisplay(CurrentKeyTreeNodes);
                

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

        public ObservableCollection<CharacterDisplayItemBase> UpdateCharacterDisplay(List<KeyTreeNode> keyTreeNodes)
        {
            var displayItems = new ObservableCollection<CharacterDisplayItemBase>();
            foreach (var keyTreeNode in keyTreeNodes)
            {
                foreach (var character in keyTreeNode.Characters)
                {
                    if (character is NumericCharacter)
                    {
                        displayItems.Add(new NumericDisplayItem { 
                            Prompt = ((NumericCharacter)character).keyId,
                            Value = float.MinValue
                        });
                    }
                    else if (character is PickerCharacterValue)
                    {
                        var pickerCharacterValue = (PickerCharacterValue)character;
                        var options = App.dbase.KeyTree.GetPickerOptions(pickerCharacterValue.keyTableNodeId, pickerCharacterValue.keyId).Select(o => o.OptionPrompt);
                        displayItems.Add(new PickerDisplayItem
                        {
                            Prompt = pickerCharacterValue.keyId,
                            SelectedOptionId = pickerCharacterValue.optionId,
                            Options = options.ToList()
                        }); 
                    }
                }
            }
            displayItems.Add(new MapRegionsDisplayItem
            {
                RegionIds = CurrentRegionIds,
                OnChanged = (displayItem) => { OnSpecifyRegionClicked.Execute(null); }
            });

            displayItems.OrderBy(o=>o.DisplayOrder);
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
                CharacterDisplayItems = UpdateCharacterDisplay(KeyTreeFilter.Current.GetFilterResetNodes());
                CurrentRegionIds.Clear();

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

                CurrentKeyTreeNodes = ConductSearch(CurrentKeyTreeNodes);
                CharacterDisplayItems = UpdateCharacterDisplay(CurrentKeyTreeNodes);

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

        private List<KeyTreeNode> ConductSearch(List<KeyTreeNode> currentKeyTreeNodes)
        {

            foreach (var characterDisplayItem in CharacterDisplayItems)
            {
                currentKeyTreeNodes = characterDisplayItem.ConductSearch(currentKeyTreeNodes);
                if (currentKeyTreeNodes.Count == 0) return new List<KeyTreeNode>();                
            }
            if (CurrentRegionIds.Count > 0)
            {
                currentKeyTreeNodes = KeyTreeFilter.Current.GetKeyTreeNodesInRegions(currentKeyTreeNodes, CurrentRegionIds);
                if (currentKeyTreeNodes.IsEmpty()) return new List<KeyTreeNode>();
            }
            return currentKeyTreeNodes;
        }

        public ICommand OnViewResultsClicked => commandHelper.ProduceDebouncedCommand(async () =>
        {
            try
            {
                ActivityIndicatorStart();

                var viewModel = new FamilyKeyResultPageViewModel(CurrentKeyTreeNodes) { IsHomeEnabled = true };
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


