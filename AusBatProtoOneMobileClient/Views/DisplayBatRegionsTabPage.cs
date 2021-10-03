using AusBatProtoOneMobileClient.ViewModels;
using AusBatProtoOneMobileClient.Views.Components;
using FFImageLoading.Forms;
using Mobile.Helpers;
using Mobile.ViewModels;
using Xamarin.Essentials;
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
#if false
            var mainDisplayInfo = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo;
            var map = new Map();
            map.WidthRequest = mainDisplayInfo.Width;
            map.SetBinding(Map.SelectedItemsProperty, new Binding(nameof(DisplayBatTabbedPageViewModel.SelectedMapItems), BindingMode.TwoWay));
#else
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

            var image = new CachedImage
            {
                Aspect = Aspect.AspectFit,
            };
            image.WidthRequest = mainDisplayInfo.Width;
            image.SetBinding(CachedImage.SourceProperty, new Binding(nameof(DisplayBatTabbedPageViewModel.DistributionMapImage), BindingMode.OneWay));
#endif
            Title = "Distribution";
            BackgroundColor = Color.Black;

            Content = image;
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
