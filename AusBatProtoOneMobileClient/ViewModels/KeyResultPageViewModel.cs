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
using static DocGenOneMobileClient.Views.FilterSnapShots;
using static DocGenOneMobileClient.Views.KeyPageViewModel;

namespace DocGenOneMobileClient.Views
{
    public class KeyResultPageViewModel : ViewModelBase
    {
        List<KeyTreeNodeBase> selectedKeyTreeNodes = null;
        List<int> currentRegionIds;
        public class DisplayItemBase {
            public KeyTreeNodeBase Content { get; set; }
        }
        public class NodeDisplayItem : DisplayItemBase
        {
            public string Name { get; set; }
        }

        public class LeafNodeDisplayItem : DisplayItemBase
        {
            public string SpeciesName { get; set; }
            public string CommonName { get; set; }
            public string ImageSource { get; set; }

        }

        public class NoticeDisplayItem : DisplayItemBase
        {
            public string Description { get; set; }
        }

        public ObservableCollection<DisplayItemBase> DisplayItems { get; set; }

        public DisplayItemBase SelectedDisplayItem { get; set; }

        public bool IsDirty { get; set; }

        public ICommand InvalidateMenu { get; set; }

        CommandHelper commandHelper = new CommandHelper();

        CancellationTokenSource cts;

        public string Title { get; set; }


        #region *// Menu related 

        public bool IsSelected { get; set; }

        #endregion


        public KeyResultPageViewModel(List<KeyTreeNodeBase> selectedKeyTreeNodes, List<int> currentRegionIds = null)
        {
            this.selectedKeyTreeNodes = selectedKeyTreeNodes;
            this.currentRegionIds = (currentRegionIds == null) ? new List<int>(): currentRegionIds;
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

                DisplayItems =  UpdateDisplay();


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

        public ObservableCollection<DisplayItemBase> UpdateDisplay()
        {

            var displayItems = new ObservableCollection<DisplayItemBase>();
            if (selectedKeyTreeNodes.Count == 0) { displayItems.Add(new NoticeDisplayItem { }); return displayItems; }
            foreach (var selectedKeyTreeNode in selectedKeyTreeNodes)
            {
                if (selectedKeyTreeNode is LeafKeyTreeNode lktn)
                {
                    var species = App.dbase.FindSpecies(lktn.GenusId, lktn.SpeciesId);
                    var imageSource = (species.Images.Count == 0) ? "bat.png": species.Images[0];
                    displayItems.Add(new LeafNodeDisplayItem { 
                        SpeciesName = $"{species.GenusId.ToUpperFirstChar()} {species.SpeciesId}",
                        CommonName = species.Name,
                        ImageSource = imageSource,
                        Content = selectedKeyTreeNode
                    });
                }
                else 
                {
                    displayItems.Add(new NodeDisplayItem
                    {
                        Name = selectedKeyTreeNode.NodeId,
                        Content = selectedKeyTreeNode
                    });
                }
            }

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

        public bool IsHomeEnabled { get; set; }
        public ICommand OnHomeMenuPressed => new Command(() =>
        {
            NavigateBack(NavigateReturnType.GotoHome);
        });

        public bool isBackCancelled = false;
        public ICommand OnBackButtonPressed => new Command(() =>
        {
            isBackCancelled = true;
        });


        public ICommand OnSelectPressed => commandHelper.ProduceDebouncedCommand(async () =>
        {

            try
            {
                if (SelectedDisplayItem == null) return;

                if (SelectedDisplayItem is NodeDisplayItem )
                {
                    // Navigate to next level key filter
                    var viewModel = new KeyPageViewModel(SelectedDisplayItem.Content, currentRegionIds);
                    viewModel.Title = SelectedDisplayItem.Content.NodeId;
                    viewModel.State = FilterState.StartNextLevel;
                    var page = new KeyPage(viewModel);
                    var resultType = await NavigateToPageAsync(page, viewModel);
                    if (resultType == NavigateReturnType.GotoHome) NavigateBack(NavigateReturnType.GotoHome);
                    if (resultType == NavigateReturnType.GotoFilterReset) NavigateBack(NavigateReturnType.GotoFilterReset);
                }
                else if (SelectedDisplayItem is LeafNodeDisplayItem lndi)
                {
                    // Navigate direct to species
                    var leafKeyNode = SelectedDisplayItem.Content as LeafKeyTreeNode;
                    var species = App.dbase.FindSpecies(leafKeyNode.GenusId, leafKeyNode.SpeciesId);
                    var viewModel = new DisplaySpeciesTabbedPageViewModel(species) { IsHomeEnabled = true, IsResetFilterEnabled = true };
                    var page = new DisplaySpeciesTabbedPage(viewModel);
                    var resultType = await NavigateToPageAsync(page, viewModel);
                    if (resultType == NavigateReturnType.GotoHome) NavigateBack(NavigateReturnType.GotoHome);
                    if (resultType == NavigateReturnType.GotoFilterReset) NavigateBack(NavigateReturnType.GotoFilterReset);
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

        public bool IsFilterReset = false;
        public ICommand OnResetFiltersClicked => commandHelper.ProduceDebouncedCommand(async () =>
        {
            try
            {
                IsFilterReset = true;
                NavigateBack(NavigateReturnType.IsCancelled);
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


