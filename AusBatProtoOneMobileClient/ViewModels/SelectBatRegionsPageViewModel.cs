using AusBatProtoOneMobileClient.Models;
using Mobile.Helpers;
using Mobile.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TreeApp.Helpers;
using Xamarin.Forms;

namespace AusBatProtoOneMobileClient.ViewModels
{
    public class SelectBatRegionsPageViewModel : ViewModelBase
    {
        CommandHelper commandHelper = new CommandHelper();

        public ObservableCollection<MapRegion> SelectedMapRegions { get; set; }

        #region *// Menu related
        public ICommand InvalidateMenuCommand { get; set; }
        public bool IsLoggedIn { get; set; } = false;
        #endregion
        public SelectBatRegionsPageViewModel()
        {
        }

        public ICommand OnFirstAppearance => commandHelper.ProduceDebouncedCommand(() => {

        });

        public ICommand OnSubsequentAppearance => commandHelper.ProduceDebouncedCommand(() => { });

        public ICommand OnBackMenuPressed => new Command<bool>((isSelect) =>
        {
            NavigateBack(isSelect);
        });

        public bool isBackCancelled = false;
        public ICommand OnBackButtonPressed => new Command(() =>
        {
            isBackCancelled = true;
        });

        public ICommand OnSelectBatRegionsMenuPressed => commandHelper.ProduceDebouncedCommand(async () => {
            try
            {
                var cts = new CancellationTokenSource();
                ActivityIndicatorStart("Starting ...", () =>
                {
                    cts.Cancel();
                    ActivityIndicatorStop();
                });

                // Do work here
            }
            catch (Exception ex) when (ex is TaskCanceledException ext)
            {
                Debug.Write("Cancelled by user");
            }
            catch (Exception ex) when (ex is BusinessException exb)
            {
                await DisplayAlert("Notification", exb.CompleteMessage(), "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Problem: ", ex.CompleteMessage(), "OK");
            }
            finally
            {
                ActivityIndicatorStop();
            }
        });


    }
}
