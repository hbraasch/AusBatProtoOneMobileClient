using AusBatProtoOneMobileClient.Models;
using AusBatProtoOneMobileClient.ViewModels;
using AusBatProtoOneMobileClient.Views.Components;
using Mobile.Helpers;
using Mobile.ViewModels;
using Xamarin.Forms;


namespace AusBatProtoOneMobileClient
{
    public class DisplaySpeciesDetailsTabPage : ContentPageBase
    {
        DisplaySpeciesTabbedPageViewModel viewModel;
        TransparentWebView webView;
        public DisplaySpeciesDetailsTabPage(DisplaySpeciesTabbedPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            webView = new TransparentWebView() { Margin = 5 };          
            webView.SetBinding(WebView.SourceProperty, new Binding(nameof(DisplaySpeciesTabbedPageViewModel.DetailsHtmlSource), BindingMode.OneWay));

            NavigationPage.SetTitleView(this, new Label { Text = "Details", Style = Styles.TitleLabelStyle });
            BackgroundColor = Color.White;

            Content = webView;
            
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
