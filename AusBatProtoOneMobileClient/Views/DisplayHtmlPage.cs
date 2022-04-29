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
    public class DisplayHtmlPage : ContentPageBase
    {
        DisplayHtmlPageViewModel viewModel;
        MenuGenerator menu;
        WebView webView;
        public DisplayHtmlPage(DisplayHtmlPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            webView = (DeviceInfo.Platform != DevicePlatform.UWP) ? new TransparentWebView() { FontSizePercentage = Settings.HtmlFontSizePercentage } : new WebView();
            webView.SetBinding(WebView.SourceProperty, new Binding(nameof(DisplayHtmlPageViewModel.WebViewSource), BindingMode.OneWay));
            webView.Margin = 5;



            var titleLabel = new Xamarin.Forms.Label { Text = "Filter results", Style = Styles.TitleLabelStyle };
            titleLabel.SetBinding(Label.TextProperty, new Binding(nameof(DisplayHtmlPageViewModel.Title), BindingMode.OneWay));
            NavigationPage.SetTitleView(this, titleLabel);

            BackgroundColor = Color.Black;

            Content = webView;

            menu = new MenuGenerator().Configure()
                .AddMenuItem("back", "Back", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(null); });
                
            menu.GenerateToolbarItemsForPage(this);
            menu.SetBinding(MenuGenerator.InvalidateCommandProperty, new Binding(nameof(DisplayHtmlPageViewModel.InvalidateMenuCommand), BindingMode.OneWayToSource, source: viewModel));
            
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
