using AusBatProtoOneMobileClient.Data;
using AusBatProtoOneMobileClient.Models;
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

        public ICommand OnFirstAppearance => commandHelper.ProduceDebouncedCommand(() => { });

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

        public ICommand OnAboutButtonClicked => commandHelper.ProduceDebouncedCommand(async () => {
            try
            {
                var viewModel = new AboutPageViewModel();
                var page = new AboutPage(viewModel);
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
                var viewModel = new FamilyKeyPageViewModel();
                var page = new FamilyKeyPage(viewModel);
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

                var currentResults = Dbase.Filter(App.dbase.Bats, selectedRegions.ToList());
                var viewModel = new DisplayFilteredSpeciesPageViewModel(currentResults) { IsHomeEnabled = false };
                var page = new DisplayFilteredSpeciesPage(viewModel);
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
                App.dbase.Init();
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
