using AusBatProtoOneMobileClient.Models;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using Mobile.Helpers;
using Mobile.ViewModels;
using System;
using Xamarin.Forms;
using static DocGenOneMobileClient.Views.SightingsPageViewModel;

namespace DocGenOneMobileClient.Views
{
    public class SightingsPage : ContentPageBase
    {
        bool isFirstAppearance = true; 
        SightingsPageViewModel viewModel;
        MenuGenerator menu;

        public SightingsPage(SightingsPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            var listView = new ListView {
                SelectionMode = ListViewSelectionMode.None,
                HasUnevenRows = true,
                BackgroundColor = Color.Transparent,
                SeparatorVisibility = SeparatorVisibility.Default,
                SeparatorColor = Constants.APP_COLOUR
            };
            listView.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(SightingsPageViewModel.DisplayItems), BindingMode.TwoWay));
            listView.SetBinding(ListView.SelectedItemProperty, new Binding(nameof(SightingsPageViewModel.SelectedItem), BindingMode.TwoWay));
            listView.ItemTapped += (s, e) => {  };
            listView.ItemTemplate = new SightingDataTemplateSelector();

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
                Margin = 5
            };
            AbsoluteLayout.SetLayoutFlags(backgroundImage, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(backgroundImage, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(listViewLayout, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(listViewLayout, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(activityIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(activityIndicator, new Rectangle(0.5, .5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            NavigationPage.SetTitleView(this, new Xamarin.Forms.Label { Text = "Sightings", Style = Styles.TitleLabelStyle });
            Content = finalLayout;

            menu = new MenuGenerator().Configure()
                .AddMenuItem("home", "Home", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnHomeMenuPressed.Execute(null); })
                .AddMenuItem("back", "Back", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(null); })
                .AddMenuItem("mail", "Mail", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnMailMenuPressed.Execute(null); })
                .AddMenuItem("clear", "Clear all", ToolbarItemOrder.Secondary, (menuItem) => { viewModel.OnClearAllMenuPressed.Execute(null); });
            menu.SetVisibilityFactors(viewModel, "IsHomeEnabled")
                .ToShowMenuItem("home", true);
            menu.GenerateToolbarItemsForPage(this);
            menu.SetBinding(MenuGenerator.InvalidateCommandProperty, new Binding(nameof(SightingsPageViewModel.InvalidateMenu), BindingMode.OneWayToSource, source: viewModel));

            
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

        internal class SightingDataTemplateSelector : DataTemplateSelector
        {
            DataTemplate sightingTemplate;

            public SightingDataTemplateSelector()
            {

                sightingTemplate = new DataTemplate(() => {
                    var speciesNameLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White, FontAttributes =  FontAttributes.Italic };
                    speciesNameLabel.SetBinding(Label.TextProperty, new Binding(nameof(SightingsPageViewModel.DisplayItem.SpeciesName), BindingMode.TwoWay));

                    var speciesFriendlyNameLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Constants.COMMON_NAME_COLOUR };
                    speciesFriendlyNameLabel.SetBinding(Label.TextProperty, new Binding(nameof(SightingsPageViewModel.DisplayItem.SpeciesFriendlyName), BindingMode.TwoWay));

                    var latLonLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    latLonLabel.SetBinding(Label.TextProperty, new Binding(nameof(SightingsPageViewModel.DisplayItem.Location), BindingMode.TwoWay));

                    var timestampLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    timestampLabel.SetBinding(Label.TextProperty, new Binding(nameof(SightingsPageViewModel.DisplayItem.Timestamp), BindingMode.TwoWay));

                    var heightRequest = Device.GetNamedSize(NamedSize.Large, typeof(Label)) * 3;
                    var image = new CachedImage
                    {
                        Aspect = Aspect.AspectFit,
                        ErrorPlaceholder = "bat.png"
                    };
                    image.Transformations.Add(new CircleTransformation() { BorderHexColor = "C0C0C0", BorderSize = 7 });
                    image.SetBinding(CachedImage.SourceProperty, new Binding(nameof(SpeciesByAreaPageViewModel.SpeciesDisplayItem.ImageSource), BindingMode.OneWay));

                    var grid = new Grid() { Margin = 5};
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(speciesNameLabel, 0, 0);
                    grid.Children.Add(speciesFriendlyNameLabel, 0, 1);
                    grid.Children.Add(timestampLabel, 0, 2);
                    grid.Children.Add(latLonLabel, 0, 3);
                    grid.Children.Add(image, 2, 0);

                    Grid.SetColumnSpan(speciesNameLabel, 2);
                    Grid.SetColumnSpan(speciesFriendlyNameLabel, 2);
                    Grid.SetColumnSpan(timestampLabel, 2);
                    Grid.SetColumnSpan(latLonLabel, 2);
                    Grid.SetRowSpan(image, 4);

                    return new ViewCell { View = grid };
                });

            }

            protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
            {
                if (item is SightingsPageViewModel.DisplayItem)
                {
                    return sightingTemplate;
                }
                else
                {
                    throw new ApplicationException("Unidentified template type");
                }
            }
        }
    }
}
