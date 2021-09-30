using AusBatProtoOneMobileClient.Data;
using AusBatProtoOneMobileClient.Models;
using Mobile.Helpers;
using Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace DocGenOneMobileClient.Views
{
    public class SearchCriteriaTabPage : ContentPageBase
    {

        public SearchCriteriaTabPage(SearchPageViewModel viewModel) : base(viewModel)
        {
            BindingContext = viewModel;

            var searchButton = new Button
            {
                Text = "Search",
                Style = Styles.RoundedButtonStyle,
                BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5)
            };
            searchButton.Clicked += (s, e) => { viewModel.OnSearchButtonPressed.Execute(true); };

            var criteriaListView = new ListView { SelectionMode = ListViewSelectionMode.None };
            criteriaListView.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(SearchPageViewModel.CriteriaDisplayItems), BindingMode.TwoWay));
            criteriaListView.SetBinding(ListView.SelectedItemProperty, new Binding(nameof(SearchPageViewModel.CriteriaSelectedItem), BindingMode.TwoWay));
            criteriaListView.ItemTemplate = new CriteriaDataTemplateSelector();


            var resultsListView = new ListView { SelectionMode = ListViewSelectionMode.Single };
            resultsListView.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(SearchPageViewModel.ResultsDisplayItems), BindingMode.TwoWay));
            resultsListView.SetBinding(ListView.SelectedItemProperty, new Binding(nameof(SearchPageViewModel.ResultsSelectedItem), BindingMode.TwoWay));
            resultsListView.ItemTapped += (s, e) => { viewModel.OnResultsListTapped.Execute(null); };
            resultsListView.Refreshing += (s, e) => { };
            resultsListView.ItemTemplate = new DataTemplate(() => {
                var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center };
                descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(SearchPageViewModel.ResultDisplayItem.Description), BindingMode.TwoWay));
                var descriptionLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Children = { descriptionLabel }, Margin = 5 };
                return new ViewCell { View = descriptionLayout };
            });

            var layout = new StackLayout { Children = { searchButton, criteriaListView } };

            var scrollView = new ScrollView
            {
                Content = layout,
                Orientation = ScrollOrientation.Vertical
            };

            var finalLayout = new AbsoluteLayout
            {
                Children = { scrollView, activityIndicator },
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = 5
            };
            AbsoluteLayout.SetLayoutFlags(scrollView, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(scrollView, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(activityIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(activityIndicator, new Rectangle(0.5, .5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            Title = "Criteria";
            BackgroundImageSource = Constants.BACKGROUND_IMAGE;
            Content = finalLayout;

        }




        internal class CriteriaDataTemplateSelector : DataTemplateSelector
        {
            DataTemplate regionTemplate;
            DataTemplate foreArmLengthTemplate;
            DataTemplate OuterCanineWidthTemplate;
            DataTemplate TailLengthTemplate;
            DataTemplate FootWithClawLengthTemplate;
            DataTemplate PenisLengthTemplate;
            DataTemplate HeadToBodyLengthTemplate;
            DataTemplate WeightTemplate;
            DataTemplate ThreeMetTemplate;
            DataTemplate IsGularPoachPresentTemplate;
            DataTemplate HasFleshyGenitalProjectionsTemplate;

            public CriteriaDataTemplateSelector()
            {
                regionTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White};
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(SearchPageViewModel.MapRegionsDisplayItem.Description), BindingMode.TwoWay));

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

                foreArmLengthTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(SearchPageViewModel.ForeArmLengthDisplayItem.Description), BindingMode.TwoWay));

                    var valueEntry = new Entry { BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5), Keyboard = Keyboard.Numeric };

                    valueEntry.Behaviors.Add(new Xamarin.CommunityToolkit.Behaviors.NumericValidationBehavior() { MinimumValue = 0 });
                    valueEntry.SetBinding(Entry.TextProperty, new Binding(nameof(SearchPageViewModel.ForeArmLengthDisplayItem.Value), BindingMode.TwoWay));

                    var grid = new Grid();
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(descriptionLabel, 0, 0);
                    Grid.SetColumnSpan(descriptionLabel, 2);
                    grid.Children.Add(valueEntry, 2, 0);


                    return new ViewCell { View = grid };
                });
                OuterCanineWidthTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(SearchPageViewModel.OuterCanineWidthDisplayItem.Description), BindingMode.TwoWay));

                    var valueEntry = new Entry { BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5), Keyboard = Keyboard.Numeric };
                    valueEntry.SetBinding(Entry.TextProperty, new Binding(nameof(SearchPageViewModel.OuterCanineWidthDisplayItem.Value), BindingMode.TwoWay));

                    var grid = new Grid();
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(descriptionLabel, 0, 0);
                    Grid.SetColumnSpan(descriptionLabel, 2);
                    grid.Children.Add(valueEntry, 2, 0);

                    return new ViewCell { View = grid };
                });
                TailLengthTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(SearchPageViewModel.TailLengthDisplayItem.Description), BindingMode.TwoWay));

                    var valueEntry = new Entry { BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5), Keyboard = Keyboard.Numeric };
                    valueEntry.SetBinding(Entry.TextProperty, new Binding(nameof(SearchPageViewModel.TailLengthDisplayItem.Value), BindingMode.TwoWay));

                    var grid = new Grid();
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(descriptionLabel, 0, 0);
                    Grid.SetColumnSpan(descriptionLabel, 2);
                    grid.Children.Add(valueEntry, 2, 0);

                    return new ViewCell { View = grid };
                });
                FootWithClawLengthTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(SearchPageViewModel.FootWithClawLengthDisplayItem.Description), BindingMode.TwoWay));

                    var valueEntry = new Entry { BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5), Keyboard = Keyboard.Numeric };
                    valueEntry.SetBinding(Entry.TextProperty, new Binding(nameof(SearchPageViewModel.FootWithClawLengthDisplayItem.Value), BindingMode.TwoWay));

                    var grid = new Grid();
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(descriptionLabel, 0, 0);
                    Grid.SetColumnSpan(descriptionLabel, 2);
                    grid.Children.Add(valueEntry, 2, 0);

                    return new ViewCell { View = grid };
                });
                PenisLengthTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(SearchPageViewModel.PenisLengthDisplayItem.Description), BindingMode.TwoWay));

                    var valueEntry = new Entry { BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5), Keyboard = Keyboard.Numeric };
                    valueEntry.SetBinding(Entry.TextProperty, new Binding(nameof(SearchPageViewModel.PenisLengthDisplayItem.Value), BindingMode.TwoWay));

                    var grid = new Grid();
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(descriptionLabel, 0, 0);
                    Grid.SetColumnSpan(descriptionLabel, 2);
                    grid.Children.Add(valueEntry, 2, 0);

                    return new ViewCell { View = grid };
                });
                HeadToBodyLengthTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(SearchPageViewModel.HeadToBodyLengthDisplayItem.Description), BindingMode.TwoWay));

                    var valueEntry = new Entry { BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5), Keyboard = Keyboard.Numeric };
                    valueEntry.SetBinding(Entry.TextProperty, new Binding(nameof(SearchPageViewModel.HeadToBodyLengthDisplayItem.Value), BindingMode.TwoWay));

                    var grid = new Grid();
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(descriptionLabel, 0, 0);
                    Grid.SetColumnSpan(descriptionLabel, 2);
                    grid.Children.Add(valueEntry, 2, 0);

                    return new ViewCell { View = grid };
                });
                WeightTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(SearchPageViewModel.WeightDisplayItem.Description), BindingMode.TwoWay));

                    var valueEntry = new Entry { BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5), Keyboard = Keyboard.Numeric };
                    valueEntry.SetBinding(Entry.TextProperty, new Binding(nameof(SearchPageViewModel.WeightDisplayItem.Value), BindingMode.TwoWay));

                    var grid = new Grid();
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(descriptionLabel, 0, 0);
                    Grid.SetColumnSpan(descriptionLabel, 2);
                    grid.Children.Add(valueEntry, 2, 0);

                    return new ViewCell { View = grid };
                });
                ThreeMetTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(SearchPageViewModel.ThreeMetDisplayItem.Description), BindingMode.TwoWay));

                    var valueEntry = new Entry { BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5), Keyboard = Keyboard.Numeric };
                    valueEntry.SetBinding(Entry.TextProperty, new Binding(nameof(SearchPageViewModel.ThreeMetDisplayItem.Value), BindingMode.TwoWay));

                    var grid = new Grid();
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(descriptionLabel, 0, 0);
                    Grid.SetColumnSpan(descriptionLabel, 2);
                    grid.Children.Add(valueEntry, 2, 0);

                    return new ViewCell { View = grid };
                });
                IsGularPoachPresentTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(SearchPageViewModel.IsGularPoachPresentDisplayItem.Description), BindingMode.TwoWay));

                    var valuePicker = new Picker { TextColor = Color.White, VerticalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.FillAndExpand, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5) };
                    valuePicker.SetBinding(Picker.ItemsSourceProperty, new Binding(nameof(SearchPageViewModel.IsGularPoachPresentDisplayItem.Values), BindingMode.OneWay));
                    valuePicker.SetBinding(Picker.SelectedItemProperty, new Binding(nameof(SearchPageViewModel.IsGularPoachPresentDisplayItem.Value), BindingMode.TwoWay));

                    var grid = new Grid();
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(descriptionLabel, 0, 0);
                    Grid.SetColumnSpan(descriptionLabel, 2);
                    grid.Children.Add(valuePicker, 2, 0);

                    return new ViewCell { View = grid };
                });
                HasFleshyGenitalProjectionsTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(SearchPageViewModel.HasFleshyGenitalProjectionsDisplayItem.Description), BindingMode.TwoWay));

                    var valuePicker = new Picker { TextColor = Color.White, VerticalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.FillAndExpand, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5) };
                    valuePicker.SetBinding(Picker.ItemsSourceProperty, new Binding(nameof(SearchPageViewModel.HasFleshyGenitalProjectionsDisplayItem.Values), BindingMode.OneWay));
                    valuePicker.SetBinding(Picker.SelectedItemProperty, new Binding(nameof(SearchPageViewModel.HasFleshyGenitalProjectionsDisplayItem.Value), BindingMode.TwoWay));

                    var grid = new Grid();
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(descriptionLabel, 0, 0);
                    Grid.SetColumnSpan(descriptionLabel, 2);
                    grid.Children.Add(valuePicker, 2, 0);

                    return new ViewCell { View = grid };
                });
            }

            protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
            {
                if (item is SearchPageViewModel.MapRegionsDisplayItem)
                {
                    return regionTemplate;
                }
                else if (item is SearchPageViewModel.ForeArmLengthDisplayItem)
                {
                    return foreArmLengthTemplate;
                }
                else if (item is SearchPageViewModel.OuterCanineWidthDisplayItem)
                {
                    return OuterCanineWidthTemplate;
                }
                else if (item is SearchPageViewModel.TailLengthDisplayItem)
                {
                    return TailLengthTemplate;
                }
                else if (item is SearchPageViewModel.FootWithClawLengthDisplayItem)
                {
                    return FootWithClawLengthTemplate;
                }
                else if (item is SearchPageViewModel.PenisLengthDisplayItem)
                {
                    return PenisLengthTemplate;
                }
                else if (item is SearchPageViewModel.HeadToBodyLengthDisplayItem)
                {
                    return HeadToBodyLengthTemplate;
                }
                else if (item is SearchPageViewModel.WeightDisplayItem)
                {
                    return WeightTemplate;
                }
                else if (item is SearchPageViewModel.ThreeMetDisplayItem)
                {
                    return ThreeMetTemplate;
                }
                else if (item is SearchPageViewModel.IsGularPoachPresentDisplayItem)
                {
                    return IsGularPoachPresentTemplate;
                }
                else if (item is SearchPageViewModel.HasFleshyGenitalProjectionsDisplayItem)
                {
                    return HasFleshyGenitalProjectionsTemplate;
                }
                else
                {
                    throw new ApplicationException("Unidentified template type");
                }
            }
        }


    }


}
