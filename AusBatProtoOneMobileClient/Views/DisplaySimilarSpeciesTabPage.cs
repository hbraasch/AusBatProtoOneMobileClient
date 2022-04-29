using AusBatProtoOneMobileClient.Models;
using AusBatProtoOneMobileClient.ViewModels;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using Mobile.Helpers;
using Mobile.ViewModels;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using static DocGenOneMobileClient.Views.SpeciesByAreaPageViewModel;

namespace AusBatProtoOneMobileClient
{
    public class DisplaySimilarSpeciesTabPage : ContentPageBase
    {
        bool isFirstAppearance = true;
        DisplaySpeciesTabbedPageViewModel viewModel;

        public DisplaySimilarSpeciesTabPage(DisplaySpeciesTabbedPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            var listView = new ListView {
                SelectionMode = ListViewSelectionMode.Single,
                HasUnevenRows = true,
                IsGroupingEnabled = false,
                BackgroundColor = Color.Transparent,
                SeparatorColor = Constants.APP_COLOUR,
                Margin = 5
            };
            listView.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(DisplaySpeciesTabbedPageViewModel.SimilarSpeciesDisplayItems), BindingMode.TwoWay));
            listView.SetBinding(ListView.SelectedItemProperty, new Binding(nameof(DisplaySpeciesTabbedPageViewModel.SimilarSpeciesSelectedItem), BindingMode.TwoWay));
            listView.ItemTapped += (s, e) => { viewModel.OnSimilarSpeciesSelectMenuPressed.Execute(null); };
            listView.ItemTemplate = new SpeciesKeyDataTemplateSelector();
         
            var listViewLayout = new ScrollView
            {
                Content = listView,
                Orientation = ScrollOrientation.Vertical
            };

            var finalLayout = new AbsoluteLayout
            {
                Children = { listViewLayout, activityIndicator },
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            AbsoluteLayout.SetLayoutFlags(listViewLayout, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(listViewLayout, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(activityIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(activityIndicator, new Rectangle(0.5, .5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            Content = finalLayout;
            NavigationPage.SetTitleView(this, new Label { Text = "Similar species", Style = Styles.TitleLabelStyle });
            BackgroundColor = Color.Black;


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
                    speciesNameLabel.SetBinding(Label.TextProperty, new Binding(nameof(DisplaySpeciesTabbedPageViewModel.SpeciesDisplayItem.SpeciesName), BindingMode.TwoWay));

                    var speciesFriendlyNameLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Constants.COMMON_NAME_COLOUR };
                    speciesFriendlyNameLabel.SetBinding(Label.TextProperty, new Binding(nameof(DisplaySpeciesTabbedPageViewModel.SpeciesDisplayItem.FriendlyName), BindingMode.TwoWay));


                    var heightRequest = Device.GetNamedSize(NamedSize.Large, typeof(Label)) * 2;
                    var image = new CachedImage
                    {
                        Aspect = Aspect.AspectFit,
                        HeightRequest = heightRequest,
                        ErrorPlaceholder = "bat.png"
                    };
                    image.Transformations.Add(new CircleTransformation() { BorderHexColor = "C0C0C0", BorderSize = 7 });
                    image.SetBinding(CachedImage.SourceProperty, new Binding(nameof(DisplaySpeciesTabbedPageViewModel.SpeciesDisplayItem.ImageSource), BindingMode.OneWay));

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
                if (item is DisplaySpeciesTabbedPageViewModel.SpeciesDisplayItem)
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
