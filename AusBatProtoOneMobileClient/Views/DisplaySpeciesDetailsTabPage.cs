﻿using AusBatProtoOneMobileClient.Models;
using AusBatProtoOneMobileClient.ViewModels;
using AusBatProtoOneMobileClient.Views.Components;
using Mobile.Helpers;
using Mobile.ViewModels;
using System;
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
            webView.OnMeasurementClickedIOS = () =>
            {
                viewModel.OnDisplayMeasurementsTableClicked.Execute(null);
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
            if (!e.Url.StartsWith("url", StringComparison.InvariantCultureIgnoreCase)) {
                viewModel.OnDisplayMeasurementsTableClicked.Execute(null);
            };
            e.Cancel = true;
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
