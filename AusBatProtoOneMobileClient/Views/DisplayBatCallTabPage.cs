using AusBatProtoOneMobileClient.Data;
using AusBatProtoOneMobileClient.Models;
using AusBatProtoOneMobileClient.ViewModels;
using FFImageLoading.Forms;
using Mobile.Helpers;
using Mobile.ViewModels;
using System;
using System.Globalization;
using Xamarin.Essentials;
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

            var listView = new ListView
            {
                SelectionMode = ListViewSelectionMode.Single,
                BackgroundColor = Color.Transparent,
                SeparatorColor = Color.Black
            };
            listView.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(DisplayBatTabbedPageViewModel.CallDisplayItems), BindingMode.TwoWay));
            listView.SetBinding(ListView.SelectedItemProperty, new Binding(nameof(DisplayBatTabbedPageViewModel.SelectedCallDisplayItem), BindingMode.TwoWay));
            listView.ItemTemplate = new DataTemplate(typeof(CallListViewDataTemplate));

            var layout = new StackLayout { VerticalOptions = LayoutOptions.Center, Children = { listView } };

            Title = "Calls";
            BackgroundColor = Color.Black;

            var centeredLayout = new AbsoluteLayout
            {
                Children = { layout, activityIndicator },
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = 5
            };
            AbsoluteLayout.SetLayoutFlags(layout, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(layout, new Rectangle(0, 0, 1, 1));
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

    public class CallListViewDataTemplate : ViewCell
    {
        public CallListViewDataTemplate()
        {

            var startStopPlaybackButton = new ImageButton() { BackgroundColor = Color.Transparent };
            startStopPlaybackButton.SetBinding(ImageButton.CommandProperty, new Binding(nameof(DisplayBatTabbedPageViewModel.CallDisplayItem.OnStartStopClicked), BindingMode.OneWay));
            startStopPlaybackButton.SetBinding(ImageButton.SourceProperty, new Binding(nameof(DisplayBatTabbedPageViewModel.CallDisplayItem.IsPlaying), BindingMode.OneWay, new IsPlayingToImageConverter()));

            View = startStopPlaybackButton;

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
