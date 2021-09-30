using AusBatProtoOneMobileClient.ViewModels;
using AusBatProtoOneMobileClient.Views.Components;
using Mobile.Helpers;
using Mobile.ViewModels;
using Xamarin.Forms;


namespace AusBatProtoOneMobileClient
{
    public class SelectBatRegionsPage : ContentPageBase
    {
        SelectBatRegionsPageViewModel viewModel;

        public SelectBatRegionsPage(SelectBatRegionsPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;
            var mainDisplayInfo = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo;
            var map = new Map { IsSelectable = true };
            map.WidthRequest = mainDisplayInfo.Width;
            map.SetBinding(Map.SelectedItemsProperty, new Binding(nameof(SelectBatRegionsPageViewModel.SelectedMapRegions), BindingMode.TwoWay));

            Title = "Regions";
            BackgroundColor = Color.Black;

            Content = map;

            var menu = new MenuGenerator().Configure()
                .AddMenuItem("back", "Back", Xamarin.Forms.ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(false); })
                .AddMenuItem("select", "Select", Xamarin.Forms.ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(true); });
            menu.GenerateToolbarItemsForPage(this);
            menu.SetBinding(MenuGenerator.InvalidateCommandProperty, new Xamarin.Forms.Binding(nameof(DisplayBatTabbedPageViewModel.InvalidateMenuCommand), Xamarin.Forms.BindingMode.OneWayToSource, source: viewModel));
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
