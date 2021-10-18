using AusBatProtoOneMobileClient.Models;
using AusBatProtoOneMobileClient.ViewModels;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using Mobile.Helpers;
using Mobile.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using static AusBatProtoOneMobileClient.ViewModels.DisplaySpeciesTabbedPageViewModel;

namespace AusBatProtoOneMobileClient
{
    public class DisplaySpeciesImageTabPage : ContentPageBase
    {
        DisplaySpeciesTabbedPageViewModel viewModel;
        public DisplaySpeciesImageTabPage(DisplaySpeciesTabbedPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            var carouselView = new CarouselView { 
                EmptyView = "Nothing to display",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            carouselView.SetBinding(CarouselView.ItemsSourceProperty, new Binding(nameof(DisplaySpeciesTabbedPageViewModel.ImageDataItems), BindingMode.TwoWay, source: viewModel));
            carouselView.ItemTemplate = new DataTemplate(() =>
            {

                var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

                var image = new CachedImage
                {
                    Aspect = Aspect.AspectFit,
                };
                image.WidthRequest = mainDisplayInfo.Width;
                image.SetBinding(CachedImage.SourceProperty, new Binding(nameof(ImageDataItem.ImageSource), BindingMode.OneWay));

                return image; 
            });

            var indicatorView = new IndicatorView
            {
                IndicatorColor = Color.LightGray,
                SelectedIndicatorColor = Color.DarkGray,
                HorizontalOptions = LayoutOptions.Center,
                IndicatorsShape = IndicatorShape.Square,
                IndicatorSize = 18
            };
            carouselView.IndicatorView = indicatorView;

            var mainLayout = new StackLayout { Children = { carouselView, indicatorView } };

            Title = "Images";
            BackgroundColor = Color.Black;

            var centeredLayout = new AbsoluteLayout
            {
                Children = { mainLayout, activityIndicator },
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            AbsoluteLayout.SetLayoutFlags(mainLayout, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(mainLayout, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(activityIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(activityIndicator, new Rectangle(0.5, .5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));


            Content = centeredLayout;

        }

        bool isFirstAppearance = true;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (isFirstAppearance)
            {
                isFirstAppearance = false;
                // viewModel.OnDisplayBatImageFirstAppearance.Execute(null);
            }
            else
            {
                // viewModel.OnDisplayBatImageSubsequentAppearance.Execute(null);
            }
        }
    }
}
