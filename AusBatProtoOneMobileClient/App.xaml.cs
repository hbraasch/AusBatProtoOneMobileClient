using AusBatProtoOneMobileClient;
using AusBatProtoOneMobileClient.Models;
using AusBatProtoOneMobileClient.ViewModels;
using Mobile.Models;
using System.Diagnostics;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AusBatProtoOneMobileClient
{
    public partial class App : Application
    {

        public static Dbase dbase;
        public static double screenWidth;
        public static double screenHeight;
        public App()
        {
            InitializeComponent();

#if false
            Dbase.Clear();
            Debugger.Break();
#endif
            dbase = Dbase.Load();

            DeviceDisplay.MainDisplayInfoChanged += OnMainDisplayInfoChanged;

            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            UpdateScreenSizeInfo(mainDisplayInfo);

            var viewModel = new StartupPageViewModel();
            MainPage = new NavigationPage(new StartupPage(viewModel)) { BarBackgroundColor = Color.Black, BarTextColor = Color.White };


        }

        void OnMainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
        {
            UpdateScreenSizeInfo(e.DisplayInfo);
        }

        void UpdateScreenSizeInfo(DisplayInfo displayInfo)
        {
            screenWidth = displayInfo.Width;
            screenHeight = displayInfo.Height;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
