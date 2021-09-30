using AusBatProtoOneMobileClient.Models;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using Mobile.Helpers;
using Mobile.ViewModels;
using System;
using Xamarin.Forms;
using static DocGenOneMobileClient.Views.GenusKeyPageViewModel;

namespace DocGenOneMobileClient.Views
{
    public class GenusKeyPage : ContentPageBase
    {
        bool isFirstAppearance = true; 
        GenusKeyPageViewModel viewModel;
        MenuGenerator menu;
        ImageButton actionButton;

        public GenusKeyPage(GenusKeyPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            var listView = new ListView {
                SelectionMode = ListViewSelectionMode.Single,
                HasUnevenRows = true,
                BackgroundColor = Color.Transparent,
                SeparatorColor = Constants.APP_COLOUR
            };
            listView.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(GenusKeyPageViewModel.DisplayItems), BindingMode.TwoWay));
            listView.SetBinding(ListView.SelectedItemProperty, new Binding(nameof(GenusKeyPageViewModel.SelectedItem), BindingMode.TwoWay));
            listView.ItemTapped += (s, e) => {  };
            listView.ItemTemplate = new GenusKeyDataTemplateSelector();

            actionButton = new ImageButton { Source = "ic_select.png", BackgroundColor = Color.Transparent };
            actionButton.Clicked += (s,e) => { viewModel.OnSelectMenuPressed.Execute(true); };
            actionButton.SetBinding(ImageButton.IsVisibleProperty, new Binding(nameof(GenusKeyPageViewModel.IsSelected), BindingMode.TwoWay));

            var listViewLayout = new ScrollView
            {
                Content = listView,
                Orientation = ScrollOrientation.Vertical
            };

            var finalLayout = new AbsoluteLayout
            {
                Children = { listViewLayout, actionButton, activityIndicator },
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = 5
            };
            AbsoluteLayout.SetLayoutFlags(listViewLayout, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(listViewLayout, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(actionButton, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(actionButton, new Rectangle(0.95, .95, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(activityIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(activityIndicator, new Rectangle(0.5, .5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            Title = "Keys to Genera/Species";

            Content = finalLayout;
            BackgroundImageSource = Constants.BACKGROUND_IMAGE;

            menu = new MenuGenerator().Configure()
                .AddMenuItem("back", "Back", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(null); });
 
            menu.GenerateToolbarItemsForPage(this);
            menu.SetBinding(MenuGenerator.InvalidateCommandProperty, new Binding(nameof(GenusKeyPageViewModel.InvalidateMenu), BindingMode.OneWayToSource, source: viewModel));

            
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

        internal class GenusKeyDataTemplateSelector : DataTemplateSelector
        {
            DataTemplate regionTemplate;
            DataTemplate genusTemplate;
            DataTemplate speciesTemplate;
            DataTemplate barTemplate;

            public GenusKeyDataTemplateSelector()
            {
                regionTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(GenusKeyPageViewModel.MapRegionsDisplayItem.Description), BindingMode.TwoWay));

                    var selectButton = new Button
                    {
                        Text = "Regions",
                        Style = Styles.RoundedButtonStyle,
                        BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5)
                    };
                    selectButton.SetBinding(Button.CommandProperty, new Binding(nameof(SearchPageViewModel.MapRegionsDisplayItem.OnSearch), BindingMode.TwoWay));

                    var selectionAmountLabel = new Label { TextColor = Color.White, VerticalTextAlignment = TextAlignment.Center };
                    selectionAmountLabel.SetBinding(Label.TextProperty, new Binding(nameof(SearchPageViewModel.MapRegionsDisplayItem.SelectionAmount), BindingMode.TwoWay));

                    var grid = new Grid();
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(descriptionLabel, 0, 0);
                    grid.Children.Add(selectionAmountLabel, 1, 0);
                    grid.Children.Add(selectButton, 2, 0);

                    return new ViewCell { View = grid };

                });

                genusTemplate = new DataTemplate(() => {
                    var genusNameLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    genusNameLabel.SetBinding(Label.TextProperty, new Binding(nameof(GenusKeyPageViewModel.GenusDisplayItem.GenusName), BindingMode.TwoWay));

                   
                    var grid = new Grid() { Margin = 5 };
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                    grid.Children.Add(genusNameLabel, 0, 0);
                    Grid.SetColumnSpan(genusNameLabel, 3);

                    return new ViewCell { View = grid };
                });

                speciesTemplate = new DataTemplate(() => {
                    var speciesNameLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    speciesNameLabel.SetBinding(Label.TextProperty, new Binding(nameof(GenusKeyPageViewModel.SpeciesDisplayItem.SpeciesName), BindingMode.TwoWay));
                    var heightRequest = Device.GetNamedSize(NamedSize.Large, typeof(Label)) * 2;
                    var image = new CachedImage
                    {
                        Aspect = Aspect.AspectFit,
                        HeightRequest = heightRequest
                    };
                    image.Transformations.Add(new CircleTransformation());
                    image.SetBinding(CachedImage.SourceProperty, new Binding(nameof(GenusKeyPageViewModel.SpeciesDisplayItem.ImageSource), BindingMode.OneWay));

                    var grid = new Grid() { Margin = 5 };
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(speciesNameLabel, 0, 0);
                    grid.Children.Add(image, 2, 0);
                    Grid.SetColumnSpan(speciesNameLabel, 2);

                    return new ViewCell { View = grid };
                });

                barTemplate = new DataTemplate(() => {

                    var bar = new BoxView { BackgroundColor = Constants.APP_COLOUR, HeightRequest = 5, CornerRadius = 5 };

                    var grid = new Grid() { Margin = 5 };
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(bar, 0, 0);
                    Grid.SetColumnSpan(bar, 3);

                    return new ViewCell { View = grid };
                });
            }

            protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
            {
                if (item is GenusKeyPageViewModel.MapRegionsDisplayItem)
                {
                    return regionTemplate;
                }
                else if (item is GenusKeyPageViewModel.GenusDisplayItem)
                {
                    return genusTemplate;
                }
                else if (item is GenusKeyPageViewModel.SpeciesDisplayItem)
                {
                    return speciesTemplate;
                }
                else if (item is GenusKeyPageViewModel.BarDisplayItem)
                {
                    return barTemplate;
                }
                else
                {
                    throw new ApplicationException("Unidentified template type");
                }
            }
        }
    }
}
