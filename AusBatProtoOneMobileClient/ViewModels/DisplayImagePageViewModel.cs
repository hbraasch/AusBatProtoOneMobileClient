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
    public class DisplayImagePageViewModel : ViewModelBase
    {
        CommandHelper commandHelper = new CommandHelper();

        public ImageSource ImageSource { get; set; }

        #region *// Menu related
        public ICommand InvalidateMenuCommand { get; set; }
        #endregion
        public DisplayImagePageViewModel(ImageSource imageSource)
        {
            ImageSource = imageSource;
        }

        public ICommand OnFirstAppearance => commandHelper.ProduceDebouncedCommand(() => { });

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
