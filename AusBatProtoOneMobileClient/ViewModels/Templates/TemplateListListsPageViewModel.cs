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
    public class TemplateListListsPageViewModel : ViewModelBase
    {

        public ObservableCollection<DisplayItem> DisplayItems { get; set; }

        public DisplayItem SelectedItem { get; set; }

        public void OnSelectedItemChanged()
        {
            IsSelected = SelectedItem != null;
            InvalidateMenu.Execute(null);
        }

        private bool isItemChecked;
        public Command OnCheckChanged => new Command(() => { 
            isItemChecked = (DisplayItems.FirstOrDefault(o => o.IsChecked == true) != null);
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
          

        public TemplateListListsPageViewModel()
        {
            DisplayItems = new ObservableCollection<DisplayItem>();
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

                DisplayItems = UpdateDisplay();


            }
            catch (Exception ex) when (ex is TaskCanceledException ext)
            {
                Debug.Write("Cancelled by user");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error",ex.CompleteMessage(), "Ok");
            }
            finally
            {
                ActivityIndicatorStop();
            }
        });

        public ObservableCollection<ListViewDataTemplate.DisplayItem> UpdateDisplay()
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


        public ICommand OnAddMenuPressed => commandHelper.ProduceDebouncedCommand(async () =>
        {
            try
            {

                ActivityIndicatorStart();


                DisplayItems = UpdateDisplay();

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
                if (SelectedItem == null) return;
                ActivityIndicatorStart();


                UpdateDisplay();

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

                UpdateDisplay();

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
    

