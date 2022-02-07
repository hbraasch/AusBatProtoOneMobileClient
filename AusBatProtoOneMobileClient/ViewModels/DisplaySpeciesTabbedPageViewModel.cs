using AusBatProtoOneMobileClient.Data;
using AusBatProtoOneMobileClient.Helpers;
using AusBatProtoOneMobileClient.Models;
using AusBatProtoOneMobileClient.Views.Components;
using DocGenOneMobileClient.Views;
using HtmlAgilityPack;
using Mobile.Helpers;
using Mobile.Models;
using Mobile.ViewModels;
using Newtonsoft.Json;
using Plugin.SimpleAudioPlayer;
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
using static AusBatProtoOneMobileClient.Helpers.VuHelper;

namespace AusBatProtoOneMobileClient.ViewModels
{
    public class DisplaySpeciesTabbedPageViewModel : ViewModelBase
    {
        public Species Species;

        CommandHelper commandHelper = new CommandHelper();

        public class ImageDataItem
        {
            public ImageSource ImageSource { get; set; }
            public Action<ImageSource> OnImageTapped { get; set; }
        }

        public ObservableCollection<ImageDataItem> ImageDataItems { get; set; }
        public ObservableCollection<MapRegion> SelectedMapItems { get; set; }

        public ImageSource DistributionMapImage { get; set; }


        public HtmlWebViewSource DetailsHtmlSource { get; set; }
        public float HtmlFontSizePercentage { get; set; }

        public HtmlTable MeasurementsTable;

        public class CallDataItem
        {
            public ImageSource ImageSource { get; set; }
            public string AudioSourceFilename { get; set; }
        }
        public ObservableCollection<CallDataItem> CallDisplayItems { get; set; }
        public CallDataItem SelectedCallDisplayItem { get; set; }

        public ImageSource HeadImageSource = null;

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
        public bool IsDetailsDisplay { get; set; }
        #endregion

        public DisplaySpeciesTabbedPageViewModel(Species species)
        {
            this.Species = species;

            ImageDataItems = new ObservableCollection<ImageDataItem>(); 
            DetailsHtmlSource = new HtmlWebViewSource();
            CallDisplayItems = new ObservableCollection<CallDataItem>();
            HtmlFontSizePercentage = Settings.HtmlFontSizePercentage;

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
                    if (fullFilename.Contains("_head")) HeadImageSource = imageSource;
                    imageDataItems.Add(new ImageDataItem { ImageSource = imageSource, OnImageTapped = OnImageTapped });
                }
                else
                {
                    imageDataItems.Add(new ImageDataItem { ImageSource = "bat.png" });
                }
            }

            ImageDataItems = imageDataItems;

            DetailsHtmlSource = GenerateSource(Species.DetailsHtml, out MeasurementsTable); 

            DistributionMapImage = ImageSource.FromFile(ZippedFiles.GetFullFilename(Species.DistributionMapImage));

