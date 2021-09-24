using AusBatProtoOneMobileClient.ViewModels;
using Mobile.Helpers;
using Mobile.ViewModels;
using Xamarin.Forms;


namespace AusBatProtoOneMobileClient
{
    public class DisplayBatDetailsTabPage : ContentPageBase
    {
        DisplayBatTabbedPageViewModel viewModel;
        WebView webView;
        public DisplayBatDetailsTabPage(DisplayBatTabbedPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            webView = new WebView();          
            webView.SetBinding(WebView.SourceProperty, new Binding(nameof(DisplayBatTabbedPageViewModel.DetailsHtmlSource), BindingMode.OneWay));


            Title = "Details";

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
