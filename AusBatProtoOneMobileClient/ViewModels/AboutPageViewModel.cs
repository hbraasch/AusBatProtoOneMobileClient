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
    public class AboutPageViewModel : ViewModelBase
    {
        CommandHelper commandHelper = new CommandHelper();

        public HtmlWebViewSource WebViewSource { get; set; }

        #region *// Menu related
        public ICommand InvalidateMenuCommand { get; set; }
        public bool IsLoggedIn { get; set; } = false;
        #endregion
        public AboutPageViewModel()
        {
            WebViewSource = new HtmlWebViewSource();
        }

        public ICommand OnFirstAppearance => commandHelper.ProduceDebouncedCommand(() => 
        {
            WebViewSource.Html = App.dbase.AboutHtml + $"<p style=\"color: red\">Version: {Settings.CurrentDataVersion}</p>";
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