            var callDisplayItems = new ObservableCollection<CallDataItem>();
            if (!Species.CallImages.IsEmpty())
            {
                // There is a call image
                callDisplayItems.Add(new CallDataItem { ImageSource = ImageSource.FromFile(ZippedFiles.GetFullFilename(Species.CallImages.First())) });
                HasCallImage = true;
                HasCallAudio = false;
            }
            else
            {
                // No call image, but there could be call audio
                if (!Species.CallAudios.IsEmpty())
                {
                    // There is some audio
                    callDisplayItems.Add(new CallDataItem { AudioSourceFilename = Species.CallAudios.First() });
                    HasCallAudio = true;
                    HasCallImage = false;
                }
                else
                {
                    // No call data
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

        /// <summary>
        /// Used to encapsulate extracted table data
        /// </summary>
        public class HtmlTable
        {
            public class Row
            {
                public List<Col> Columns = new List<Col>();

            }

            public class Col
            {
                public string Value;
            }

            public List<Row> Rows = new List<Row>();

            public int ColAmount => Rows[0].Columns.Count;



            /// <summary>
            /// Used to create a table with first half of data. Retain header and row names
            /// </summary>
            /// <returns></returns>
            internal HtmlTable FirstHalf()
            {
                var startIndex = (int)((float)(Rows[0].Columns.Count) / 2);
                var removeAmount = Rows[0].Columns.Count - startIndex;
                var newTable = this.Clone() as HtmlTable;
                foreach (var row in newTable.Rows)
                {
                    for (int i = 1; i < removeAmount; i++)
                    {
                        row.Columns.RemoveAt(startIndex + 1);
                    }
                }
                return newTable;
            }



            /// <summary>
            /// Used to create a table with second half of data. Retain header and row names
            /// </summary>
            /// <returns></returns>
            internal HtmlTable SecondHalf()
            {
                var startIndex = 1;
                var removeAmount = (int)((float)(Rows[0].Columns.Count) / 2);
                var newTable = this.Clone() as HtmlTable;
                foreach (var row in newTable.Rows)
                {
                    for (int i = 0; i < removeAmount; i++)
                    {
                        row.Columns.RemoveAt(startIndex);
                    }
                }
                return newTable;
            }

            private HtmlTable Clone()
            {
                return JsonConvert.DeserializeObject<HtmlTable>(JsonConvert.SerializeObject(this));
            }
        }

        /// <summary>
        /// Used to also allow local objects to be linked to 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="htmlTable"></param>
        /// <returns></returns>
        private HtmlWebViewSource GenerateSource(string html, out HtmlTable htmlTable)
        {
            var source = new HtmlWebViewSource();
            source.Html = ConvertHtml(html, out htmlTable);
            source.BaseUrl = DependencyService.Get<IBaseUrl>().Get();
            return source;
        }
        public static string ConvertHtml(string detailsHtml, out HtmlTable htmlTable)
        {
            // https://stackoverflow.com/questions/35413763/xamarin-parsing-html

            HtmlDocument document = new HtmlDocument();
            htmlTable = new HtmlTable();
            //your html stream
            document.LoadHtml(detailsHtml);
            var container = document.DocumentNode.Descendants("table").FirstOrDefault();
            if (container != null)
            {
                foreach (var row in container.Descendants("tr"))
                {
                    var htmlRow = new HtmlTable.Row();
                    foreach (var col in row.Descendants("th"))
                    {
                        htmlRow.Columns.Add(new HtmlTable.Col { Value = col.InnerText });
                    }
                    foreach (var col in row.Descendants("td"))
                    {
                        htmlRow.Columns.Add(new HtmlTable.Col { Value = col.InnerText });
                    }
                    htmlTable.Rows.Add(htmlRow);
                }
                document.DocumentNode.SelectSingleNode(container.XPath).Remove();
            }

            foreach (var item in document.DocumentNode.Descendants("p"))
            {
                if (item.InnerText.Contains("Measurement"))
                {
                    item.InnerHtml = "<a href='measurements.html'>Measurements</a>";
                }
            }
            var stringWriter = new StringWriter();
            document.Save(stringWriter);
            return stringWriter.ToString();
        }

        public Action<ImageSource> OnImageTapped => async (imageSource) =>
        {
            var viewModel = new DisplayImagePageViewModel(imageSource);
            var page = new DisplayImagePage(viewModel);
            await NavigateToPageAsync(page, viewModel);
        };

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

        public ICommand OnDisplayMeasurementsTableClicked => commandHelper.ProduceDebouncedCommand(async () => {
            try
            {

                var viewModel = new DisplayMeasurementsPageViewModel(MeasurementsTable, HeadImageSource);
                var page = new DisplayMeasurementsPage(viewModel);
                await NavigateToPageAsync(page, viewModel);

                DetailsHtmlSource = GenerateSource(Species.DetailsHtml, out MeasurementsTable);
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
        
        public ICommand OnScaleTextMenuPressed => commandHelper.ProduceDebouncedCommand<TransparentWebView>(async (webView) => {
            try
            {

                var cts = new CancellationTokenSource();
                ActivityIndicatorStart("Starting ...", () =>
                {
                    cts.Cancel();
                    ActivityIndicatorStop();
                });

                // Do work here

                var value = await Application.Current.MainPage.DisplayPromptAsync("Font size", "Enter percentage", "OK", "Cancel", initialValue: Settings.HtmlFontSizePercentage.ToString("N0"), keyboard: Keyboard.Text);
                if (value == null) return;
                var isValid = float.TryParse(value, out float percentage);
                if (!isValid) throw new BusinessException("Value entered is not a valid number");
                percentage = Math.Max(10, percentage);
                percentage = Math.Min(percentage, 1000);
                Settings.HtmlFontSizePercentage = percentage;
                HtmlFontSizePercentage = percentage;

                DetailsHtmlSource = GenerateSource(Species.DetailsHtml, out MeasurementsTable);

                if (DeviceInfo.Platform == DevicePlatform.Android) webView.Reload(); 


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
