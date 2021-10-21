using AusBatProtoOneMobileClient.Data;
using AusBatProtoOneMobileClient.Models;
using AusBatProtoOneMobileClient.Views.Components;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using Mobile.Helpers;
using Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace DocGenOneMobileClient.Views
{
    public class FamilyKeyResultPage : ContentPageBase
    {
        FamilyKeyResultPageViewModel viewModel;
        public FamilyKeyResultPage(FamilyKeyResultPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            var keyTreeNodesListView = new ListView
            {
                SelectionMode = ListViewSelectionMode.Single,
                SeparatorColor = Constants.APP_COLOUR,
                HasUnevenRows = true,
                BackgroundColor = Color.Transparent
            };
            keyTreeNodesListView.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(FamilyKeyResultPageViewModel.DisplayItems), BindingMode.TwoWay));
            keyTreeNodesListView.SetBinding(ListView.SelectedItemProperty, new Binding(nameof(FamilyKeyResultPageViewModel.SelectedDisplayItem), BindingMode.TwoWay));
            keyTreeNodesListView.ItemTemplate = new TemplateSelector();
            keyTreeNodesListView.ItemTapped += (s, e) => {
                viewModel.OnSelectPressed.Execute(null);
            };

            var backgroundImage = new Image { Aspect = Aspect.AspectFill, Source = Constants.BACKGROUND_IMAGE };

            var finalLayout = new AbsoluteLayout
            {
                Children = { backgroundImage, keyTreeNodesListView, activityIndicator },
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = 5
            };
            AbsoluteLayout.SetLayoutFlags(backgroundImage, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(backgroundImage, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(keyTreeNodesListView, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(keyTreeNodesListView, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(activityIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(activityIndicator, new Rectangle(0.5, .5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            Title = "Select to filter";
            BackgroundImageSource = Constants.BACKGROUND_IMAGE;
            Content = finalLayout;
            var menu = new MenuGenerator().Configure()
                .AddMenuItem("back", "Back", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(null); })
                .AddMenuItem("reset", "Reset", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnResetFiltersClicked.Execute(null); });


            menu.GenerateToolbarItemsForPage(this);
            menu.SetBinding(MenuGenerator.InvalidateCommandProperty, new Binding(nameof(FamilyKeyResultPageViewModel.InvalidateMenu), BindingMode.OneWayToSource, source: viewModel));

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



        internal class TemplateSelector : DataTemplateSelector
        {
            DataTemplate leafNodeDisplayItem;
            DataTemplate generalNodeDisplayItem;
            DataTemplate noticeDisplayItem;

            public TemplateSelector()
            {
                leafNodeDisplayItem = new DataTemplate(() => {
                    var speciesNameLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    speciesNameLabel.SetBinding(Label.TextProperty, new Binding(nameof(FamilyKeyResultPageViewModel.LeafNodeDisplayItem.SpeciesName), BindingMode.TwoWay));

                    var commonNameLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    commonNameLabel.SetBinding(Label.TextProperty, new Binding(nameof(FamilyKeyResultPageViewModel.LeafNodeDisplayItem.CommonName), BindingMode.TwoWay));


                    var heightRequest = Device.GetNamedSize(NamedSize.Large, typeof(Label)) * 2;
                    var image = new CachedImage
                    {
                        Aspect = Aspect.AspectFit,
                        HeightRequest = heightRequest,
                        ErrorPlaceholder = "bat.png"
                    };
                    image.Transformations.Add(new CircleTransformation());
                    image.SetBinding(CachedImage.SourceProperty, new Binding(nameof(FamilyKeyResultPageViewModel.LeafNodeDisplayItem.ImageSource), BindingMode.OneWay));

                    var grid = new Grid() { Margin = 5 };
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(speciesNameLabel, 0, 0);
                    grid.Children.Add(commonNameLabel, 0, 1);
                    grid.Children.Add(image, 2, 0);
                    Grid.SetColumnSpan(speciesNameLabel, 2);
                    Grid.SetColumnSpan(commonNameLabel, 2);
                    Grid.SetRowSpan(image, 2);

                    return new ViewCell { View = grid };
                });

                generalNodeDisplayItem = new DataTemplate(() => {
                    var descriptionLabel = new Label { 
                        VerticalTextAlignment = TextAlignment.Center, 
                        TextColor = Color.White 
                    };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(FamilyKeyResultPageViewModel.NodeDisplayItem.Name), BindingMode.TwoWay));

                    var grid = new Grid { Margin = 5 };
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(descriptionLabel, 0, 0);
                    Grid.SetColumnSpan(descriptionLabel, 3);

                    return new ViewCell { View = grid };
                });

                noticeDisplayItem = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(FamilyKeyResultPageViewModel.NoticeDisplayItem.Description), BindingMode.TwoWay));

                    var grid = new Grid { Margin = 5 };
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(descriptionLabel, 0, 0);
                    Grid.SetColumnSpan(descriptionLabel, 3);

                    return new ViewCell { View = grid };
                });
            }

            protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
            {
                if (item is FamilyKeyResultPageViewModel.LeafNodeDisplayItem)
                {
                    return leafNodeDisplayItem;
                }
                else if (item is FamilyKeyResultPageViewModel.NodeDisplayItem)
                {
                    return generalNodeDisplayItem;
                }
                else if (item is FamilyKeyResultPageViewModel.NoticeDisplayItem)
                {
                    return noticeDisplayItem;
                }
                else
                {
                    throw new ApplicationException("Unidentified template type");
                }
            }
        }
    }
}
