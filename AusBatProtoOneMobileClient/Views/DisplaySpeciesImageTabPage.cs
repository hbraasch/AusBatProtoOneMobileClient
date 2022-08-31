using AusBatProtoOneMobileClient.Models;
using AusBatProtoOneMobileClient.ViewModels;
using AusBatProtoOneMobileClient.Views.Components;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using Mobile.Helpers;
using Mobile.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
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
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Loop = false,
                ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Horizontal)
                {
                    ItemSpacing = 5
                }
            };
            carouselView.SetBinding(CarouselView.ItemsSourceProperty, new Binding(nameof(DisplaySpeciesTabbedPageViewModel.ImageDataItems), BindingMode.TwoWay, source: viewModel));
            carouselView.ItemTemplate = new DataTemplate(() =>
            {

                var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

                var image = new ImageWithTap
                {
                    Aspect = Aspect.AspectFit,                   
                };

                image.WidthRequest = mainDisplayInfo.Width;
                image.SetBinding(ImageWithTap.SourceProperty, new Binding(nameof(ImageDataItem.ImageSource), BindingMode.OneWay));
                image.SetBinding(ImageWithTap.OnTappedProperty, new Binding(nameof(ImageDataItem.OnImageTapped), BindingMode.TwoWay));

                return image; 
            });

            var indicatorView = new IndicatorView
            {
                IndicatorColor = Color.LightGray,
                SelectedIndicatorColor = Constants.APP_COLOUR,
                HorizontalOptions = LayoutOptions.Center,
                IndicatorsShape = IndicatorShape.Circle,
                IndicatorSize = 20,
                Count = 5
            };
            indicatorView.SetBinding(IndicatorView.CountProperty, new Binding(nameof(DisplaySpeciesTabbedPageViewModel.ImageCount), BindingMode.TwoWay, source: viewModel, converter: new TestConverter()));
            carouselView.IndicatorView = indicatorView;

            var mainLayout = new StackLayout { Children = { carouselView, indicatorView }, Margin = 5 };

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

            NavigationPage.SetTitleView(this, new Label { Text = "Images", Style = Styles.TitleLabelStyle });
            BackgroundColor = Color.Black;
            Content = centeredLayout;

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

        private class TestConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return (int) value;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return (int)value;
            }
        }
    }
}
