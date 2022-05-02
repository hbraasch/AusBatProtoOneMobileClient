using AusBatProtoOneMobileClient.Data;
using AusBatProtoOneMobileClient.Models;
using AusBatProtoOneMobileClient.Models.Touch;
using DocGenOneMobileClient.Views;
using Mobile.Helpers;
using Mobile.Models;
using Mobile.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TreeApp.Helpers;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Xamarin.Essentials.Permissions;

namespace AusBatProtoOneMobileClient.ViewModels
{
    public class StartupPageViewModel : ViewModelBase
    {
        CommandHelper commandHelper = new CommandHelper();

        #region *// Menu related
        public ICommand InvalidateMenuCommand { get; set; }
        public bool IsLoggedIn { get; set; } = false;
        #endregion
        public StartupPageViewModel()
        {
        }

        public ICommand OnFirstAppearance => commandHelper.ProduceDebouncedCommand(async () => {

            try
            {
                var cts = new CancellationTokenSource();
                ActivityIndicatorStart("Starting ...", () =>
                {
                    cts.Cancel();
                    ActivityIndicatorStop();
                });

                // Do work here
                var codeDataVersion = new DbaseVersion(VersionTracking.CurrentVersion);
                var currentDataVersion = Settings.CurrentDataVersion;
                if (currentDataVersion < codeDataVersion)
                {
                    if (DeviceInfo.Platform == DevicePlatform.Android)
                    {
                        #region *// Request access to storage
                        var status = await CheckAndRequestPermissionAsync(new Permissions.StorageRead());
                        if (status != PermissionStatus.Granted)
                        {
                            throw new ApplicationException("Permission denied to access external storage");
                        }
                        status = await CheckAndRequestPermissionAsync(new Permissions.StorageWrite());
                        if (status != PermissionStatus.Granted)
                        {
                            throw new ApplicationException("Permission denied to access external storage");
                        }
                        #endregion
                    }
                    SetActivityIndicatorPrompt("Initializing data...");
                    await App.dbase.Init();
                    Settings.CurrentDataVersion = codeDataVersion;
                    Debug.WriteLine("Data has been initialized");
                }
                else
                {
                    Debug.WriteLine("Using current data. No need for data initialization. Loading...");
                    App.dbase = await Dbase.Load();
                    Debug.WriteLine("Current data loaded");
                }
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

            // Exit

            async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission)   where T : BasePermission
            {
                var status = await permission.CheckStatusAsync();
                if (status != PermissionStatus.Granted)
                {
                    status = await permission.RequestAsync();
                }

                return status;
            }
        });

        public ICommand OnSubsequentAppearance => commandHelper.ProduceDebouncedCommand(() => { });

        public ICommand OnBackMenuPressed => new Command(() =>
        {
            NavigateBack(NavigateReturnType.IsCancelled);
        });

        public bool isBackCancelled = false;
        public ICommand OnBackButtonPressed => new Command(() =>
        {
            NavigateBack(NavigateReturnType.IsCancelled);
            isBackCancelled = true;
        });

