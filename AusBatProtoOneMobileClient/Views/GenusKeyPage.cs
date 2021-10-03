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
    public class GenusKeyPage : ContentPageBase
    {
        GenusKeyPageViewModel viewModel;
        public GenusKeyPage(GenusKeyPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            var characteristicListView = new ListView { SelectionMode = ListViewSelectionMode.None };
            characteristicListView.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(GenusKeyPageViewModel.CharacteristicDisplayItems), BindingMode.TwoWay));
            characteristicListView.SetBinding(ListView.SelectedItemProperty, new Binding(nameof(GenusKeyPageViewModel.CharacteristicSelectedItem), BindingMode.TwoWay));
            characteristicListView.ItemTemplate = new CharacteristicDataTemplateSelector();


            var regionButton = new Button
            {
                Text = "Specify region",
                Style = Styles.RoundedButtonStyle,
                BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5)
            };
            regionButton.Clicked += (s, e) => { viewModel.OnSpecifyRegionClicked.Execute(null); };

            var filterNowButton = new Button
            {
                Text = "Filter now",
                Style = Styles.RoundedButtonStyle,
                BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5)
            };
            filterNowButton.Clicked += (s, e) => { viewModel.OnFilterNowClicked.Execute(null); };

            var layout = new StackLayout { Children = { characteristicListView, regionButton, filterNowButton } };

            var finalLayout = new AbsoluteLayout
            {
                Children = { layout, activityIndicator },
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = 5
            };
            AbsoluteLayout.SetLayoutFlags(layout, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(layout, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(activityIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(activityIndicator, new Rectangle(0.5, .5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            Title = "Filter";
            BackgroundImageSource = Constants.BACKGROUND_IMAGE;
            Content = finalLayout;

            var menu = new MenuGenerator().Configure()
                .AddMenuItem("home", "Home", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnHomeMenuPressed.Execute(null); })
                .AddMenuItem("back", "Back", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(null); })
                .AddMenuItem("clear", "Clear selections", ToolbarItemOrder.Secondary, (menuItem) => { viewModel.OnClearFiltersClicked.Execute(null); });

            menu.SetVisibilityFactors(viewModel, "IsHomeEnabled")
                .ToShowMenuItem("home", true);

            menu.GenerateToolbarItemsForPage(this);
            menu.SetBinding(MenuGenerator.InvalidateCommandProperty, new Binding(nameof(GenusKeyPageViewModel.InvalidateMenu), BindingMode.OneWayToSource, source: viewModel));

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


        internal class CharacteristicDataTemplateSelector : DataTemplateSelector
        {
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

            public CharacteristicDataTemplateSelector()
            {

                foreArmLengthTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(GenusKeyPageViewModel.ForeArmLengthDisplayItem.Description), BindingMode.TwoWay));

                    var valueEntry = new Entry { BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5), Keyboard = Keyboard.Numeric };

                    valueEntry.Behaviors.Add(new Xamarin.CommunityToolkit.Behaviors.NumericValidationBehavior() { MinimumValue = 0 });
                    valueEntry.SetBinding(Entry.TextProperty, new Binding(nameof(GenusKeyPageViewModel.ForeArmLengthDisplayItem.Value), BindingMode.TwoWay));

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
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(GenusKeyPageViewModel.OuterCanineWidthDisplayItem.Description), BindingMode.TwoWay));

                    var valueEntry = new Entry { BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5), Keyboard = Keyboard.Numeric };
                    valueEntry.SetBinding(Entry.TextProperty, new Binding(nameof(GenusKeyPageViewModel.OuterCanineWidthDisplayItem.Value), BindingMode.TwoWay));

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
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(GenusKeyPageViewModel.TailLengthDisplayItem.Description), BindingMode.TwoWay));

                    var valueEntry = new Entry { BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5), Keyboard = Keyboard.Numeric };
                    valueEntry.SetBinding(Entry.TextProperty, new Binding(nameof(GenusKeyPageViewModel.TailLengthDisplayItem.Value), BindingMode.TwoWay));

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
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(GenusKeyPageViewModel.FootWithClawLengthDisplayItem.Description), BindingMode.TwoWay));

                    var valueEntry = new Entry { BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5), Keyboard = Keyboard.Numeric };
                    valueEntry.SetBinding(Entry.TextProperty, new Binding(nameof(GenusKeyPageViewModel.FootWithClawLengthDisplayItem.Value), BindingMode.TwoWay));

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
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(GenusKeyPageViewModel.PenisLengthDisplayItem.Description), BindingMode.TwoWay));

                    var valueEntry = new Entry { BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5), Keyboard = Keyboard.Numeric };
                    valueEntry.SetBinding(Entry.TextProperty, new Binding(nameof(GenusKeyPageViewModel.PenisLengthDisplayItem.Value), BindingMode.TwoWay));

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
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(GenusKeyPageViewModel.HeadToBodyLengthDisplayItem.Description), BindingMode.TwoWay));

                    var valueEntry = new Entry { BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5), Keyboard = Keyboard.Numeric };
                    valueEntry.SetBinding(Entry.TextProperty, new Binding(nameof(GenusKeyPageViewModel.HeadToBodyLengthDisplayItem.Value), BindingMode.TwoWay));

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
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(GenusKeyPageViewModel.WeightDisplayItem.Description), BindingMode.TwoWay));

                    var valueEntry = new Entry { BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5), Keyboard = Keyboard.Numeric };
                    valueEntry.SetBinding(Entry.TextProperty, new Binding(nameof(GenusKeyPageViewModel.WeightDisplayItem.Value), BindingMode.TwoWay));

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
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(GenusKeyPageViewModel.ThreeMetDisplayItem.Description), BindingMode.TwoWay));

                    var valueEntry = new Entry { BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5), Keyboard = Keyboard.Numeric };
                    valueEntry.SetBinding(Entry.TextProperty, new Binding(nameof(GenusKeyPageViewModel.ThreeMetDisplayItem.Value), BindingMode.TwoWay));

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
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(GenusKeyPageViewModel.IsGularPoachPresentDisplayItem.Description), BindingMode.TwoWay));

                    var valuePicker = new Picker { TextColor = Color.White, VerticalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.FillAndExpand, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5) };
                    valuePicker.SetBinding(Picker.ItemsSourceProperty, new Binding(nameof(GenusKeyPageViewModel.IsGularPoachPresentDisplayItem.Values), BindingMode.OneWay));
                    valuePicker.SetBinding(Picker.SelectedItemProperty, new Binding(nameof(GenusKeyPageViewModel.IsGularPoachPresentDisplayItem.Value), BindingMode.TwoWay));

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
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(GenusKeyPageViewModel.HasFleshyGenitalProjectionsDisplayItem.Description), BindingMode.TwoWay));

                    var valuePicker = new Picker { TextColor = Color.White, VerticalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.FillAndExpand, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5) };
                    valuePicker.SetBinding(Picker.ItemsSourceProperty, new Binding(nameof(GenusKeyPageViewModel.HasFleshyGenitalProjectionsDisplayItem.Values), BindingMode.OneWay));
                    valuePicker.SetBinding(Picker.SelectedItemProperty, new Binding(nameof(GenusKeyPageViewModel.HasFleshyGenitalProjectionsDisplayItem.Value), BindingMode.TwoWay));

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
                if (item is GenusKeyPageViewModel.ForeArmLengthDisplayItem)
                {
                    return foreArmLengthTemplate;
                }
                else if (item is GenusKeyPageViewModel.OuterCanineWidthDisplayItem)
                {
                    return OuterCanineWidthTemplate;
                }
                else if (item is GenusKeyPageViewModel.TailLengthDisplayItem)
                {
                    return TailLengthTemplate;
                }
                else if (item is GenusKeyPageViewModel.FootWithClawLengthDisplayItem)
                {
                    return FootWithClawLengthTemplate;
                }
                else if (item is GenusKeyPageViewModel.PenisLengthDisplayItem)
                {
                    return PenisLengthTemplate;
                }
                else if (item is GenusKeyPageViewModel.HeadToBodyLengthDisplayItem)
                {
                    return HeadToBodyLengthTemplate;
                }
                else if (item is GenusKeyPageViewModel.WeightDisplayItem)
                {
                    return WeightTemplate;
                }
                else if (item is GenusKeyPageViewModel.ThreeMetDisplayItem)
                {
                    return ThreeMetTemplate;
                }
                else if (item is GenusKeyPageViewModel.IsGularPoachPresentDisplayItem)
                {
                    return IsGularPoachPresentTemplate;
                }
                else if (item is GenusKeyPageViewModel.HasFleshyGenitalProjectionsDisplayItem)
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
