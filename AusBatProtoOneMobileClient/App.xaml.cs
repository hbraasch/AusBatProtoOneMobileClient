using AusBatProtoOneMobileClient;
using AusBatProtoOneMobileClient.Models;
using AusBatProtoOneMobileClient.ViewModels;
using DocGenOneMobileClient.Views;
using Mobile.Models;
using System;
using System.Diagnostics;
using System.Linq;
using Xamarin.Essentials;
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
            VersionTracking.Track();

            var viewModel = new StartupPageViewModel();
            MainPage = new NavigationPage(new StartupPage(viewModel)) { BarBackgroundColor = Color.Black, BarTextColor = Color.White, BackgroundColor = Color.Black};


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