        public ICommand OnStartupMenuPressed => commandHelper.ProduceDebouncedCommand(async () => {
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
        

        public ICommand OnIntroButtonClicked => commandHelper.ProduceDebouncedCommand(async () => {
            try
            {
                var viewModel = new IntroductionPageViewModel();
                var page = new IntroductionPage(viewModel);
                await NavigateToPageAsync(page, viewModel);
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
        public ICommand OnAcknowledgementsMenuClicked => commandHelper.ProduceDebouncedCommand(async () => {
            try
            {
                var viewModel = new DisplayHtmlPageViewModel()
                {
                    Title = "Acknowledgements",
                    Html = App.dbase.AcknowledgementsHtml
                };
                var page = new DisplayHtmlPage(viewModel);
                await NavigateToPageAsync(page, viewModel);
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

        public ICommand OnCopyrightMenuClicked => commandHelper.ProduceDebouncedCommand(async () => {
            try
            {
                var viewModel = new DisplayHtmlPageViewModel()
                {
                    Title = "Copyrights",
                    Html = App.dbase.CopyrightHtml
                };
                var page = new DisplayHtmlPage(viewModel);
                await NavigateToPageAsync(page, viewModel);
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
        public ICommand OnAboutMenuClicked => commandHelper.ProduceDebouncedCommand(async () => {
            try
            {
                var viewModel = new DisplayHtmlPageViewModel() {
                    Title = "About",
                    Html = App.dbase.AboutHtml.Replace("<<<TEXT>>>", $"<p style=\"color: #E7E7E7\">{Constants.APP_NAME}</p><p style=\"color: #E7E7E7\">Version text: {Settings.CurrentDataVersion}</p><p style=\"color: #E7E7E7\">Version code: {ApkHelper.GetVersionCode()}</p>")
                };
                var page = new DisplayHtmlPage(viewModel);
                await NavigateToPageAsync(page, viewModel);
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
        public ICommand OnSpeciesByFamilyClicked => commandHelper.ProduceDebouncedCommand(async () => {
             try
             {
                var viewModel = new SpeciesByFamilyPageViewModel();
                var page = new SpeciesByFamilyPage(viewModel);
                await NavigateToPageAsync(page, viewModel);
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

        public ICommand OnSpeciesAtoZClicked => commandHelper.ProduceDebouncedCommand(async () => {
            try
            {
                var viewModel = new SpeciesAtoZPageViewModel();
                var page = new SpeciesAtoZPage(viewModel);
                await NavigateToPageAsync(page, viewModel);
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

        public ICommand OnFamilyKeyButtonClicked => commandHelper.ProduceDebouncedCommand(async () => {
            try
            {
                var viewModel = new KeyPageViewModel(KeyTreeFilter.Current.GetFilterResetNode(), new List<int>());
                var page = new KeyPage(viewModel);
                await NavigateToPageAsync(page, viewModel);
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

        public ICommand OnGeneraKeyButtonClicked => commandHelper.ProduceDebouncedCommand(async () => {
            try
            {
                var viewModel = new GeneraPageViewModel(KeyTreeFilter.Current.GetGeneraNodes());
                var page = new GeneraPage(viewModel);
                await NavigateToPageAsync(page, viewModel);
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
        
        public ICommand OnAreaListingsClicked => commandHelper.ProduceDebouncedCommand(async () => {
            try
            {
                var selectedRegions =  new ObservableCollection<MapRegion>();
                var viewModelRegions = new SelectBatRegionsPageViewModel() { SelectedMapRegions = selectedRegions };
                var pageRegions = new SelectBatRegionsPage(viewModelRegions);
                var returnType = await NavigateToPageAsync(pageRegions, viewModelRegions);
                if (returnType == NavigateReturnType.IsCancelled) return;

                selectedRegions = viewModelRegions.SelectedMapRegions;

                var currentResults = Dbase.Filter(App.dbase.Species, selectedRegions.ToList());
                var viewModel = new SpeciesByAreaPageViewModel(currentResults) { IsHomeEnabled = false };
                var page = new SpeciesByAreaPage(viewModel);
                await NavigateToPageAsync(page, viewModel);

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


        public ICommand OnInitPressed => commandHelper.ProduceDebouncedCommand(async () => {
            try
            {
                await ActivityIndicatorStart("Initializing...");
                await CommandHelper.DoEvents();
                await App.dbase.Init();
                await Application.Current.MainPage.DisplayAlert("Notice", "Init completed", "Ok");
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

        public ICommand OnSightingsPressed => commandHelper.ProduceDebouncedCommand(async () => {
            try
            {
                var sightingsPageViewModel = new SightingsPageViewModel();
                var sightingsPage = new SightingsPage(sightingsPageViewModel);
                var returnType = await NavigateToPageAsync(sightingsPage, sightingsPageViewModel);
                if (returnType == NavigateReturnType.GotoHome) NavigateBack(NavigateReturnType.GotoHome);
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
