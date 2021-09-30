using AusBatProtoOneMobileClient.Models;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using Mobile.Helpers;
using Mobile.ViewModels;
using System;
using Xamarin.Forms;
using static DocGenOneMobileClient.Views.SpeciesKeyPageViewModel;

namespace DocGenOneMobileClient.Views
{
    public class SpeciesKeyPage : ContentPageBase
    {
        bool isFirstAppearance = true; 
        SpeciesKeyPageViewModel viewModel;
        MenuGenerator menu;
        ImageButton actionButton;

        public SpeciesKeyPage(SpeciesKeyPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            var listView = new ListView {
                SelectionMode = ListViewSelectionMode.Single,
                HasUnevenRows = true,
                BackgroundColor = Color.Transparent,
                SeparatorColor = Constants.APP_COLOUR
            };
            listView.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(SpeciesKeyPageViewModel.DisplayItems), BindingMode.TwoWay));
            listView.SetBinding(ListView.SelectedItemProperty, new Binding(nameof(SpeciesKeyPageViewModel.SelectedItem), BindingMode.TwoWay));
            listView.ItemTapped += (s, e) => {  };
            listView.ItemTemplate = new SpeciesKeyDataTemplateSelector();

            actionButton = new ImageButton { Source = "ic_select.png", BackgroundColor = Color.Transparent };
            actionButton.Clicked += (s,e) => { viewModel.OnSelectMenuPressed.Execute(true); };
            actionButton.SetBinding(ImageButton.IsVisibleProperty, new Binding(nameof(SpeciesKeyPageViewModel.IsSelected), BindingMode.TwoWay));

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

            Title = "Species by Species";

            Content = finalLayout;
            BackgroundImageSource = Constants.BACKGROUND_IMAGE;

            menu = new MenuGenerator().Configure()
                .AddMenuItem("back", "Back", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(null); });
 
            menu.GenerateToolbarItemsForPage(this);
            menu.SetBinding(MenuGenerator.InvalidateCommandProperty, new Binding(nameof(SpeciesKeyPageViewModel.InvalidateMenu), BindingMode.OneWayToSource, source: viewModel));

            
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
                    var speciesNameLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    speciesNameLabel.SetBinding(Label.TextProperty, new Binding(nameof(SpeciesKeyPageViewModel.SpeciesDisplayItem.SpeciesName), BindingMode.TwoWay));
                    var heightRequest = Device.GetNamedSize(NamedSize.Large, typeof(Label)) * 2;
                    var image = new CachedImage
                    {
                        Aspect = Aspect.AspectFit,
                        HeightRequest = heightRequest
                    };
                    image.Transformations.Add(new CircleTransformation());
                    image.SetBinding(CachedImage.SourceProperty, new Binding(nameof(SpeciesKeyPageViewModel.SpeciesDisplayItem.ImageSource), BindingMode.OneWay));

                    var grid = new Grid() { Margin = 5 };
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(speciesNameLabel, 0, 0);
                    grid.Children.Add(image, 2, 0);
                    Grid.SetRowSpan(speciesNameLabel, 2);

                    return new ViewCell { View = grid };
                });

            }

            protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
            {
                if (item is SpeciesKeyPageViewModel.SpeciesDisplayItem)
                {
                    return speciesTemplate;
                }
                else
                {
                    throw new ApplicationException("Unidentified template type");
                }
            }
        }
    }
}
