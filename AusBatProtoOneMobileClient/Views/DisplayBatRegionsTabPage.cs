using AusBatProtoOneMobileClient.ViewModels;
using AusBatProtoOneMobileClient.Views.Components;
using Mobile.Helpers;
using Mobile.ViewModels;
using Xamarin.Forms;


namespace AusBatProtoOneMobileClient
{
    public class DisplayBatRegionsTabPage : ContentPageBase
    {
        DisplayBatTabbedPageViewModel viewModel;

        public DisplayBatRegionsTabPage(DisplayBatTabbedPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;
            var mainDisplayInfo = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo;
            var map = new Map();
            map.WidthRequest = mainDisplayInfo.Width;
            map.SetBinding(Map.SelectedItemsProperty, new Binding(nameof(DisplayBatTabbedPageViewModel.SelectedMapItems), BindingMode.TwoWay));

            Title = "Regions";
            BackgroundColor = Color.Black;

            Content = map;
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
