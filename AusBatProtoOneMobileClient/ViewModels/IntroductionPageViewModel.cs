using Mobile.Helpers;
using Mobile.ViewModels;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TreeApp.Helpers;
using Xamarin.Forms;

namespace AusBatProtoOneMobileClient.ViewModels
{
    public class IntroductionPageViewModel : ViewModelBase
    {
        CommandHelper commandHelper = new CommandHelper();

        public HtmlWebViewSource WebViewSource { get; set; }

        #region *// Menu related
        public ICommand InvalidateMenuCommand { get; set; }
        public bool IsLoggedIn { get; set; } = false;
        #endregion
        public IntroductionPageViewModel()
        {
            WebViewSource = new HtmlWebViewSource();
            WebViewSource.Html = "";
        }

        public ICommand OnFirstAppearance => commandHelper.ProduceDebouncedCommand(() => {
            WebViewSource.Html = App.dbase.IntroductionHtml;
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
