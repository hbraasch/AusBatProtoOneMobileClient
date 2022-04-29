using AusBatProtoOneMobileClient.Models;
using AusBatProtoOneMobileClient.ViewModels;
using AusBatProtoOneMobileClient.Views.Components;
using Mobile.Helpers;
using Mobile.ViewModels;
using System;
using System.Linq;
using Xamarin.Forms;


namespace AusBatProtoOneMobileClient
{
    public class DisplaySpeciesDetailsTabPage : ContentPageBase
    {
        DisplaySpeciesTabbedPageViewModel viewModel;
        public TransparentWebView webView;
        public DisplaySpeciesDetailsTabPage(DisplaySpeciesTabbedPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;


            webView = new TransparentWebView() { Margin = 5 };

            #region *// Navigation. Needed to handle Droid and IOS differently. IOS does not trigger the Xamarin Forms [Navigating] event 
            webView.Navigating += WebViewOnNavigatingDroid;
            webView.OnMeasurementClickedIOS = (index) =>
            {
                viewModel.OnDisplayMeasurementsTableClicked.Execute(index);
            }; 
            #endregion

            webView.SetBinding(TransparentWebView.SourceProperty, new Binding(nameof(DisplaySpeciesTabbedPageViewModel.DetailsHtmlSource), BindingMode.OneWay));
            webView.SetBinding(TransparentWebView.FontSizePercentageProperty, new Binding(nameof(DisplaySpeciesTabbedPageViewModel.HtmlFontSizePercentage), BindingMode.OneWay));

            NavigationPage.SetTitleView(this, new Label { Text = "Details", Style = Styles.TitleLabelStyle });
            BackgroundColor = Color.White;

            Content = webView;

        }


        private void WebViewOnNavigatingDroid(object sender, WebNavigatingEventArgs e)
        {
            if (e.Url.Contains("measurements.html")) {
                var index = ExtractIndex(e.Url);
                viewModel.OnDisplayMeasurementsTableClicked.Execute(index);
            };
            e.Cancel = true;
        }

        private int ExtractIndex(string url)
        {
            var firstSplit = url.Split('?');
            if (firstSplit.Length != 2) throw new ApplicationException("Url is incomplete");
            var secondSplit = firstSplit[1].Split('=');
            if (firstSplit.Length != 2) throw new ApplicationException("Url has no index parameter");
            var success = int.TryParse(secondSplit[1], out var value);
            if (!success) throw new ApplicationException("Url index parameter is not a number");
            return value;
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
