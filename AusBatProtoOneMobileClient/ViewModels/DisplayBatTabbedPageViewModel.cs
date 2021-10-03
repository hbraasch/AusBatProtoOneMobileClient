using AusBatProtoOneMobileClient.Data;
using AusBatProtoOneMobileClient.Helpers;
using AusBatProtoOneMobileClient.Models;
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

        public string DistributionMapImage { get; set; }


        public HtmlWebViewSource DetailsHtmlSource { get; set; }

        ISimpleAudioPlayer player;

        public class CallDisplayItem : INotifyPropertyChanged
        {
            public Command OnStartStopClicked { get;  set; }
            public bool IsPlaying { get; set; }
            public CallDataItem Content { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;
        }
        public ObservableCollection<CallDisplayItem> CallDisplayItems { get; set; }
        public CallDisplayItem SelectedCallDisplayItem { get; set; }
        public int CallDataItemIndex { get; set; }

        #region *// Menu related
        public ICommand InvalidateMenuCommand { get; set; }
        public bool IsLoggedIn { get; set; } = false;
        #endregion
        public DisplayBatTabbedPageViewModel(Bat bat)
        {
            this.bat = bat;

            ImageDataItems = new ObservableCollection<ImageDataItem>();
            foreach (var imageSource in bat.Images)
            {
                ImageDataItems.Add(new ImageDataItem { ImageSource = imageSource });
            }


            DetailsHtmlSource = new HtmlWebViewSource();
            DetailsHtmlSource.Html = bat.DetailsHtml;

            DistributionMapImage = bat.DistributionMapImage;

            player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            player.PlaybackEnded += (s, e) => { SelectedCallDisplayItem.IsPlaying = false; };
            CallDisplayItems = new ObservableCollection<CallDisplayItem>();
            foreach (var call in bat.Calls)
            {
                CallDisplayItems.Add(new CallDisplayItem { 
                    OnStartStopClicked = new Command(async ()=> { await OnStartStopPlaybackPressed(call); }), 
                    IsPlaying = false,
                    Content = call
                }); 
            }
        }

        public ICommand OnFirstAppearance => commandHelper.ProduceDebouncedCommand(() => { 
        
        });

        public ICommand OnSubsequentAppearance => commandHelper.ProduceDebouncedCommand(() => { });

        public ICommand OnBackMenuPressed => new Command(() =>
        {
            NavigateBack(NavigateReturnType.IsCancelled);
        });

        public bool IsHomeEnabled { get; set; }
        public ICommand OnHomeMenuPressed => new Command(() =>
        {
            NavigateBack(NavigateReturnType.GotoRoot);
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


        public async Task OnStartStopPlaybackPressed(CallDataItem callDataItem)
        { 
            try
            {
                SelectedCallDisplayItem = CallDisplayItems.FirstOrDefault(o => o.Content == callDataItem);

                if (player.IsPlaying)
                {
                    player.Stop();
                    SelectedCallDisplayItem.IsPlaying = false;
                }
                else
                {
                    var audioFilename = callDataItem.CallAudioFilename;
                    if (audioFilename == "")
                    {
                        throw new BusinessException("There is no audio to play");
                    }
                    player.Load(FileHelper.GetStreamFromFile($"Data.CallAudio.{audioFilename}"));
                    player.Play();
                    SelectedCallDisplayItem.IsPlaying = true;
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
