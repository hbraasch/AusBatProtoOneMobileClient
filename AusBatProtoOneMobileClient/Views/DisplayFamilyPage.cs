using AusBatProtoOneMobileClient.Models;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using Mobile.Helpers;
using Mobile.ViewModels;
using System;
using Xamarin.Forms;
using static DocGenOneMobileClient.Views.DisplayFamilyPageViewModel;

namespace DocGenOneMobileClient.Views
{
    public class DisplayFamilyPage : ContentPageBase
    {
        bool isFirstAppearance = true; 
        DisplayFamilyPageViewModel viewModel;
        MenuGenerator menu;

        public DisplayFamilyPage(DisplayFamilyPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            var listView = new ListView {
                SelectionMode = ListViewSelectionMode.Single,
                HasUnevenRows = true,
                BackgroundColor = Color.Transparent,
                SeparatorColor = Constants.APP_COLOUR
            };
            listView.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(DisplayFamilyPageViewModel.DisplayItems), BindingMode.TwoWay));
            listView.SetBinding(ListView.SelectedItemProperty, new Binding(nameof(DisplayFamilyPageViewModel.SelectedItem), BindingMode.TwoWay));
            listView.ItemTapped += (s, e) => { viewModel.OnSelectMenuPressed.Execute(true); };
            listView.ItemTemplate = new FamilyKeyDataTemplateSelector();

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

            Title = "Filter results";

            Content = finalLayout;
            BackgroundImageSource = Constants.BACKGROUND_IMAGE;

            menu = new MenuGenerator().Configure()
                .AddMenuItem("home", "Home", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnHomeMenuPressed.Execute(null); })
                .AddMenuItem("back", "Back", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(null); });

            menu.SetVisibilityFactors(viewModel, "IsHomeEnabled")
                .ToShowMenuItem("home", true);

            menu.GenerateToolbarItemsForPage(this);
            menu.SetBinding(MenuGenerator.InvalidateCommandProperty, new Binding(nameof(DisplayFamilyPageViewModel.InvalidateMenu), BindingMode.OneWayToSource, source: viewModel));

            
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
            DataTemplate familyTemplate;

            public FamilyKeyDataTemplateSelector()
            {

                familyTemplate = new DataTemplate(() => {
                    var familyNameLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    familyNameLabel.SetBinding(Label.TextProperty, new Binding(nameof(DisplayFamilyPageViewModel.FamilyDisplayItem.FamilyName), BindingMode.TwoWay));

                    var image = new CachedImage
                    {
                        Aspect = Aspect.AspectFit
                    };
                    image.Transformations.Add(new CircleTransformation());
                    image.SetBinding(CachedImage.SourceProperty, new Binding(nameof(DisplayFamilyPageViewModel.FamilyDisplayItem.ImageSource), BindingMode.OneWay));

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
                if (item is DisplayFamilyPageViewModel.FamilyDisplayItem)
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
