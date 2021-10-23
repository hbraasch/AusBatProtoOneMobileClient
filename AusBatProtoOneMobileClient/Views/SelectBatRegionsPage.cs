using AusBatProtoOneMobileClient.ViewModels;
using AusBatProtoOneMobileClient.Views.Components;
using Mobile.Helpers;
using Mobile.ViewModels;
using System;
using Xamarin.Forms;


namespace AusBatProtoOneMobileClient
{
    public class SelectBatRegionsPage : ContentPageBase
    {
        SelectBatRegionsPageViewModel viewModel;
        Map map;
        AbsoluteLayout layout;

        public SelectBatRegionsPage(SelectBatRegionsPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;
            var mainDisplayInfo = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo;
            map = new Map { IsSelectable = true };
            map.SetBinding(Map.SelectedItemsProperty, new Binding(nameof(SelectBatRegionsPageViewModel.SelectedMapRegions), BindingMode.TwoWay));

            Title = "Regions";
            BackgroundColor = Color.Black;

            double width;
            double height;
            if (mainDisplayInfo.Orientation == Xamarin.Essentials.DisplayOrientation.Portrait)
            {
                width = 1.0f;
                height = mainDisplayInfo .Width/ mainDisplayInfo.Height;
            }
            else
            {
                height = 1.0;
                width = mainDisplayInfo.Height / mainDisplayInfo.Width;
            }

            layout = new AbsoluteLayout();
            layout.Children.Add(map);
            AbsoluteLayout.SetLayoutFlags(map, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(map, new Rectangle(0.5, 0.5, width, height));

            Content = layout;

            var menu = new MenuGenerator().Configure()
                .AddMenuItem("back", "Back", Xamarin.Forms.ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(false); })
                .AddMenuItem("select", "Select", Xamarin.Forms.ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(true); });
            menu.GenerateToolbarItemsForPage(this);
            menu.SetBinding(MenuGenerator.InvalidateCommandProperty, new Xamarin.Forms.Binding(nameof(DisplaySpeciesTabbedPageViewModel.InvalidateMenuCommand), Xamarin.Forms.BindingMode.OneWayToSource, source: viewModel));

            App.OnFlipHandler += OnFlipHandler;
        }

        private void OnFlipHandler(bool isPortrait)
        {
            double width;
            double height;
            var mainDisplayInfo = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo;
            if (mainDisplayInfo.Height < mainDisplayInfo.Width)
            {
                width = 1.0f;
                height = mainDisplayInfo.Width / mainDisplayInfo.Height;
                map.WidthRequest = mainDisplayInfo.Width;
            }
            else
            {
                height = 1.0;
                width = mainDisplayInfo.Height / mainDisplayInfo.Width;
                map.HeightRequest = mainDisplayInfo.Height;
            }

            AbsoluteLayout.SetLayoutBounds(map, new Rectangle(0.5, 0.5, width, height));
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
