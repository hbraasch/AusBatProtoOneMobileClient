using AusBatProtoOneMobileClient.Data;
using AusBatProtoOneMobileClient.Models;
using AusBatProtoOneMobileClient.Views.Components;
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
    public class FamilyKeyPage : ContentPageBase
    {
        FamilyKeyPageViewModel viewModel;
        public FamilyKeyPage(FamilyKeyPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            var characterListView = new ListView { SelectionMode = ListViewSelectionMode.None };
            characterListView.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(FamilyKeyPageViewModel.CharacterDisplayItems), BindingMode.TwoWay));
            characterListView.ItemTemplate = new TemplateSelector(this);
            characterListView.ItemSelected += (s,e) =>
            {
                viewModel.OnFilterClicked.Execute(null);
            };

            var resultLabel = new Label { FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            resultLabel.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(FamilyKeyPageViewModel.FilterResult), BindingMode.TwoWay));

            var viewResultsButton = new Button
            {
                Text = "View results",
                Style = Styles.RoundedButtonStyle,
                BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5)
            };
            viewResultsButton.Clicked += (s, e) => { viewModel.OnViewResultsClicked.Execute(null); };

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            grid.Children.Add(resultLabel, 0, 0);
            grid.Children.Add(viewResultsButton, 1, 0);


            var layout = new StackLayout { Children = {characterListView, grid } };

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
                .AddMenuItem("back", "Back", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(null); })
                .AddMenuItem("reset", "Reset", ToolbarItemOrder.Secondary, (menuItem) => { viewModel.OnResetFiltersClicked.Execute(null); });


            menu.GenerateToolbarItemsForPage(this);
            menu.SetBinding(MenuGenerator.InvalidateCommandProperty, new Binding(nameof(FamilyKeyPageViewModel.InvalidateMenu), BindingMode.OneWayToSource, source: viewModel));


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

            Page page;
            DataTemplate numericDisplayDataTemplate;
            DataTemplate pickerDisplayDataTemplate;
            DataTemplate mapRegionsDataTemplate;

            public TemplateSelector(Page page)
            {
                numericDisplayDataTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(FamilyKeyPageViewModel.NumericDisplayItem.Prompt), BindingMode.TwoWay));

                    var valueEntry = new Entry { BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5), Keyboard = Keyboard.Numeric };

                    valueEntry.Behaviors.Add(new Xamarin.CommunityToolkit.Behaviors.NumericValidationBehavior() { MinimumValue = 0 });
                    valueEntry.SetBinding(Entry.TextProperty, new Binding(nameof(FamilyKeyPageViewModel.NumericDisplayItem.Value), BindingMode.TwoWay));

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
                pickerDisplayDataTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(FamilyKeyPageViewModel.PickerDisplayItem.Prompt), BindingMode.TwoWay));

                    var valuePicker = new ImagePicker(page) { TextColor = Color.White, VerticalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.FillAndExpand, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5) };
                    valuePicker.SetBinding(ImagePicker.ItemsSourceProperty, new Binding(nameof(FamilyKeyPageViewModel.PickerDisplayItem.Options), BindingMode.OneWay));
                    valuePicker.SetBinding(ImagePicker.SelectedItemProperty, new Binding(nameof(FamilyKeyPageViewModel.PickerDisplayItem.SelectedOptionId), BindingMode.TwoWay));
                    valuePicker.SetBinding(ImagePicker.ImageItemsSourceProperty, new Binding(nameof(FamilyKeyPageViewModel.PickerDisplayItem.ImageSources), BindingMode.TwoWay));

                    var grid = new Grid();
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(descriptionLabel, 0, 0);
                    Grid.SetColumnSpan(descriptionLabel, 1);
                    grid.Children.Add(valuePicker, 1, 0);

                    return new ViewCell { View = grid };
                });
                mapRegionsDataTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(FamilyKeyPageViewModel.MapRegionsDisplayItem.Prompt), BindingMode.TwoWay));

                    var selectButton = new Button
                    {
                        Text = "Select",
                        Style = Styles.RoundedButtonStyle,
                        BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5)
                    };
                    selectButton.SetBinding(Button.CommandProperty, new Binding(nameof(FamilyKeyPageViewModel.MapRegionsDisplayItem.OnSearch), BindingMode.TwoWay));

                    var selectionAmountLabel = new Label { TextColor = Color.White, VerticalTextAlignment = TextAlignment.Center };
                    selectionAmountLabel.SetBinding(Label.TextProperty, new Binding(nameof(FamilyKeyPageViewModel.MapRegionsDisplayItem.SelectionAmount), BindingMode.TwoWay));

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
            }
            protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
            {
                if (item is FamilyKeyPageViewModel.NumericDisplayItem)
                {
                    return numericDisplayDataTemplate;
                }
                else if (item is FamilyKeyPageViewModel.PickerDisplayItem)
                {
                    return pickerDisplayDataTemplate;
                }
                else if (item is FamilyKeyPageViewModel.MapRegionsDisplayItem)
                {
                    return mapRegionsDataTemplate;
                }
                else
                {
                    throw new ApplicationException("Unidentified template type");
                }
            }
        }


    }

}
