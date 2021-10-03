using AusBatProtoOneMobileClient.Data;
using AusBatProtoOneMobileClient.Models;
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

            var characteristicListView = new ListView { SelectionMode = ListViewSelectionMode.None };
            characteristicListView.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(FamilyKeyPageViewModel.CharacteristicDisplayItems), BindingMode.TwoWay));
            characteristicListView.ItemTemplate = new characteristicDataTemplateSelector();

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

            var layout = new StackLayout { Children = {characteristicListView, regionButton, filterNowButton } };

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
                .AddMenuItem("clear", "Clear selections", ToolbarItemOrder.Secondary, (menuItem) => { viewModel.OnClearFiltersClicked.Execute(null); });


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



        internal class characteristicDataTemplateSelector : DataTemplateSelector
        {
            DataTemplate tailPresentCharacteristicTemplate;
            DataTemplate tailMembraneStructureCharacteristicTemplate;

            public characteristicDataTemplateSelector()
            {  
                tailPresentCharacteristicTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(FamilyKeyPageViewModel.TailPresentCharacteristicDisplayItem.Description), BindingMode.TwoWay));

                    var valuePicker = new Picker { TextColor = Color.White, VerticalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.FillAndExpand, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5) };
                    valuePicker.SetBinding(Picker.ItemsSourceProperty, new Binding(nameof(FamilyKeyPageViewModel.TailPresentCharacteristicDisplayItem.Values), BindingMode.OneWay));
                    valuePicker.SetBinding(Picker.SelectedItemProperty, new Binding(nameof(FamilyKeyPageViewModel.TailPresentCharacteristicDisplayItem.Value), BindingMode.TwoWay, new TailPresentCharacteristicConverter()));

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
                tailMembraneStructureCharacteristicTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(FamilyKeyPageViewModel.TailMembraneStructureCharacteristicDisplayItem.Description), BindingMode.TwoWay));

                    var valuePicker = new Picker { TextColor = Color.White, VerticalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.FillAndExpand, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5) };
                    valuePicker.SetBinding(Picker.ItemsSourceProperty, new Binding(nameof(FamilyKeyPageViewModel.TailMembraneStructureCharacteristicDisplayItem.Values), BindingMode.OneWay));
                    valuePicker.SetBinding(Picker.SelectedItemProperty, new Binding(nameof(FamilyKeyPageViewModel.TailMembraneStructureCharacteristicDisplayItem.Value), BindingMode.TwoWay, new TailMembraneStructureConverter()));

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
                if (item is FamilyKeyPageViewModel.TailPresentCharacteristicDisplayItem)
                {
                    return tailPresentCharacteristicTemplate;
                }
                else if (item is FamilyKeyPageViewModel.TailMembraneStructureCharacteristicDisplayItem)
                {
                    return tailMembraneStructureCharacteristicTemplate;
                }
                else
                {
                    throw new ApplicationException("Unidentified template type");
                }
            }
        }


    }

    internal class TailPresentCharacteristicConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TailPresentCharacteristic)
            {
                var characteristic = value as TailPresentCharacteristic;
                return characteristic.GetPrompt();
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                var prompt = (string)value;
                return TailPresentCharacteristic.CreateFromPrompt(prompt);
            }
            return new TailPresentCharacteristic(TailPresentCharacteristic.TailPresentEnum.Undefined);
        }
    }

    internal class TailMembraneStructureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TailMembraneStructureCharacteristic)
            {
                var characteristic = value as TailMembraneStructureCharacteristic;
                return characteristic.GetPrompt();
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {

                var prompt = (string) value;
                return TailMembraneStructureCharacteristic.CreateFromPrompt(prompt);
            }
            return new TailMembraneStructureCharacteristic(TailMembraneStructureCharacteristic.TailMembraneStructureEnum.Undefined);
        }

    }
}
