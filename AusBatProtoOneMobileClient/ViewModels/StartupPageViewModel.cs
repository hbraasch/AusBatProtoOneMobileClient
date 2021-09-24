using AusBatProtoOneMobileClient.Data;
using AusBatProtoOneMobileClient.Models;
using Mobile.Helpers;
using Mobile.Models;
using Mobile.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            NavigateBack(true);
        });

        public bool isBackCancelled = false;
        public ICommand OnBackButtonPressed => new Command(() =>
        {
            NavigateBack(true);
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
        

        public ICommand OnClassificationClicked => commandHelper.ProduceDebouncedCommand(async () => {
             try
             {
                var viewModel = new ClassificationPageViewModel();
                var page = new ClassificationPage<ClassificationPageViewModel.DisplayItem>(viewModel);
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

        public ICommand OnIdentificationKeysClicked => commandHelper.ProduceDebouncedCommand(async () => {
            try
            {
                var viewModel = new ClassificationPageViewModel();
                var page = new ClassificationPage<ClassificationPageViewModel.DisplayItem>(viewModel);
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

        public ICommand OnGenerateMockDataPressed => commandHelper.ProduceDebouncedCommand(async () => {
            try
            {
                var dbase = new Dbase();

                var hotspotRadius = 0.05f;
                dbase.MapRegions.Add(new Models.MapRegion { Id = 102, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.79, 0.21), Radius = hotspotRadius} } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 103, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.81, 0.32), Radius = hotspotRadius} } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 104, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.87, 0.42), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 105, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.93, 0.59), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 106, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.87, 0.70), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 107, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.81, 0.80), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 108, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.78, 0.94), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 201, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.73, 0.23), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 202, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.70, 0.50), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 203, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.73, 0.72), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 301, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.46, 0.13), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 302, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.31, 0.23), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 304, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.50, 0.29), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 306, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.28, 0.40), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 307, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.12, 0.41), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 308, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.07, 0.50), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 309, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.19, 0.59), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 310, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.37, 0.61), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 311, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.56, 0.67), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 312, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.63, 0.66), Radius = hotspotRadius } } });

                dbase.Classifications.Add(new Classification { Id = "Molossid", Type = Classification.ClassificationType.Family });
                    dbase.Classifications.Add(new Classification { Id = "Mormopterus", Type = Classification.ClassificationType.Genus, Parent = "Molossid" });
                        dbase.Classifications.Add(new Classification { Id = "Cobourgianus", Type = Classification.ClassificationType.Species, Parent = "Mormopterus" });
                        dbase.Classifications.Add(new Classification { Id = "Ridei", Type = Classification.ClassificationType.Species, Parent = "Mormopterus" });
                    dbase.Classifications.Add(new Classification { Id = "Chaerephon", Type = Classification.ClassificationType.Genus, Parent = "Molossid" });
                        dbase.Classifications.Add(new Classification { Id = "Jobensis", Type = Classification.ClassificationType.Species, Parent = "Chaerephon" });
                dbase.Classifications.Add(new Classification { Id = "Vespertilionidae", Type = Classification.ClassificationType.Family });
                    dbase.Classifications.Add(new Classification { Id = "Chalinolobus", Type = Classification.ClassificationType.Genus, Parent = "Vespertilionidae" });
                        dbase.Classifications.Add(new Classification { Id = "Dwyeri", Type = Classification.ClassificationType.Species, Parent = "Chalinolobus" });
                        dbase.Classifications.Add(new Classification { Id = "Gouldii", Type = Classification.ClassificationType.Species, Parent = "Chalinolobus" });
                    dbase.Classifications.Add(new Classification { Id = "Falsistrellus", Type = Classification.ClassificationType.Genus, Parent = "Vespertilionidae" });
                        dbase.Classifications.Add(new Classification { Id = "Mackenziei", Type = Classification.ClassificationType.Species, Parent = "Falsistrellus" });
                        dbase.Classifications.Add(new Classification { Id = "Tasmaniensis", Type = Classification.ClassificationType.Species, Parent = "Falsistrellus" });


                dbase.Bats.Add(new Bat {ClassificationId = "Cobourgianus", Name = "Western little free-tailed bat", ImageTag = "Aust_aust" });
                dbase.Bats.Add(new Bat { ClassificationId = "Ridei", Name = "Eastern little free-tailed bat", ImageTag = "Chae_job" });
                dbase.Bats.Add(new Bat { ClassificationId = "Jobensis", Name = "Great Northern free-tailed bat", ImageTag = "Chal_dwye" });
                dbase.Bats.Add(new Bat { ClassificationId = "Dwyeri", Name = "Large-eared pied bat", ImageTag = "Chal_gould" });
                dbase.Bats.Add(new Bat { ClassificationId = "Gouldii", Name = "Gould's wattled bat", ImageTag = "Chal_morio" });
                dbase.Bats.Add(new Bat { ClassificationId = "Mackenziei", Name = "Western false pipistrelle", ImageTag = "Dory_semon" });
                dbase.Bats.Add(new Bat { ClassificationId = "Tasmaniensis", Name = "Eastern false pipistrelle", ImageTag = "Chal_nigro" });

                foreach (var bat in dbase.Bats)
                {
                    bat.GenerateMockImageIds();
                    bat.GenerateMockDetails();
                    bat.GenerateMockRegions(dbase.MapRegions);
                    bat.GenerateMockCalls();
                }


                var folderPath = Path.Combine(FileSystem.AppDataDirectory, "Library");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                Debug.WriteLine($"Writing files to path: {folderPath}");

                var classifJson = JsonConvert.SerializeObject(dbase.Classifications, Formatting.Indented);
                File.WriteAllText(Path.Combine(folderPath,"classification.json"), classifJson);
                var batsJson = JsonConvert.SerializeObject(dbase.Bats, Formatting.Indented);
                File.WriteAllText(Path.Combine(folderPath, "bats.json"), batsJson);

                Dbase.Save(dbase);
                App.dbase = dbase;
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
