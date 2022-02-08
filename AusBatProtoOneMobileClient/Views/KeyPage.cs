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
using static DocGenOneMobileClient.Views.KeyPage.TemplateSelector;

namespace DocGenOneMobileClient.Views
{
    public class KeyPage : ContentPageBase
    {
        KeyPageViewModel viewModel;
        public KeyPage(KeyPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            var filterHintLabel = new Label { TextColor = Color.White };
            filterHintLabel.SetBinding(Label.TextProperty, new Binding(nameof(KeyPageViewModel.FilterHint), BindingMode.TwoWay));
            filterHintLabel.SetBinding(Label.IsVisibleProperty, new Binding(nameof(KeyPageViewModel.FilterHint), BindingMode.OneWay, new IsStringEmptyConverter() ));

            var characterListView = new ListView(ListViewCachingStrategy.RetainElement) { 
                SelectionMode = ListViewSelectionMode.None,
                SeparatorColor = Constants.APP_COLOUR,
                HasUnevenRows = false,
                BackgroundColor = Color.Transparent
            };

            characterListView.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(KeyPageViewModel.CharacterDisplayItems), BindingMode.TwoWay));
            characterListView.ItemTemplate = new TemplateSelector(this);


            var resultLabel = new Label {TextColor = Color.White, FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label)) };
            resultLabel.SetBinding(Label.TextProperty, new Binding(nameof(KeyPageViewModel.FilterResult), BindingMode.TwoWay));

            var viewResultsButton = new Button
            {
                Text = "View Taxa",
                Style = Styles.RoundedButtonStyle,
                BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                
            };
            viewResultsButton.Clicked += (s, e) => { viewModel.OnViewResultsClicked.Execute(null); };

            var resultGrid = new Grid() { HorizontalOptions = LayoutOptions.FillAndExpand };
            resultGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            resultGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            resultGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            resultGrid.Children.Add(resultLabel, 0, 0);
            resultGrid.Children.Add(viewResultsButton, 0, 1);

            var resultFrame = new Frame { CornerRadius = 5, BorderColor = Constants.APP_COLOUR, Content = resultGrid, BackgroundColor = Color.Transparent, HorizontalOptions = LayoutOptions.FillAndExpand };

            var layout = new StackLayout { Children = {filterHintLabel, characterListView, resultFrame } , HorizontalOptions = LayoutOptions.FillAndExpand };

            var backgroundImage = new Image { Aspect = Aspect.AspectFill, Source = Constants.BACKGROUND_IMAGE };

            var finalLayout = new AbsoluteLayout
            {
                Children = { backgroundImage, layout, activityIndicator },
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = 5
            };
            AbsoluteLayout.SetLayoutFlags(backgroundImage, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(backgroundImage, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(layout, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(layout, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(activityIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(activityIndicator, new Rectangle(0.5, .5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            var titleLabel = new Xamarin.Forms.Label { Text = "Filter", Style = Styles.TitleLabelStyle };
            titleLabel.SetBinding(Label.TextProperty, new Binding(nameof(KeyPageViewModel.Title), BindingMode.TwoWay));
            NavigationPage.SetTitleView(this, titleLabel);

            BackgroundImageSource = Constants.BACKGROUND_IMAGE;
            Content = finalLayout;

            var menu = new MenuGenerator().Configure()
                .AddMenuItem("home", "Home", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnHomeMenuPressed.Execute(null); })
                .AddMenuItem("back", "Back", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(null); })
                .AddMenuItem("reset", "Reset", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnResetFiltersClicked.Execute(null); })
                .AddMenuItem("undo", "Undo", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnUndoFilterActionClicked.Execute(null); });

            menu.SetVisibilityFactors(viewModel, "IsResetFilterEnabled", "CanUndo", "IsBackMenuEnabled")
                .ToShowMenuItem("back", null, null, true)
                .ToShowMenuItem("reset", true, null, null)
                .ToShowMenuItem("undo", null, true, null);

            menu.GenerateToolbarItemsForPage(this);
            menu.SetBinding(MenuGenerator.InvalidateCommandProperty, new Binding(nameof(KeyPageViewModel.InvalidateMenuCommand), BindingMode.OneWayToSource, source: viewModel));
            viewModel.InvalidateMenuCommand = new Command(() => { menu.Invalidate(); });

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
            DataTemplate numericDisplayDataTemplate;
            DataTemplate pickerDisplayDataTemplate;
            DataTemplate mapRegionsDataTemplate;

            public TemplateSelector(Page page)
            {
                numericDisplayDataTemplate = new DataTemplate(() => {

                    var image = new CachedImageWithTap
                    {
                        Aspect = Aspect.AspectFit,
                        HorizontalOptions = LayoutOptions.End
                    };
                    image.Transformations.Add(new RoundedTransformation());
                    image.SetBinding(CachedImageWithTap.SourceProperty, new Binding(nameof(KeyPageViewModel.NumericDisplayItem.ImageSource), BindingMode.OneWay));
                    image.SetBinding(CachedImageWithTap.IsVisibleProperty, new Binding(nameof(KeyPageViewModel.NumericDisplayItem.ImageSource), BindingMode.OneWay, new IsStringEmptyConverter()));
                    image.SetBinding(CachedImageWithTap.OnTappedProperty, new Binding(nameof(KeyPageViewModel.NumericDisplayItem.OnImageClicked), BindingMode.TwoWay));

                    var descriptionLabel = new Label { HorizontalOptions = LayoutOptions.StartAndExpand, VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(KeyPageViewModel.NumericDisplayItem.Prompt), BindingMode.TwoWay));

                    var descripionLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Children = { descriptionLabel, image } };

                    var valueEntry = new TriggerEntry { BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5), Keyboard = Keyboard.Numeric };
                    // valueEntry.Behaviors.Add(new Xamarin.CommunityToolkit.Behaviors.NumericValidationBehavior() { MinimumValue = 0 });
                    valueEntry.SetBinding(TriggerEntry.TextProperty, new Binding(nameof(KeyPageViewModel.NumericDisplayItem.Value), BindingMode.TwoWay));
                    valueEntry.SetBinding(TriggerEntry.OnChangedProperty, new Binding(nameof(KeyPageViewModel.NumericDisplayItem.OnChanged), BindingMode.TwoWay));

                    var grid = new Grid { Margin = 2 };
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(descripionLayout, 0, 0);
                    Grid.SetColumnSpan(descripionLayout, 1);
                    grid.Children.Add(valueEntry, 1, 0);


                    return new ViewCell { View = grid };
                });
                pickerDisplayDataTemplate = new DataTemplate(() => {

                    var image = new CachedImageWithTap
                    {
                        Aspect = Aspect.AspectFit,
                        HorizontalOptions = LayoutOptions.End
                    };
                    image.Transformations.Add(new RoundedTransformation());
                    image.SetBinding(CachedImageWithTap.SourceProperty, new Binding(nameof(KeyPageViewModel.PickerDisplayItem.ImageSource), BindingMode.OneWay));
                    image.SetBinding(CachedImageWithTap.IsVisibleProperty, new Binding(nameof(KeyPageViewModel.PickerDisplayItem.ImageSource), BindingMode.OneWay, new IsStringEmptyConverter()));
                    image.SetBinding(CachedImageWithTap.OnTappedProperty, new Binding(nameof(KeyPageViewModel.PickerDisplayItem.OnImageClicked), BindingMode.TwoWay));

                    var descriptionLabel = new Label { HorizontalOptions = LayoutOptions.StartAndExpand, VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(KeyPageViewModel.PickerDisplayItem.Prompt), BindingMode.TwoWay));

                    var descripionLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Children = { descriptionLabel, image  } };

                    var valuePicker = new ImagePicker(page) { TextColor = Color.White, VerticalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.FillAndExpand, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5) };
                    valuePicker.SetBinding(ImagePicker.ItemsSourceProperty, new Binding(nameof(KeyPageViewModel.PickerDisplayItem.Options), BindingMode.OneWay));
                    valuePicker.SetBinding(ImagePicker.SelectedItemProperty, new Binding(nameof(KeyPageViewModel.PickerDisplayItem.SelectedOption), BindingMode.TwoWay));
                    valuePicker.SetBinding(ImagePicker.ImageItemsSourceProperty, new Binding(nameof(KeyPageViewModel.PickerDisplayItem.ImageSources), BindingMode.TwoWay));
                    valuePicker.SetBinding(ImagePicker.OnChangedProperty, new Binding(nameof(KeyPageViewModel.PickerDisplayItem.OnChanged), BindingMode.TwoWay));
                    valuePicker.SetBinding(ImagePicker.PromptProperty, new Binding(nameof(KeyPageViewModel.PickerDisplayItem.Prompt), BindingMode.TwoWay));

                    var grid = new Grid { Margin = 2 };
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(descripionLayout, 0, 0);
                    grid.Children.Add(valuePicker, 1, 0);

                    return new ViewCell { View = grid };
                });
                mapRegionsDataTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(KeyPageViewModel.MapRegionsDisplayItem.Prompt), BindingMode.TwoWay));

                    var selectButton = new Button
                    {
                        Text = "Select",
                        Style = Styles.RegionSelectButtonStyle,
                        BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5)
                    };
                    selectButton.SetBinding(Button.CommandProperty, new Binding(nameof(KeyPageViewModel.MapRegionsDisplayItem.OnSelectRegionClicked), BindingMode.TwoWay));

                    var selectionAmountLabel = new Label { TextColor = Color.White, VerticalTextAlignment = TextAlignment.Center };
                    selectionAmountLabel.SetBinding(Label.TextProperty, new Binding(nameof(KeyPageViewModel.MapRegionsDisplayItem.SelectionAmount), BindingMode.TwoWay));

                    var grid = new Grid { Margin = 2 };
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(descriptionLabel, 0, 0);
                    grid.Children.Add(selectButton, 1, 0);

                    return new ViewCell { View = grid };

                });
            }
            protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
            {
                if (item is KeyPageViewModel.NumericDisplayItem)
                {
                    return numericDisplayDataTemplate;
                }
                else if (item is KeyPageViewModel.PickerDisplayItem)
                {
                    return pickerDisplayDataTemplate;
                }
                else if (item is KeyPageViewModel.MapRegionsDisplayItem)
                {
                    return mapRegionsDataTemplate;
                }
                else
                {
                    throw new ApplicationException("Unidentified template type");
                }
            }

            public class IsStringEmptyConverter : IValueConverter
            {
                public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
                {
                    return !(bool) string.IsNullOrEmpty((value as string));
                }

                public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
                {
                    throw new NotImplementedException();
                }
            }
        }


    }

}
