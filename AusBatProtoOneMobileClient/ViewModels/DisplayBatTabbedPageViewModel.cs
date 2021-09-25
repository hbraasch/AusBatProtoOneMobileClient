using AusBatProtoOneMobileClient.Data;
using AusBatProtoOneMobileClient.Models;
using Mobile.Helpers;
using Mobile.ViewModels;
using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TreeApp.Helpers;
using Xamarin.Forms;

namespace AusBatProtoOneMobileClient.ViewModels
{
    public class DisplayBatTabbedPageViewModel : ViewModelBase
    {
        public Bat bat;
        CommandHelper commandHelper = new CommandHelper();

        public class ImageDataItem
        {
            public string ImageSource { get; set; }
        }

        public ObservableCollection<ImageDataItem> ImageDataItems { get; set; }
        public ObservableCollection<MapRegion> SelectedMapItems { get; set; }
        
        public HtmlWebViewSource DetailsHtmlSource { get; set; }

        ISimpleAudioPlayer player;
        public bool IsPlaying { get; set; }
        public ObservableCollection<CallDataItem> CallDataItems { get; set; }
        public int CallDataItemIndex { get; set; }

        #region *// Menu related
        public ICommand InvalidateMenuCommand { get; set; }
        public bool IsLoggedIn { get; set; } = false;
        #endregion
        public DisplayBatTabbedPageViewModel(Bat bat)
        {
            this.bat = bat;
            ImageDataItems = new ObservableCollection<ImageDataItem>();
            SelectedMapItems = new ObservableCollection<MapRegion>();
            DetailsHtmlSource = new HtmlWebViewSource();
            CallDataItems = new ObservableCollection<CallDataItem>();

            foreach (var imageSource in bat.Images)
            {
                ImageDataItems.Add(new ImageDataItem { ImageSource = imageSource });
                }

            DetailsHtmlSource.Html = bat.Details;


            foreach (var region in bat.MapRegions)
            {
                SelectedMapItems.Add(region);
            }

            player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            player.PlaybackEnded += (s, e) => { IsPlaying = false; };
            foreach (var call in bat.Calls)
            {
                CallDataItems.Add(call);
            }
            IsPlaying = false; 

        }

        public ICommand OnFirstAppearance => commandHelper.ProduceDebouncedCommand(() => { 
        
        });

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
                    IsPlaying = false;
                }
                else
                {
                    var audioFilename = CallDataItems[CallDataItemIndex].CallFilename;
                    player.Load(GetStreamFromFile($"Data.CallAudio.{audioFilename}"));
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

        Stream GetStreamFromFile(string filename)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;

            var stream = assembly.GetManifestResourceStream("AusBatProtoOneMobileClient." + filename);

            return stream;
        }
        public ICommand OnTestMenuPressed => commandHelper.ProduceDebouncedCommand(async () => {
            try
            {
                await DisplayAlert("Alert", "Next page", "Ok");
                var cts = new CancellationTokenSource();
                ActivityIndicatorStart("Starting ...", () =>
                {
                    cts.Cancel();
                    ActivityIndicatorStop();
                    Debug.WriteLine("Cancel pressed");
                });
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
