using AusBatProtoOneMobileClient.Models;
using AusBatProtoOneMobileClient.ViewModels;
using FFImageLoading.Forms;
using Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AusBatProtoOneMobileClient.Views.Components
{
    public class ImagePickerDisplayOptions : ContentPage
    {
        CachedImage displayImage;
        Frame listViewFrame;
        public class PickerWithImagesDisplayData
        {
            public string Description { get; set; }
            public string ImageSource { get; set; }
            public Action<ImageSource> OnImageTapped { get; set; }
        }

        public ObservableCollection<PickerWithImagesDisplayData> DisplayItems { get; set; } = new ObservableCollection<PickerWithImagesDisplayData>();
        public PickerWithImagesDisplayData SelectedDisplayItem { get; set; }

        public ImagePickerDisplayOptions(ObservableCollection<PickerWithImagesDisplayData> data)
        {
            BindingContext = this;

            DisplayItems = data;
            foreach (var displayItem in DisplayItems)
            {
                displayItem.OnImageTapped = OnImageClicked;
            }

            var listView = new ListView
            {
                SelectionMode = ListViewSelectionMode.Single,
                HasUnevenRows = true,
                BackgroundColor = Color.Transparent,
                SeparatorColor = Constants.APP_COLOUR
            };
            listView.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(DisplayItems), BindingMode.TwoWay));
            listView.SetBinding(ListView.SelectedItemProperty, new Binding(nameof(SelectedDisplayItem), BindingMode.TwoWay));
            listView.ItemSelected +=  (s, e) => {
                ExecutionStops();
            };
            listView.ItemTemplate = new TemplateSelector(this);

            listViewFrame = new Frame
            {
                BorderColor = Constants.APP_COLOUR,
                BackgroundColor = Color.Transparent,
                CornerRadius = 10,
                Content = listView,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            displayImage = new CachedImage
            {
                Aspect = Aspect.AspectFit,
                ErrorPlaceholder = "bat.png"
            };
            displayImage.IsVisible = false;

            var overlayLayout = new AbsoluteLayout
            {
                Children = { listViewFrame, displayImage },
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = 5
            };
            AbsoluteLayout.SetLayoutFlags(listViewFrame, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(listViewFrame, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(displayImage, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(displayImage, new Rectangle(0, 0, 1, 1));

            Title = "Select";
            BackgroundColor = Color.Black;
            Content = overlayLayout;

            ToolbarItems.Add(new ToolbarItem { Text = "Back", Order = ToolbarItemOrder.Primary, 
                Command = new Command(() => 
                {
                    if (listViewFrame.IsVisible == true)
                    {
                        ExecutionStops(); 
                    }
                    else {
                        listViewFrame.IsVisible = true;
                        displayImage.IsVisible = false;
                    }
                }
            )});
        }

        public Action<ImageSource> OnImageClicked => async (imageSource) =>
        {
            try
            {
                displayImage.Source = imageSource;
                listViewFrame.IsVisible = false;
                displayImage.IsVisible = true;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Problem: ", ex.Message, "OK");
            }
        };

        internal class TemplateSelector : DataTemplateSelector
        {
            DataTemplate numericDisplayDataTemplate;

            public TemplateSelector(ImagePickerDisplayOptions contentPage)
            {
                numericDisplayDataTemplate = new DataTemplate(() => {

                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(PickerWithImagesDisplayData.Description), BindingMode.TwoWay));

                    var heightRequest = Device.GetNamedSize(NamedSize.Large, typeof(Label)) * 4;
                    var image = new CachedImageWithTap
                    {
                        Aspect = Aspect.AspectFit,
                        ErrorPlaceholder = "bat.png",
                        HeightRequest = heightRequest
                    };
                    image.SetBinding(CachedImageWithTap.SourceProperty, new Binding(nameof(PickerWithImagesDisplayData.ImageSource), BindingMode.OneWay));
                    image.SetBinding(CachedImageWithTap.OnTappedProperty, new Binding(nameof(PickerWithImagesDisplayData.OnImageTapped), BindingMode.TwoWay));

                    var grid = new Grid() { Margin = 5, BackgroundColor = Color.Transparent };
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                    grid.Children.Add(descriptionLabel, 0, 0);
                    grid.Children.Add(image, 1, 0);

                    return new ViewCell { View = grid };
                });

            }
            protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
            {
                return numericDisplayDataTemplate;
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
            Navigation.PopAsync();
            tcs?.SetResult(null);
        }

        protected override bool OnBackButtonPressed()
        {
            return false;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }

}
