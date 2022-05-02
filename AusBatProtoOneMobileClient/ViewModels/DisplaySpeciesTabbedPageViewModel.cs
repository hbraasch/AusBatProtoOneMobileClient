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

        public List<TableData> MeasurementsTableDatas;

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

        #region *// Similar species
        public class SpeciesDisplayItem
        {
            public string SpeciesName { get; set; }
            public string FriendlyName { get; set; }
            public string ImageSource { get; set; }
            public Species Species { get; set; }
        }

        public ObservableCollection<SpeciesDisplayItem> SimilarSpeciesDisplayItems { get; set; }

        public SpeciesDisplayItem SimilarSpeciesSelectedItem { get; set; }
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
            SimilarSpeciesDisplayItems = new ObservableCollection<SpeciesDisplayItem>();

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

            DetailsHtmlSource = GenerateSource(Species.DetailsHtml, out MeasurementsTableDatas); 

            DistributionMapImage = ImageSource.FromFile(ZippedFiles.GetFullFilename(Species.DistributionMapImage));

            var callDisplayItems = new ObservableCollection<CallDataItem>();
            if (!Species.CallImages.IsEmpty())
            {
                // There are call images
                foreach (var callImage in Species.CallImages)
                {
                    callDisplayItems.Add(new CallDataItem { ImageSource = ImageSource.FromFile(ZippedFiles.GetFullFilename(callImage)) });
                }
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

            SimilarSpeciesDisplayItems = GenerateSimilarSpeciesDisplayItems(Species);

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
            NavigateBack(NavigateReturnType.GotoHome);
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

        private ObservableCollection<SpeciesDisplayItem> GenerateSimilarSpeciesDisplayItems(Species species)
        {
            var list = new ObservableCollection<SpeciesDisplayItem>();

            foreach (var similarSpeciesId in species.SimilarSpecies)
            {
                var similarSpecies = App.dbase.Species.FirstOrDefault(o => o.GenusId == similarSpeciesId.GenusId && o.SpeciesId == similarSpeciesId.SpeciesId);
                if (similarSpecies == null)
                {
                    Debug.WriteLine($"Species with Genus [similarSpeciesId.GenusId] and Species [similarSpeciesId.SpeciesId] does not exist");
                    continue;
                }
                var imageSource = (similarSpecies.Images.Count > 0) ? similarSpecies.Images.First() : "bat.png";
                list.Add(new SpeciesDisplayItem
                {
                    SpeciesName = $"{similarSpecies.GenusId.ToUpperFirstChar()} {similarSpecies.SpeciesId.ToLower()}",
                    FriendlyName = similarSpecies.Name,
                    ImageSource = imageSource,
                    Species = similarSpecies
                });
            }
            return list;
        }

        /// <summary>
        /// Used to encapsulate extracted table data
        /// </summary>
        public class TableData
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
            internal TableData FirstHalf()
            {
                var startIndex = (int)((float)(Rows[0].Columns.Count) / 2);
                var removeAmount = Rows[0].Columns.Count - startIndex;
                var newTable = this.Clone() as TableData;
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
            internal TableData SecondHalf()
            {
                var startIndex = 1;
                var removeAmount = (int)((float)(Rows[0].Columns.Count) / 2);
                var newTable = this.Clone() as TableData;
                foreach (var row in newTable.Rows)
                {
                    for (int i = 0; i < removeAmount; i++)
                    {
                        row.Columns.RemoveAt(startIndex);
                    }
                }
                return newTable;
            }

            private TableData Clone()
            {
                return JsonConvert.DeserializeObject<TableData>(JsonConvert.SerializeObject(this));
            }
        }

        /// <summary>
        /// Used to also allow local objects to be linked to 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="tableDatas"></param>
        /// <returns></returns>
        private HtmlWebViewSource GenerateSource(string html, out List<TableData> tableDatas)
        {
            var source = new HtmlWebViewSource();
            source.Html = ConvertHtml(html, out tableDatas);
            source.BaseUrl = DependencyService.Get<IBaseUrl>().Get();
            return source;
        }
        public static string ConvertHtml(string detailsHtml, out List<TableData> tableDatas)
        {
            // https://stackoverflow.com/questions/35413763/xamarin-parsing-html

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(detailsHtml);
            var containers = document.DocumentNode.Descendants("table").ToList();

            #region *// Run through each table, extract data and remove
            tableDatas = new List<TableData>();

            for (int i = 0; i < containers.Count; i++)
            {
                var container = containers[i];
                var tableData = new TableData();

                if (container != null)
                {
                    foreach (var row in container.Descendants("tr"))
                    {
                        var htmlRow = new TableData.Row();
                        foreach (var col in row.Descendants("th"))
                        {
                            htmlRow.Columns.Add(new TableData.Col { Value = col.InnerText });
                        }
                        foreach (var col in row.Descendants("td"))
                        {
                            htmlRow.Columns.Add(new TableData.Col { Value = col.InnerText });
                        }
                        tableData.Rows.Add(htmlRow);
                    }
                    document.DocumentNode.SelectSingleNode(container.XPath).Remove();
                    tableDatas.Add(tableData);   
                }
            }
            #endregion

            #region *// Replace [Measurements] string with an indexed hyperlink
            if (tableDatas.Count == 1)
            {
                foreach (var item in document.DocumentNode.Descendants("p"))
                {
                    if (item.InnerText.Contains("Measurements"))
                    {
                        item.InnerHtml = $"<a href='measurements.html?index={0}'>Measurements</a>";
                        break;
                    }
                }  
            }
            else
            {
                int hyperlinkCount = 0;
                foreach (var item in document.DocumentNode.Descendants("p"))
                {
                    if (item.InnerText.Contains("Measurements:"))
                    {
                        var region = ExtractRegion(item.InnerText);
                        item.InnerHtml = $"<a href='measurements.html?index={hyperlinkCount++}'>{region}</a>";
                    }
                }
            }
            #endregion
            var stringWriter = new StringWriter();
            document.Save(stringWriter);
            return stringWriter.ToString();
        }

        private static string ExtractRegion(string text)
        {
            var firstSplit = text.Split(':');
            if (firstSplit.Length != 2) throw new ApplicationException("Incorrect [Measurements:] tag");
            return firstSplit[1].Trim();
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

        public ICommand OnDisplayMeasurementsTableClicked => commandHelper.ProduceDebouncedCommand<int>(async (index) => {
            try
            {

                var viewModel = new DisplayMeasurementsPageViewModel(MeasurementsTableDatas[index], HeadImageSource);
                var page = new DisplayMeasurementsPage(viewModel);
                await NavigateToPageAsync(page, viewModel);

                DetailsHtmlSource = GenerateSource(Species.DetailsHtml, out MeasurementsTableDatas);
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

                DetailsHtmlSource = GenerateSource(Species.DetailsHtml, out MeasurementsTableDatas);

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
                ActivityIndicatorStart("Reading location ...", () =>
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
                if (returnType == NavigateReturnType.GotoHome) NavigateBack(NavigateReturnType.GotoHome);
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


        public ICommand OnSimilarSpeciesSelectMenuPressed => commandHelper.ProduceDebouncedCommand(async () =>
        {

            try
            {
                if (SimilarSpeciesSelectedItem == null) return;
                var viewModel = new DisplaySpeciesTabbedPageViewModel((SimilarSpeciesSelectedItem as SpeciesDisplayItem).Species) { IsHomeEnabled = IsHomeEnabled };
                var page = new DisplaySpeciesTabbedPage(viewModel);
                var resultType = await NavigateToPageAsync(page, viewModel);
                if (resultType == NavigateReturnType.GotoHome) NavigateBack(NavigateReturnType.GotoHome);
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
