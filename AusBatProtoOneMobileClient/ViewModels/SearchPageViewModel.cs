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

namespace DocGenOneMobileClient.Views
{
    public class SearchPageViewModel : ViewModelBase
    {

        public ObservableCollection<DisplayItem> CriteriaDisplayItems { get; set; }

        public DisplayItem CriteriaSelectedItem { get; set; }

        public ObservableCollection<DisplayItem> ResultsDisplayItems { get; set; }

        public DisplayItem ResultsSelectedItem { get; set; }

        public void OnSelectedItemChanged()
        {
            IsSelected = CriteriaSelectedItem != null;
            InvalidateMenu.Execute(null);
        }

        private bool isItemChecked;
        public Command OnCheckChanged => new Command(() => {
            isItemChecked = (CriteriaDisplayItems.FirstOrDefault(o => o.IsChecked == true) != null);
            InvalidateMenu.Execute(null);
        });


        public bool IsDirty { get; set; }

        public ICommand InvalidateMenu { get; set; }

        CommandHelper commandHelper = new CommandHelper();

        CancellationTokenSource cts;


        #region *// Menu related 

        public bool IsSelected { get; set; }
        public bool IsChecked => isItemChecked;

        #endregion


        public SearchPageViewModel()
        {
            CriteriaDisplayItems = new ObservableCollection<DisplayItem>();
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

                CriteriaDisplayItems = UpdateCriteriaDisplay();
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

        public ObservableCollection<ListViewDataTemplate.DisplayItem> UpdateCriteriaDisplay()
        {
            var displayItems = new ObservableCollection<ListViewDataTemplate.DisplayItem>();
            return displayItems;
        }

        public ObservableCollection<ListViewDataTemplate.DisplayItem> UpdateResultsDisplay()
        {
            var displayItems = new ObservableCollection<ListViewDataTemplate.DisplayItem>();
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


        public ICommand OnSearchButtonPressed => commandHelper.ProduceDebouncedCommand(async () =>
        {
            try
            {

                ActivityIndicatorStart();


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



        public ICommand OnEditMenuPressed => commandHelper.ProduceDebouncedCommand(async () =>
        {
            try
            {
                if (CriteriaSelectedItem == null) return;
                ActivityIndicatorStart();


                UpdateCriteriaDisplay();

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

                UpdateCriteriaDisplay();

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

        public ICommand OnRenameMenuPressed => commandHelper.ProduceDebouncedCommand(async () =>
        {

            try
            {
                ActivityIndicatorStart();


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
}


