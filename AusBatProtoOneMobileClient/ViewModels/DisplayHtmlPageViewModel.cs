using AusBatProtoOneMobileClient.Views.Components;
using Mobile.Helpers;
using Mobile.Models;
using Mobile.ViewModels;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TreeApp.Helpers;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AusBatProtoOneMobileClient.ViewModels
{
    public class DisplayHtmlPageViewModel : ViewModelBase
    {

        CommandHelper commandHelper = new CommandHelper();

        public string Title { get; set; }
        public float HtmlFontSizePercentage { get; set; }
        public string Html { get; set; }
        public HtmlWebViewSource WebViewSource { get; set; }

        #region *// Menu related
        public ICommand InvalidateMenuCommand { get; set; }
        public bool IsLoggedIn { get; set; } = false;
        #endregion
        public DisplayHtmlPageViewModel()
        {
            WebViewSource = new HtmlWebViewSource();
            HtmlFontSizePercentage = Settings.HtmlFontSizePercentage;
        }

        public ICommand OnFirstAppearance => commandHelper.ProduceDebouncedCommand(() => 
        {
            WebViewSource.Html = Html; 
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

                WebViewSource.Html = Html;

                webView.Reload();  //if (DeviceInfo.Platform == DevicePlatform.Android) webView.Reload();


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
