using AusBatProtoOneMobileClient.Data;
using AusBatProtoOneMobileClient.Helpers;
using AusBatProtoOneMobileClient.Models;
using DocGenOneMobileClient.Views;
using FFImageLoading.Forms;
using Mobile.Helpers;
using Mobile.ViewModels;
using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TreeApp.Helpers;
using Xamarin.Essentials;
using Xamarin.Forms;
using static AusBatProtoOneMobileClient.Helpers.VuHelper;
using NAudio;
using NAudio.Wave;

namespace AusBatProtoOneMobileClient.ViewModels
{
    public class DisplaySpeciesTabbedPageViewModel : ViewModelBase
    {
        public Species Species;

        CommandHelper commandHelper = new CommandHelper();

        public class ImageDataItem
        {
            public ImageSource ImageSource { get; set; }
        }

        public ObservableCollection<ImageDataItem> ImageDataItems { get; set; }
        public ObservableCollection<MapRegion> SelectedMapItems { get; set; }

        public ImageSource DistributionMapImage { get; set; }


        public HtmlWebViewSource DetailsHtmlSource { get; set; }
        public float HtmlFontSizePercentage { get; set; }

        public class CallDataItem
        {
            public ImageSource ImageSource { get; set; }
            public string AudioSourceFilename { get; set; }
        }
        public ObservableCollection<CallDataItem> CallDisplayItems { get; set; }
        public CallDataItem SelectedCallDisplayItem { get; set; }


        #region *// Audio
        ISimpleAudioPlayer player;
        VuPlayer vuPlayer;
        public bool IsPlaying { get; set; }
        public bool HasCallAudio { get; set; }
        public bool HasCallImage { get; set; }
        public double VuDecibelValue { get; set; }
        #endregion

        #region *// Menu related
        public ICommand InvalidateMenuCommand { get; set; }
        public bool IsLoggedIn { get; set; } = false;
        #endregion
        public DisplaySpeciesTabbedPageViewModel(Species species)
        {
            this.Species = species;

            ImageDataItems = new ObservableCollection<ImageDataItem>(); 
            DetailsHtmlSource = new HtmlWebViewSource();
            CallDisplayItems = new ObservableCollection<CallDataItem>();
            HtmlFontSizePercentage = Constants.HTML_FONT_SIZE_PERCENTAGE;

            player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            player.PlaybackEnded += (s, e) => { 
                IsPlaying = false;
                vuPlayer.Stop();
                VuDecibelValue = 0;
            };

            vuPlayer = new VuPlayer();
        }

        public ICommand OnFirstAppearance => commandHelper.ProduceDebouncedCommand(() => {

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var imageDataItems = new ObservableCollection<ImageDataItem>();
            foreach (var imageSourceName in Species.Images)
            {
                var fullFilename = ZippedFiles.GetFullFilename(imageSourceName);
                if (File.Exists(fullFilename))
                {
                    var imageSource = ImageSource.FromFile(ZippedFiles.GetFullFilename(imageSourceName));
                    imageDataItems.Add(new ImageDataItem { ImageSource = imageSource });
                }
                else
                {
                    imageDataItems.Add(new ImageDataItem { ImageSource = "bat.png" });
                }
            }

            ImageDataItems = imageDataItems;

            DetailsHtmlSource.Html = Species.DetailsHtml;

            DistributionMapImage = ImageSource.FromFile(ZippedFiles.GetFullFilename(Species.DistributionMapImage));

            var callDisplayItems = new ObservableCollection<CallDataItem>();
            foreach (var callData in Species.CallDatas)
            {
                if (!callData.ImageName.IsEmpty())
                {
                    callDisplayItems.Add(new CallDataItem { ImageSource = ImageSource.FromFile(ZippedFiles.GetFullFilename(callData.ImageName))});
                    HasCallImage = true;
                    HasCallAudio = false;
                }
                else if (!callData.AudioName.IsEmpty())
                {
                    callDisplayItems.Add(new CallDataItem { AudioSourceFilename = callData.AudioName });
                    HasCallAudio = true;
                    HasCallImage = false;
                }
                else
                {
                    HasCallImage = false;
                    HasCallAudio = false;
                }
            }
            CallDisplayItems = callDisplayItems;

            Debug.WriteLine($"Operation took {stopWatch.ElapsedMilliseconds} ms");
        });

