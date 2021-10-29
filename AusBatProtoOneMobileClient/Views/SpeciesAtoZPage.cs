using AusBatProtoOneMobileClient.Helpers;
using AusBatProtoOneMobileClient.Models;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using Mobile.Helpers;
using Mobile.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using static DocGenOneMobileClient.Views.SpeciesAtoZPageViewModel;

namespace DocGenOneMobileClient.Views
{
    public class SpeciesAtoZPage : ContentPageBase
    {
        bool isFirstAppearance = true; 
        SpeciesAtoZPageViewModel viewModel;
        MenuGenerator menu;

        public SpeciesAtoZPage(SpeciesAtoZPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            var listView = new ListView() {
                SelectionMode = ListViewSelectionMode.Single,
                HasUnevenRows = true,
                IsGroupingEnabled = true,
                GroupDisplayBinding = new Binding (nameof(GroupedSpeciesDisplayItem.Alphabet)),
                BackgroundColor = Color.Transparent,
                SeparatorColor = Constants.APP_COLOUR,
                Margin = 5
            };
            listView.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(SpeciesAtoZPageViewModel.SpeciesGroupDisplayItems), BindingMode.TwoWay));
            listView.SetBinding(ListView.SelectedItemProperty, new Binding(nameof(SpeciesAtoZPageViewModel.SelectedItem), BindingMode.TwoWay));
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

            NavigationPage.SetTitleView(this, new Xamarin.Forms.Label { Text = "Species", Style = Styles.TitleLabelStyle });
            Content = finalLayout;
            BackgroundImageSource = Constants.BACKGROUND_IMAGE;

            menu = new MenuGenerator().Configure()
                .AddMenuItem("back", "Back", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(null); });
 
            menu.GenerateToolbarItemsForPage(this);
            menu.SetBinding(MenuGenerator.InvalidateCommandProperty, new Binding(nameof(SpeciesAtoZPageViewModel.InvalidateMenu), BindingMode.OneWayToSource, source: viewModel));

            
        }


        public class ListViewDataTemplate: ViewCell
        {

            public ListViewDataTemplate()
            {

                var speciesNameLabel = new Label {  VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White , FontAttributes = FontAttributes.Italic};
                speciesNameLabel.SetBinding(Label.TextProperty, new Binding(nameof(SpeciesAtoZPageViewModel.SpeciesDisplayItem.SpeciesName), BindingMode.TwoWay));

                var friendlyNameLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Constants.COMMON_NAME_COLOUR };
                friendlyNameLabel.SetBinding(Label.TextProperty, new Binding(nameof(SpeciesAtoZPageViewModel.SpeciesDisplayItem.FriendlyName), BindingMode.TwoWay));

                var image = new CachedImage
                {
                    Aspect = Aspect.AspectFit
                };
                image.Transformations.Add(new CircleTransformation());
                image.SetBinding(CachedImage.SourceProperty, new Binding(nameof(SpeciesAtoZPageViewModel.SpeciesDisplayItem.ImageSource), BindingMode.OneWay));

                var grid = new Grid() { Margin = new Thickness(15, 5, 5, 5) };
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
                var color = (DeviceInfo.Platform != DevicePlatform.iOS) ? Color.Transparent : Color.Black;
                var speciesNameLabel = new Label { 
                    VerticalTextAlignment = TextAlignment.Center, 
                    TextColor = Constants.APP_COLOUR, 
                    FontAttributes = FontAttributes.Bold,
                    BackgroundColor = color
                };
                speciesNameLabel.SetBinding(Label.TextProperty, new Binding(nameof(SpeciesAtoZPageViewModel.GroupedSpeciesDisplayItem.Alphabet), BindingMode.TwoWay));
                View = speciesNameLabel;

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
