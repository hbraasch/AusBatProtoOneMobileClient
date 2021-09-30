using AusBatProtoOneMobileClient.Models;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using Mobile.Helpers;
using Mobile.ViewModels;
using System;
using Xamarin.Forms;
using static DocGenOneMobileClient.Views.FamilyKeyPageViewModel;

namespace DocGenOneMobileClient.Views
{
    public class FamilyKeyPage : ContentPageBase
    {
        bool isFirstAppearance = true; 
        FamilyKeyPageViewModel viewModel;
        MenuGenerator menu;
        ImageButton actionButton;

        public FamilyKeyPage(FamilyKeyPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            var listView = new ListView {
                SelectionMode = ListViewSelectionMode.Single,
                HasUnevenRows = true,
                BackgroundColor = Color.Transparent,
                SeparatorColor = Constants.APP_COLOUR
            };
            listView.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(FamilyKeyPageViewModel.DisplayItems), BindingMode.TwoWay));
            listView.SetBinding(ListView.SelectedItemProperty, new Binding(nameof(FamilyKeyPageViewModel.SelectedItem), BindingMode.TwoWay));
            listView.ItemTapped += (s, e) => {  };
            listView.ItemTemplate = new FamilyKeyDataTemplateSelector();

            actionButton = new ImageButton { Source = "ic_select.png", BackgroundColor = Color.Transparent };
            actionButton.Clicked += (s,e) => { viewModel.OnSelectMenuPressed.Execute(true); };
            actionButton.SetBinding(ImageButton.IsVisibleProperty, new Binding(nameof(FamilyKeyPageViewModel.IsSelected), BindingMode.TwoWay));

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

            Title = "Species by Family";

            Content = finalLayout;
            BackgroundImageSource = Constants.BACKGROUND_IMAGE;

            menu = new MenuGenerator().Configure()
                .AddMenuItem("back", "Back", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(null); });
 
            menu.GenerateToolbarItemsForPage(this);
            menu.SetBinding(MenuGenerator.InvalidateCommandProperty, new Binding(nameof(FamilyKeyPageViewModel.InvalidateMenu), BindingMode.OneWayToSource, source: viewModel));

            
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

        internal class FamilyKeyDataTemplateSelector : DataTemplateSelector
        {
            DataTemplate regionTemplate;
            DataTemplate familyTemplate;

            public FamilyKeyDataTemplateSelector()
            {
                regionTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(FamilyKeyPageViewModel.MapRegionsDisplayItem.Description), BindingMode.TwoWay));

                    var selectButton = new Button
                    {
                        Text = "Select",
                        Style = Styles.RoundedButtonStyle,
                        BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5)
                    };
                    selectButton.SetBinding(Button.CommandProperty, new Binding(nameof(SearchPageViewModel.MapRegionsDisplayItem.OnSearch), BindingMode.TwoWay));

                    var selectionAmountLabel = new Label { TextColor = Color.White, VerticalTextAlignment = TextAlignment.Center };
                    selectionAmountLabel.SetBinding(Label.TextProperty, new Binding(nameof(SearchPageViewModel.MapRegionsDisplayItem.SelectionAmount), BindingMode.TwoWay));

                    var grid = new Grid();
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(descriptionLabel, 0, 0);
                    grid.Children.Add(selectionAmountLabel, 1, 0);
                    grid.Children.Add(selectButton, 2, 0);

                    return new ViewCell { View = grid };

                });

                familyTemplate = new DataTemplate(() => {
                    var familyNameLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    familyNameLabel.SetBinding(Label.TextProperty, new Binding(nameof(FamilyKeyPageViewModel.FamilyDisplayItem.FamilyName), BindingMode.TwoWay));

                    var image = new CachedImage
                    {
                        Aspect = Aspect.AspectFit
                    };
                    image.Transformations.Add(new CircleTransformation());
                    image.SetBinding(CachedImage.SourceProperty, new Binding(nameof(FamilyKeyPageViewModel.FamilyDisplayItem.ImageSource), BindingMode.OneWay));

                    var grid = new Grid() { Margin = 5 };
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(familyNameLabel, 0, 0);
                    grid.Children.Add(image, 1, 0);

                    return new ViewCell { View = grid };
                });
            }

            protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
            {
                if (item is FamilyKeyPageViewModel.MapRegionsDisplayItem)
                {
                    return regionTemplate;
                }
                else if (item is FamilyKeyPageViewModel.FamilyDisplayItem)
                {
                    return familyTemplate;
                }
                else
                {
                    throw new ApplicationException("Unidentified template type");
                }
            }
        }
    }
}
