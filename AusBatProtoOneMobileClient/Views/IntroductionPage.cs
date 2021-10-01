﻿using AusBatProtoOneMobileClient.Models;
using AusBatProtoOneMobileClient.ViewModels;
using AusBatProtoOneMobileClient.Views.Components;
using Mobile.Helpers;
using Mobile.ViewModels;
using Xamarin.Forms;


namespace AusBatProtoOneMobileClient
{
    public class IntroductionPage : ContentPageBase
    {
        IntroductionPageViewModel viewModel;
        MenuGenerator menu;
        public IntroductionPage(IntroductionPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            var webView = new TransparentWebView();
            webView.SetBinding(WebView.SourceProperty, new Binding(nameof(IntroductionPageViewModel.WebViewSource), BindingMode.OneWay));

            var mainLayout = new StackLayout { Children = { webView }, Margin = 5 };

            Title = "Introduction";
            BackgroundImageSource = Constants.BACKGROUND_IMAGE;

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
