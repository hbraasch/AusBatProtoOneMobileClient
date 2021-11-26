using Mobile.Helpers;
using Mobile.ViewModels;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TreeApp.Helpers;
using Xamarin.Forms;
using static AusBatProtoOneMobileClient.ViewModels.DisplaySpeciesTabbedPageViewModel;

namespace AusBatProtoOneMobileClient.ViewModels
{
    public class DisplayMeasurementsPageViewModel : ViewModelBase
    {
        CommandHelper commandHelper = new CommandHelper();

        public HtmlTable measurementsTable { get; set; }
        public ImageSource headImageSource;

        #region *// Menu related
        public ICommand InvalidateMenuCommand { get; set; }
        public bool IsLoggedIn { get; set; } = false;
        #endregion
        public DisplayMeasurementsPageViewModel(HtmlTable measurementsTable, ImageSource headImageSource)
        {
            this.measurementsTable = measurementsTable;
            this.headImageSource = headImageSource;
        }

        public ICommand Appearance => commandHelper.ProduceDebouncedCommand(() => { });

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
