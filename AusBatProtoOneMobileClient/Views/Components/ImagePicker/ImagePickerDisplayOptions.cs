using AusBatProtoOneMobileClient.Models;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static AusBatProtoOneMobileClient.ViewModels.ClassificationPageViewModel;
using static AusBatProtoOneMobileClient.Views.Components.ImagePicker;

namespace AusBatProtoOneMobileClient.Views.Components
{
    public class ImagePickerDisplayOptions : ContentPage
    {
        public class PickerWithImagesDisplayData
        {
            public string Description { get; set; }
            public string ImageSource { get; set; }
        }

        public ObservableCollection<PickerWithImagesDisplayData> DisplayItems { get; set; } = new ObservableCollection<PickerWithImagesDisplayData>();
        public PickerWithImagesDisplayData SelectedDisplayItem { get; set; }

        public ImagePickerDisplayOptions()
        {
            BindingContext = this;
            var listView = new ListView
            {
                SelectionMode = ListViewSelectionMode.Single,
                HasUnevenRows = true,
                BackgroundColor = Color.Transparent,
                SeparatorColor = Constants.APP_COLOUR
            };
            listView.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(DisplayItems), BindingMode.TwoWay));
            listView.SetBinding(ListView.SelectedItemProperty, new Binding(nameof(SelectedDisplayItem), BindingMode.TwoWay));
            listView.ItemSelected += async (s, e) => {
                await Navigation.PopAsync(); 
            };
            listView.ItemTemplate = new DataTemplate(typeof(ListViewTemplate));

            var frame = new Frame
            {
                BorderColor = Constants.APP_COLOUR,
                BackgroundColor = Color.Transparent,
                CornerRadius = 10,
                Content = listView
            };

            var layout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Children = { frame }
            };

            Title = "Select";
            BackgroundColor = Color.Black;
            Content = layout;
        }


        public class ListViewTemplate : ViewCell
        {

            public ListViewTemplate()
            {

                var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(PickerWithImagesDisplayData.Description), BindingMode.TwoWay));

                var heightRequest = Device.GetNamedSize(NamedSize.Large, typeof(Label)) * 4;
                var image = new CachedImage
                {
                    Aspect = Aspect.AspectFit,
                    ErrorPlaceholder = "bat.png",
                    HeightRequest =  heightRequest
                };
                image.SetBinding(CachedImage.SourceProperty, new Binding(nameof(PickerWithImagesDisplayData.ImageSource), BindingMode.OneWay));

                var grid = new Grid() { Margin = 5, BackgroundColor = Color.Transparent };
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                grid.Children.Add(descriptionLabel, 0, 0);
                grid.Children.Add(image, 1, 0);

                View = grid;

            }

        }

        TaskCompletionSource<object> tcs;
        public async Task<object> WaitUntilExecutionStops()
        {
            tcs = new TaskCompletionSource<object>();
            return await tcs.Task;
        }
        private void ExecutionStops()
        {
            tcs?.SetResult(null);
        }

        protected override bool OnBackButtonPressed()
        {
            return false;
        }

        protected override void OnDisappearing()
        {
            ExecutionStops();
            base.OnDisappearing();
        }
    }
}
