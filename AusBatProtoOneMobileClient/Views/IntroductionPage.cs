using AusBatProtoOneMobileClient.Helpers;
using AusBatProtoOneMobileClient.Models;
using AusBatProtoOneMobileClient.ViewModels;
using AusBatProtoOneMobileClient.Views.Components;
using Mobile.Helpers;
using Mobile.Models;
using Mobile.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace AusBatProtoOneMobileClient
{
    public class IntroductionPage : ContentPageBase
    {
        IntroductionPageViewModel viewModel;
        MenuGenerator menu;
        WebView webView;
        public IntroductionPage(IntroductionPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            webView = (DeviceInfo.Platform != DevicePlatform.UWP) ? new TransparentWebView() { FontSizePercentage = Settings.HtmlFontSizePercentage } : new WebView();
            webView.SetBinding(WebView.SourceProperty, new Binding(nameof(IntroductionPageViewModel.WebViewSource), BindingMode.OneWay));
            webView.Margin = 10; 

            NavigationPage.SetTitleView(this, new Xamarin.Forms.Label { Text = "Introduction", Style = Styles.TitleLabelStyle });
            BackgroundColor = Color.Black;

            Content = webView;

            menu = new MenuGenerator().Configure()
                .AddMenuItem("back", "Back", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(null); });
                
            menu.GenerateToolbarItemsForPage(this);
            menu.SetBinding(MenuGenerator.InvalidateCommandProperty, new Binding(nameof(IntroductionPageViewModel.InvalidateMenuCommand), BindingMode.OneWayToSource, source: viewModel));
            
        }

        bool isFirstAppearance = true;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (isFirstAppearance)
            {
                isFirstAppearance = false;
                viewModel.OnFirstAppearance.Execute(null);
            }
            else
            {
                viewModel.OnSubsequentAppearance.Execute(null);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            viewModel.OnBackButtonPressed.Execute(null);
            return viewModel.isBackCancelled;
        }
    }
}
