using AusBatProtoOneMobileClient;
using AusBatProtoOneMobileClient.Models;
using AusBatProtoOneMobileClient.ViewModels;
using Mobile.Models;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace AusBatProtoOneMobileClient
{
    public partial class App : Application
    {

        public static Dbase dbase; 
        public App()
        {
            InitializeComponent();

#if false
            Dbase.Clear();
            Debugger.Break();
#endif
            dbase = Dbase.Load();

            var viewModel = new StartupPageViewModel();
            MainPage = new NavigationPage(new StartupPage(viewModel)) { BarBackgroundColor = Color.Black, BarTextColor = Color.White };


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
