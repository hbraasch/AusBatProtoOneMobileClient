using AusBatProtoOneMobileClient.Data;
using AusBatProtoOneMobileClient.ViewModels;
using FFImageLoading.Forms;
using Mobile.Helpers;
using Mobile.ViewModels;
using System;
using System.Globalization;
using Xamarin.Forms;


namespace AusBatProtoOneMobileClient
{
    public class DisplayBatCallTabPage : ContentPageBase
    {
        DisplayBatTabbedPageViewModel viewModel;
        public DisplayBatCallTabPage(DisplayBatTabbedPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            var carouselView = new CarouselView
            {
                EmptyView = "Nothing to display",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            carouselView.SetBinding(CarouselView.ItemsSourceProperty, new Binding(nameof(DisplayBatTabbedPageViewModel.CallDataItems), BindingMode.TwoWay, source: viewModel));
            carouselView.SetBinding(CarouselView.TabIndexProperty, new Binding(nameof(DisplayBatTabbedPageViewModel.CallDataItemIndex), BindingMode.TwoWay, source: viewModel));
            carouselView.ItemTemplate = new DataTemplate(() =>
            {

                CachedImage image = new CachedImage
                {
                    Aspect = Aspect.AspectFit,
                };
                image.SetBinding(CachedImage.SourceProperty, new Binding(nameof(CallDataItem.CallImage), BindingMode.OneWay));
                return new StackLayout { Children = { image } };
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

            var startStopPlaybackButton = new ImageButton();
            startStopPlaybackButton.Clicked += (s, e) => { viewModel.OnStartStopPlaybackPressed.Execute(null); };
            startStopPlaybackButton.SetBinding(ImageButton.SourceProperty, new Binding(nameof(DisplayBatTabbedPageViewModel.IsPlaying), BindingMode.OneWay, new IsPlayingToImageConverter()));

            var mainLayout = new StackLayout { Children = { carouselView , indicatorView, startStopPlaybackButton } };

            Title = "Calls";
            BackgroundColor = Color.Black;

            var centeredLayout = new AbsoluteLayout
            {
                Children = { mainLayout, activityIndicator },
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = 5
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

    internal class IsPlayingToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isPlaying = (bool)value;
            if (!isPlaying)
            {
                return "ic_audio_playback_play.png";
            }
            else
            {
                return "ic_audio_playback_stop.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
