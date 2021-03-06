using AusBatProtoOneMobileClient.Models;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using Mobile.Helpers;
using Mobile.ViewModels;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using static DocGenOneMobileClient.Views.SpeciesByAreaPageViewModel;

namespace DocGenOneMobileClient.Views
{
    public class SpeciesByAreaPage : ContentPageBase
    {
        bool isFirstAppearance = true; 
        SpeciesByAreaPageViewModel viewModel;
        MenuGenerator menu;

        public SpeciesByAreaPage(SpeciesByAreaPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            var listView = new ListView {
                SelectionMode = ListViewSelectionMode.Single,
                HasUnevenRows = true,
                IsGroupingEnabled = true,
                BackgroundColor = Color.Transparent,
                SeparatorColor = Constants.APP_COLOUR,
                Margin = 5
            };
            listView.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(SpeciesByAreaPageViewModel.SpeciesGroupDisplayItems), BindingMode.TwoWay));
            listView.SetBinding(ListView.SelectedItemProperty, new Binding(nameof(SpeciesByAreaPageViewModel.SelectedItem), BindingMode.TwoWay));
            listView.ItemTapped += (s, e) => { viewModel.OnSelectMenuPressed.Execute(true); };
            listView.ItemTemplate = new SpeciesKeyDataTemplateSelector();
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

            NavigationPage.SetTitleView(this, new Xamarin.Forms.Label { Text = "Filter results", Style = Styles.TitleLabelStyle });
            Content = finalLayout;
            BackgroundImageSource = Constants.BACKGROUND_IMAGE;

            menu = new MenuGenerator().Configure()
                .AddMenuItem("home", "Home", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnHomeMenuPressed.Execute(null); })
                .AddMenuItem("back", "Back", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(null); });

            menu.SetVisibilityFactors(viewModel, "IsHomeEnabled")
                .ToShowMenuItem("home", true);
            menu.GenerateToolbarItemsForPage(this);
            menu.SetBinding(MenuGenerator.InvalidateCommandProperty, new Binding(nameof(SpeciesByAreaPageViewModel.InvalidateMenu), BindingMode.OneWayToSource, source: viewModel));

            
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

        internal class SpeciesKeyDataTemplateSelector : DataTemplateSelector
        {
            DataTemplate speciesTemplate;

            public SpeciesKeyDataTemplateSelector()
            {

                speciesTemplate = new DataTemplate(() => {
                    var speciesNameLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White, FontAttributes = FontAttributes.Italic };
                    speciesNameLabel.SetBinding(Label.TextProperty, new Binding(nameof(SpeciesByAreaPageViewModel.SpeciesDisplayItem.SpeciesName), BindingMode.TwoWay));

                    var speciesFriendlyNameLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Constants.COMMON_NAME_COLOUR };
                    speciesFriendlyNameLabel.SetBinding(Label.TextProperty, new Binding(nameof(SpeciesByAreaPageViewModel.SpeciesDisplayItem.FriendlyName), BindingMode.TwoWay));


                    var heightRequest = Device.GetNamedSize(NamedSize.Large, typeof(Label)) * 2;
                    var image = new CachedImage
                    {
                        Aspect = Aspect.AspectFit,
                        HeightRequest = heightRequest,
                        ErrorPlaceholder = "bat.png"
                    };
                    image.Transformations.Add(new CircleTransformation() { BorderHexColor = Constants.IMAGE_CIRCLE_COLOUR.ToHex(), BorderSize = Constants.IMAGE_CIRCLE_BORDER_SIZE });
                    image.SetBinding(CachedImage.SourceProperty, new Binding(nameof(SpeciesByAreaPageViewModel.SpeciesDisplayItem.ImageSource), BindingMode.OneWay));

                    var grid = new Grid() { Margin = new Thickness(15, 5, 5, 5)};
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(speciesNameLabel, 0, 0);
                    grid.Children.Add(speciesFriendlyNameLabel, 0, 1);
                    grid.Children.Add(image, 2, 0);
                    Grid.SetColumnSpan(speciesNameLabel, 2);
                    Grid.SetColumnSpan(speciesFriendlyNameLabel, 2);
                    Grid.SetRowSpan(image, 2);

                    return new ViewCell { View = grid };
                });

            }

            protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
            {
                if (item is SpeciesByAreaPageViewModel.SpeciesDisplayItem)
                {
                    return speciesTemplate;
                }
                else
                {
                    throw new ApplicationException("Unidentified template type");
                }
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
                speciesNameLabel.SetBinding(Label.TextProperty, new Binding(nameof(SpeciesByAreaPageViewModel.GroupedSpeciesDisplayItem.Alphabet), BindingMode.TwoWay));
                View = speciesNameLabel;

            }

        }
    }
}
