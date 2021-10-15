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

namespace DocGenOneMobileClient.Views
{
    public class FamilyKeyResultPageViewModel : ViewModelBase
    {
        List<KeyTreeNodeBase> selectedKeyTreeNodes = null;
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

        public void OnSelectedItemChanged()
        {
            IsSelected = SelectedDisplayItem != null;
            InvalidateMenu.Execute(null);
        }


        public bool IsDirty { get; set; }

        public ICommand InvalidateMenu { get; set; }

        CommandHelper commandHelper = new CommandHelper();

        CancellationTokenSource cts;


        #region *// Menu related 

        public bool IsSelected { get; set; }

        #endregion


        public FamilyKeyResultPageViewModel(List<KeyTreeNodeBase> selectedKeyTreeNodes)
        {
            this.selectedKeyTreeNodes = selectedKeyTreeNodes;
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
                    displayItems.Add(new LeafNodeDisplayItem { 
                        SpeciesName = $"{species.GenusId.ToUpperFirstChar()} {species.SpeciesId}",
                        CommonName = species.Name,
                        ImageSource = species.Images[0],
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


        public ICommand OnSelectPressed => commandHelper.ProduceDebouncedCommand(async () =>
        {

            try
            {
                if (SelectedDisplayItem == null) return;

                if (SelectedDisplayItem is NodeDisplayItem )
                {
                    NavigateBack(NavigateReturnType.IsAccepted);
                }
                else if (SelectedDisplayItem is LeafNodeDisplayItem lndi)
                {
                    var leafKeyNode = SelectedDisplayItem.Content as LeafKeyTreeNode;
                    var species = App.dbase.FindSpecies(leafKeyNode.GenusId, leafKeyNode.SpeciesId);
                    var viewModel = new DisplayBatTabbedPageViewModel(species) { IsHomeEnabled = true };
                    var page = new DisplayBatTabbedPage(viewModel);
                    var resultType = await NavigateToPageAsync(page, viewModel);
                    if (resultType == NavigateReturnType.GotoRoot) NavigateBack(NavigateReturnType.GotoRoot);
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

        public bool IsFiterReset = false;
        public ICommand OnResetFiltersClicked => commandHelper.ProduceDebouncedCommand(async () =>
        {
            try
            {
                IsFiterReset = true;
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


