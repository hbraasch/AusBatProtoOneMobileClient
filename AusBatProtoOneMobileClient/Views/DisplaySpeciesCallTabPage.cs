using AusBatProtoOneMobileClient.Data;
using AusBatProtoOneMobileClient.Models;
using AusBatProtoOneMobileClient.ViewModels;
using AusBatProtoOneMobileClient.Views.Components;
using FFImageLoading.Forms;
using Mobile.Helpers;
using Mobile.ViewModels;
using System;
using System.Globalization;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace AusBatProtoOneMobileClient
{
    public class DisplaySpeciesCallTabPage : ContentPageBase
    {
        DisplaySpeciesTabbedPageViewModel viewModel;
        public DisplaySpeciesCallTabPage(DisplaySpeciesTabbedPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            var carouselView = new CarouselView
            {
                EmptyView = "Nothing to display",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            carouselView.SetBinding(CarouselView.ItemsSourceProperty, new Binding(nameof(DisplaySpeciesTabbedPageViewModel.CallDisplayItems), BindingMode.TwoWay, source: viewModel));
            carouselView.SetBinding(CarouselView.TabIndexProperty, new Binding(nameof(DisplaySpeciesTabbedPageViewModel.SelectedCallDisplayItem), BindingMode.TwoWay, source: viewModel));
            carouselView.SetBinding(CarouselView.IsVisibleProperty, new Binding(nameof(DisplaySpeciesTabbedPageViewModel.HasCallImage), BindingMode.TwoWay, source: viewModel));
            carouselView.ItemTemplate = new DataTemplate(() =>
            {
                var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
                CachedImage image = new CachedImage
                {
                    Aspect = Aspect.AspectFit,
                };
                image.WidthRequest = mainDisplayInfo.Width;
                image.SetBinding(CachedImage.SourceProperty, new Binding(nameof(DisplaySpeciesTabbedPageViewModel.CallDataItem.ImageSource), BindingMode.OneWay));
                return image;
            });

            var indicatorView = new IndicatorView
            {
                IndicatorColor = Color.LightGray,
                HorizontalOptions = LayoutOptions.Center,
                IndicatorsShape = IndicatorShape.Square,
                IndicatorSize = 18
            };
            carouselView.IndicatorView = indicatorView;

            var vuMeter = new VuMeterView { HorizontalOptions = LayoutOptions.CenterAndExpand, WidthRequest = 500, Margin = 5 };
            vuMeter.SetBinding(VuMeterView.ValueProperty, new Binding(nameof(DisplaySpeciesTabbedPageViewModel.VuDecibelValue), source: viewModel));
            vuMeter.SetBinding(VuMeterView.IsIsDisplayingProperty, new Binding(nameof(DisplaySpeciesTabbedPageViewModel.HasCallAudio), source: viewModel));

            var startStopPlaybackButton = new ImageButton() { BackgroundColor = Color.Transparent };
            startStopPlaybackButton.Clicked += (s, e) => { viewModel.OnStartStopPlaybackPressed.Execute(null); };
            startStopPlaybackButton.SetBinding(ImageButton.SourceProperty, new Binding(nameof(DisplaySpeciesTabbedPageViewModel.IsPlaying), BindingMode.OneWay, new IsPlayingToImageConverter()));
            startStopPlaybackButton.SetBinding(ImageButton.IsVisibleProperty, new Binding(nameof(DisplaySpeciesTabbedPageViewModel.HasCallAudio), BindingMode.OneWay));

            var audioPlayGrid = new Grid() { HorizontalOptions = LayoutOptions.FillAndExpand, Padding = 5 };
            audioPlayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            audioPlayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            audioPlayGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            audioPlayGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            audioPlayGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            audioPlayGrid.Children.Add(vuMeter, 0, 0);
            audioPlayGrid.Children.Add(startStopPlaybackButton, 1, 1);
            Grid.SetColumnSpan(vuMeter, 3);

            var mainLayout = new StackLayout { Children = { carouselView, indicatorView, audioPlayGrid } };

            NavigationPage.SetTitleView(this, new Label { Text = "Calls", Style = Styles.TitleLabelStyle });
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
                return "audio_playback_play.png";
            }
            else
            {
                return "audio_playback_stop.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}