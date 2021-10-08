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

            var CharacterListView = new ListView { SelectionMode = ListViewSelectionMode.None };
            CharacterListView.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(FamilyKeyPageViewModel.CharacterDisplayItems), BindingMode.TwoWay));
            CharacterListView.ItemTemplate = new CharacterDataTemplateSelector(this);

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

            var layout = new StackLayout { Children = {CharacterListView, regionButton, filterNowButton } };

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



        internal class CharacterDataTemplateSelector : DataTemplateSelector
        {
            DataTemplate tailPresentCharacterTemplate;
            DataTemplate tailMembraneStructureCharacterTemplate;
            DataTemplate secondFingerClawCharacterTemplate;
            DataTemplate faceStructureNoseLeafCharacterTemplate;
            DataTemplate wingThirdFingerCharacterTemplate;
            DataTemplate tragusStructureCharacterTemplate;


            public CharacterDataTemplateSelector(Page page)
            {  
                tailPresentCharacterTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(FamilyKeyPageViewModel.TailPresentCharacterDisplayItem.Description), BindingMode.TwoWay));


                    var valuePicker = new PickerWithImages(page);
                    valuePicker.SetBinding(PickerWithImages.ItemsSourceProperty, new Binding(nameof(FamilyKeyPageViewModel.TailPresentCharacterDisplayItem.Values), BindingMode.OneWay));
                    valuePicker.SetBinding(PickerWithImages.ImagesItemSourceProperty, new Binding(nameof(FamilyKeyPageViewModel.TailPresentCharacterDisplayItem.ImageSources), BindingMode.OneWay));
                    valuePicker.SetBinding(PickerWithImages.SelectedItemProperty, new Binding(nameof(FamilyKeyPageViewModel.TailPresentCharacterDisplayItem.Value), BindingMode.TwoWay, new TailPresentCharacterConverter()));

                    var grid = new Grid();
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(descriptionLabel, 0, 0);
                    Grid.SetColumnSpan(descriptionLabel, 1);
                    grid.Children.Add(valuePicker, 1, 0);

                    return new ViewCell { View = grid };
                });
                tailMembraneStructureCharacterTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(FamilyKeyPageViewModel.TailMembraneStructureCharacterDisplayItem.Description), BindingMode.TwoWay));

                    var valuePicker = new Picker { TextColor = Color.White, VerticalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.FillAndExpand, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5) };
                    valuePicker.SetBinding(Picker.ItemsSourceProperty, new Binding(nameof(FamilyKeyPageViewModel.TailMembraneStructureCharacterDisplayItem.Values), BindingMode.OneWay));
                    valuePicker.SetBinding(Picker.SelectedItemProperty, new Binding(nameof(FamilyKeyPageViewModel.TailMembraneStructureCharacterDisplayItem.Value), BindingMode.TwoWay, new TailMembraneStructureConverter()));

                    var grid = new Grid();
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(descriptionLabel, 0, 0);
                    Grid.SetColumnSpan(descriptionLabel, 1);
                    grid.Children.Add(valuePicker, 1, 0);

                    return new ViewCell { View = grid };
                });
                secondFingerClawCharacterTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(FamilyKeyPageViewModel.SecondFingerClawCharacterDisplayItem.Description), BindingMode.TwoWay));

                    var valuePicker = new Picker { TextColor = Color.White, VerticalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.FillAndExpand, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5) };
                    valuePicker.SetBinding(Picker.ItemsSourceProperty, new Binding(nameof(FamilyKeyPageViewModel.SecondFingerClawCharacterDisplayItem.Values), BindingMode.OneWay));
                    valuePicker.SetBinding(Picker.SelectedItemProperty, new Binding(nameof(FamilyKeyPageViewModel.SecondFingerClawCharacterDisplayItem.Value), BindingMode.TwoWay, new SecondFingerClawConverter()));

                    var grid = new Grid();
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(descriptionLabel, 0, 0);
                    Grid.SetColumnSpan(descriptionLabel, 1);
                    grid.Children.Add(valuePicker, 1, 0);

                    return new ViewCell { View = grid };
                });
                faceStructureNoseLeafCharacterTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(FamilyKeyPageViewModel.FaceStructureNoseLeafCharacterDisplayItem.Description), BindingMode.TwoWay));

                    var valuePicker = new Picker { TextColor = Color.White, VerticalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.FillAndExpand, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5) };
                    valuePicker.SetBinding(Picker.ItemsSourceProperty, new Binding(nameof(FamilyKeyPageViewModel.FaceStructureNoseLeafCharacterDisplayItem.Values), BindingMode.OneWay));
                    valuePicker.SetBinding(Picker.SelectedItemProperty, new Binding(nameof(FamilyKeyPageViewModel.FaceStructureNoseLeafCharacterDisplayItem.Value), BindingMode.TwoWay, new FaceStructureNoseLeafConverter()));

                    var grid = new Grid();
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(descriptionLabel, 0, 0);
                    Grid.SetColumnSpan(descriptionLabel, 1);
                    grid.Children.Add(valuePicker, 1, 0);

                    return new ViewCell { View = grid };
                });
                wingThirdFingerCharacterTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(FamilyKeyPageViewModel.WingThirdFingerCharacterDisplayItem.Description), BindingMode.TwoWay));

                    var valuePicker = new Picker { TextColor = Color.White, VerticalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.FillAndExpand, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5) };
                    valuePicker.SetBinding(Picker.ItemsSourceProperty, new Binding(nameof(FamilyKeyPageViewModel.WingThirdFingerCharacterDisplayItem.Values), BindingMode.OneWay));
                    valuePicker.SetBinding(Picker.SelectedItemProperty, new Binding(nameof(FamilyKeyPageViewModel.WingThirdFingerCharacterDisplayItem.Value), BindingMode.TwoWay, new WingThirdFingerConverter()));

                    var grid = new Grid();
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(descriptionLabel, 0, 0);
                    Grid.SetColumnSpan(descriptionLabel, 1);
                    grid.Children.Add(valuePicker, 1, 0);

                    return new ViewCell { View = grid };
                });
                tragusStructureCharacterTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(FamilyKeyPageViewModel.TragusCharacterDisplayItem.Description), BindingMode.TwoWay));

                    var valuePicker = new PickerWithImages(page);
                    valuePicker.SetBinding(PickerWithImages.ItemsSourceProperty, new Binding(nameof(FamilyKeyPageViewModel.TragusCharacterDisplayItem.Values), BindingMode.OneWay));
                    valuePicker.SetBinding(PickerWithImages.ImagesItemSourceProperty, new Binding(nameof(FamilyKeyPageViewModel.TragusCharacterDisplayItem.ImageSources), BindingMode.OneWay));
                    valuePicker.SetBinding(PickerWithImages.SelectedItemProperty, new Binding(nameof(FamilyKeyPageViewModel.TragusCharacterDisplayItem.Value), BindingMode.TwoWay, new TailPresentCharacterConverter()));

                    var grid = new Grid();
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(descriptionLabel, 0, 0);
                    Grid.SetColumnSpan(descriptionLabel, 1);
                    grid.Children.Add(valuePicker, 1, 0);

                    return new ViewCell { View = grid };
                });
            }

            protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
            {
                if (item is FamilyKeyPageViewModel.TailPresentCharacterDisplayItem)
                {
                    return tailPresentCharacterTemplate;
                }
                else if (item is FamilyKeyPageViewModel.TailMembraneStructureCharacterDisplayItem)
                {
                    return tailMembraneStructureCharacterTemplate;
                }
                else if (item is FamilyKeyPageViewModel.SecondFingerClawCharacterDisplayItem)
                {
                    return secondFingerClawCharacterTemplate;
                }
                else if (item is FamilyKeyPageViewModel.FaceStructureNoseLeafCharacterDisplayItem)
                {
                    return faceStructureNoseLeafCharacterTemplate;
                }
                else if (item is FamilyKeyPageViewModel.WingThirdFingerCharacterDisplayItem)
                {
                    return wingThirdFingerCharacterTemplate;
                }
                else if (item is FamilyKeyPageViewModel.TragusCharacterDisplayItem)
                {
                    return tragusStructureCharacterTemplate;
                }
                else
                {
                    throw new ApplicationException("Unidentified template type");
                }
            }
        }


    }

    internal class TragusCharacterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TragusCharacter)
            {
                var Character = value as TragusCharacter;
                return Character.GetPrompt();
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                var prompt = (string)value;
                return TragusCharacter.CreateFromPrompt(prompt);
            }
            return new TragusCharacter(TragusCharacter.TragusEnum.Undefined);
        }
    }

    internal class WingThirdFingerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is WingThirdFingerCharacter)
            {
                var Character = value as WingThirdFingerCharacter;
                return Character.GetPrompt();
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                var prompt = (string)value;
                return WingThirdFingerCharacter.CreateFromPrompt(prompt);
            }
            return new WingThirdFingerCharacter(WingThirdFingerCharacter.WingThirdFingerEnum.Undefined);
        }
    }

    internal class FaceStructureNoseLeafConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is FaceStructureNoseLeafCharacter)
            {
                var Character = value as FaceStructureNoseLeafCharacter;
                return Character.GetPrompt();
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                var prompt = (string)value;
                return FaceStructureNoseLeafCharacter.CreateFromPrompt(prompt);
            }
            return new FaceStructureNoseLeafCharacter(FaceStructureNoseLeafCharacter.FaceStructureNoseLeafEnum.Undefined);
        }
    }

    internal class SecondFingerClawConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SecondFingerClawCharacter)
            {
                var Character = value as SecondFingerClawCharacter;
                return Character.GetPrompt();
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                var prompt = (string)value;
                return SecondFingerClawCharacter.CreateFromPrompt(prompt);
            }
            return new SecondFingerClawCharacter(SecondFingerClawCharacter.SecondFingerClawEnum.Undefined);
        }
    }

    internal class TailPresentCharacterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TailPresentCharacter)
            {
                var Character = value as TailPresentCharacter;
                return Character.GetPrompt();
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                var prompt = (string)value;
                return TailPresentCharacter.CreateFromPrompt(prompt);
            }
            return new TailPresentCharacter(TailPresentCharacter.TailPresentEnum.Undefined);
        }
    }

    internal class TailMembraneStructureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TailMembraneStructureCharacter)
            {
                var Character = value as TailMembraneStructureCharacter;
                return Character.GetPrompt();
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {

                var prompt = (string) value;
                return TailMembraneStructureCharacter.CreateFromPrompt(prompt);
            }
            return new TailMembraneStructureCharacter(TailMembraneStructureCharacter.TailMembraneStructureEnum.Undefined);
        }

    }
}
