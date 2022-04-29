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
        public string Html { get; set; }
        public HtmlWebViewSource WebViewSource { get; set; }

        #region *// Menu related
        public ICommand InvalidateMenuCommand { get; set; }
        public bool IsLoggedIn { get; set; } = false;
        #endregion
        public DisplayHtmlPageViewModel()
        {
            WebViewSource = new HtmlWebViewSource();
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

    }
}
