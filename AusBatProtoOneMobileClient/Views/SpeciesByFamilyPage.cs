using AusBatProtoOneMobileClient.Models;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using Mobile.Helpers;
using Mobile.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using static DocGenOneMobileClient.Views.SpeciesByFamilyPageViewModel;

namespace DocGenOneMobileClient.Views
{
    public class SpeciesByFamilyPage : ContentPageBase
    {
        bool isFirstAppearance = true; 
        SpeciesByFamilyPageViewModel viewModel;
        MenuGenerator menu;

        public SpeciesByFamilyPage(SpeciesByFamilyPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            var listView = new ListView {
                SelectionMode = ListViewSelectionMode.Single,
                HasUnevenRows = true,
                IsGroupingEnabled = true,
                GroupDisplayBinding = new Binding (nameof(GroupedSpeciesDisplayItem.FamilyName)),
                BackgroundColor = Color.Transparent,
                SeparatorColor = Constants.APP_COLOUR
            };
            listView.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(SpeciesByFamilyPageViewModel.FamilyGroupDisplayItems), BindingMode.TwoWay));
            listView.SetBinding(ListView.SelectedItemProperty, new Binding(nameof(SpeciesByFamilyPageViewModel.SelectedItem), BindingMode.TwoWay));
            listView.ItemTapped += (s, e) => { viewModel.OnSelectPressed.Execute(true); };
            listView.ItemTemplate = new DataTemplate(typeof(ListViewDataTemplate));
            listView.GroupHeaderTemplate = new DataTemplate(typeof(ListViewGroupTemplate));

            var listViewLayout = new ScrollView
            {
                Content = listView,
                Orientation = ScrollOrientation.Vertical
            };

            var backgroundImage = new Image { Aspect = Aspect.AspectFill, Source = Constants.BACKGROUND_IMAGE };

            var finalLayout = new AbsoluteLayout
            {
                Children = { backgroundImage, listViewLayout, activityIndicator },
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            AbsoluteLayout.SetLayoutFlags(backgroundImage, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(backgroundImage, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(listViewLayout, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(listViewLayout, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(activityIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(activityIndicator, new Rectangle(0.5, .5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            Title = "Species by Family";
            BackgroundImageSource = Constants.BACKGROUND_IMAGE;
            Content = finalLayout;

            menu = new MenuGenerator().Configure()
                .AddMenuItem("back", "Back", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(null); });
 
            menu.GenerateToolbarItemsForPage(this);
            menu.SetBinding(MenuGenerator.InvalidateCommandProperty, new Binding(nameof(SpeciesByFamilyPageViewModel.InvalidateMenu), BindingMode.OneWayToSource, source: viewModel));

            
        }


        public class ListViewDataTemplate: ViewCell
        {

            public ListViewDataTemplate()
            {

                var speciesNameLabel = new Label {  VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                speciesNameLabel.SetBinding(Label.TextProperty, new Binding(nameof(SpeciesDisplayItem.SpeciesName), BindingMode.TwoWay));

                var friendlyNameLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                friendlyNameLabel.SetBinding(Label.TextProperty, new Binding(nameof(SpeciesDisplayItem.FriendlyName), BindingMode.TwoWay));

                var image = new CachedImage
                {
                    Aspect = Aspect.AspectFit
                };
                image.Transformations.Add(new CircleTransformation());
                image.SetBinding(CachedImage.SourceProperty, new Binding(nameof(SpeciesDisplayItem.ImageSource), BindingMode.OneWay));

                var grid = new Grid() { Margin = 5 };
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.Children.Add(speciesNameLabel, 0, 0);
                grid.Children.Add(friendlyNameLabel, 0, 1);
                grid.Children.Add(image, 1, 0);
                Grid.SetRowSpan(image, 2);

                View = grid;

            }

        }

        public class ListViewGroupTemplate : ViewCell
        {

            public ListViewGroupTemplate()
            {

                var speciesNameLabel = new Label { 
                    VerticalTextAlignment = TextAlignment.Center, 
                    TextColor = Constants.APP_COLOUR,
                    Margin = 5
                };
                speciesNameLabel.SetBinding(Label.TextProperty, new Binding(nameof(GroupedSpeciesDisplayItem.FamilyName), BindingMode.TwoWay));

                var color = (DeviceInfo.Platform != DevicePlatform.iOS) ? Color.Transparent : Color.Black;
                View = new StackLayout { Orientation = StackOrientation.Horizontal, Children = { speciesNameLabel }, BackgroundColor = color };

            }

        }
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
