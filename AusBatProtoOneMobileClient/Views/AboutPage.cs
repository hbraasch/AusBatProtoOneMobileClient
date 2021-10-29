using AusBatProtoOneMobileClient.Models;
using AusBatProtoOneMobileClient.ViewModels;
using AusBatProtoOneMobileClient.Views.Components;
using Mobile.Helpers;
using Mobile.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace AusBatProtoOneMobileClient
{
    public class AboutPage : ContentPageBase
    {
        AboutPageViewModel viewModel;
        MenuGenerator menu;
        WebView webView;
        public AboutPage(AboutPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            webView = (DeviceInfo.Platform != DevicePlatform.UWP) ? new TransparentWebView() : new WebView();
            webView.SetBinding(WebView.SourceProperty, new Binding(nameof(AboutPageViewModel.WebViewSource), BindingMode.OneWay));
            webView.Margin = 5;

            NavigationPage.SetTitleView(this, new Label { Text = "About", Style = Styles.TitleLabelStyle });
            BackgroundColor = Color.Black;

            Content = webView;

            menu = new MenuGenerator().Configure()
                .AddMenuItem("back", "Back", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(null); });
                
            menu.GenerateToolbarItemsForPage(this);
            menu.SetBinding(MenuGenerator.InvalidateCommandProperty, new Binding(nameof(AboutPageViewModel.InvalidateMenuCommand), BindingMode.OneWayToSource, source: viewModel));
            
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