        public ICommand OnSubsequentAppearance => commandHelper.ProduceDebouncedCommand(() => { });

        public ICommand OnBackMenuPressed => new Command(() =>
        {
            if (IsPlaying)
            {
                player.Stop();
                vuPlayer.Stop();
            }
            NavigateBack(NavigateReturnType.IsCancelled);
        });

        public bool IsHomeEnabled { get; set; }
        public ICommand OnHomeMenuPressed => new Command(() =>
        {
            if (IsPlaying)
            {
                player.Stop();
                vuPlayer.Stop();
            }
            NavigateBack(NavigateReturnType.GotoRoot);
        });

        public bool IsResetFilterEnabled { get; set; }
        public ICommand OnResetMenuPressed => new Command(() =>
        {
            NavigateBack(NavigateReturnType.GotoFilterReset);
        });

        public bool isBackCancelled = false;
        public ICommand OnBackButtonPressed => new Command(() =>
        {
            NavigateBack(NavigateReturnType.IsCancelled);
            isBackCancelled = true;
        });

        public ICommand OnDisplayBatDataMenuPressed => commandHelper.ProduceDebouncedCommand(async () => {
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

        public ICommand OnStartStopPlaybackPressed => commandHelper.ProduceDebouncedCommand(async () => {
            try
            {
                if (player.IsPlaying)
                {
                    player.Stop();
                    vuPlayer.Stop();
                    VuDecibelValue = 0;
                    IsPlaying = false;
                }
                else
                {
                    await ActivityIndicatorStart();
                    var audioFilename = CallDisplayItems[0].AudioSourceFilename;
                    var audioFullFilename = ZippedFiles.GetFullFilename(audioFilename);
                    if (!File.Exists(audioFullFilename))
                    {
                        throw new ApplicationException("Audio file does not exist");
                    }

                    // var wavFullFilename = Path.Combine(ZippedFiles.FullFolderName, $"temp.wav");
                    // Mp3ToWav(audioFullFilename, wavFullFilename);
                    vuPlayer.Load(audioFullFilename);
                    vuPlayer.Play((decibels, isPlaying) =>
                    {
                        if (!isPlaying) return;
                        VuDecibelValue = (decibels - vuPlayer.channel.minDb) / (vuPlayer.channel.maxDb - vuPlayer.channel.minDb);
                    });

                    player.Load(new FileStream(audioFullFilename, FileMode.Open, FileAccess.Read));
                    player.Play();
                    IsPlaying = true;

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
        });
        public ICommand OnAddSightingMenuPressed => commandHelper.ProduceDebouncedCommand(async () => {
            try
            {
                var cts = new CancellationTokenSource();
                ActivityIndicatorStart("Starting ...", () =>
                {
                    cts.Cancel();
                    ActivityIndicatorStop();
                });
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                var location = await Geolocation.GetLocationAsync(request, cts.Token);
                if (location == null)
                {
                    var isAccepted = await Application.Current.MainPage.DisplayAlert("Notice", "Unable to read location. Sighting to be saved without location data", "Ok", "Cancel");
                    if (!isAccepted) return;
                }

                #region *// Create sighting
                App.dbase.Sightings.Add(new Sighting
                {
                    Lat = location?.Latitude ?? 0,
                    Lon = location?.Longitude?? 0,
                    TimeStamp = DateTimeOffset.Now,
                    GenusId = Species.GenusId,
                    SpeciesId = Species.SpeciesId
                });
                Dbase.Save(App.dbase);
                #endregion

                #region *// Display sighting
                var sightingsPageViewModel = new SightingsPageViewModel() { IsHomeEnabled = IsHomeEnabled };
                var sightingsPage = new SightingsPage(sightingsPageViewModel);
                var returnType = await NavigateToPageAsync(sightingsPage, sightingsPageViewModel);
                if (returnType == NavigateReturnType.IsCancelled) return;
                if (returnType == NavigateReturnType.GotoRoot) NavigateBack(NavigateReturnType.GotoRoot);
                #endregion

            }
            catch (FeatureNotSupportedException)
            {
                throw new BusinessException("Location not supported on device");
            }
            catch (FeatureNotEnabledException)
            {
                throw new BusinessException("Location not enabled on device");
            }
            catch (PermissionException)
            {
                throw new BusinessException("No permission to access location on device");
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
