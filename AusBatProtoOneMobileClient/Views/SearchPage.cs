using Mobile.Helpers;
using Mobile.ViewModels;
using System;
using Xamarin.Forms;

namespace DocGenOneMobileClient.Views
{
    public class SearchPage : ContentPageBase
    {
        bool isFirstAppearance = true;
        SearchPageViewModel viewModel;
        MenuGenerator menu;
        int previousItemSelectedIndex = int.MinValue;

        public SearchPage(SearchPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            var searchButton = new SearchBar();
            searchButton.SearchButtonPressed += (s, e) => { viewModel.OnSearchButtonPressed.Execute(true); };

            var criteriaListView = new ListView { SelectionMode = ListViewSelectionMode.Single };
            criteriaListView.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(SearchPageViewModel.CriteriaDisplayItems), BindingMode.TwoWay));
            criteriaListView.SetBinding(ListView.SelectedItemProperty, new Binding(nameof(SearchPageViewModel.CriteriaSelectedItem), BindingMode.TwoWay));
            criteriaListView.ItemTemplate = new CriteriaDataTemplateSelector();

            var mainDisplayInfo = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo;
            var criteriaFrame = new Frame { BorderColor = Color.Red, CornerRadius = 5, Content = criteriaListView, Margin = 5, HeightRequest = mainDisplayInfo.Height/3 };


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

            var resultFrame = new Frame { BorderColor = Color.Red, CornerRadius = 5, Content = resultsListView, Margin = 5 };

            var layout = new StackLayout { Children = { searchButton, criteriaFrame, resultFrame } };

            var listViewLayout = new ScrollView
            {
                Content = layout,
                Orientation = ScrollOrientation.Vertical
            };

            var finalLayout = new AbsoluteLayout
            {
                Children = { listViewLayout, activityIndicator },
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = 5
            };
            AbsoluteLayout.SetLayoutFlags(listViewLayout, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(listViewLayout, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(activityIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(activityIndicator, new Rectangle(0.5, .5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            Title = "SearchPage";

            Content = finalLayout;

            menu = new MenuGenerator().Configure()
                .AddMenuItem("back", "Back", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(null); });

            // menu.AddUwpIcon("back", "ic_back.png");
            menu.GenerateToolbarItemsForPage(this);
            menu.SetBinding(MenuGenerator.InvalidateCommandProperty, new Binding(nameof(SearchPageViewModel.InvalidateMenu), BindingMode.OneWayToSource, source: viewModel));

        }



        private void ToggleSelection(ItemTappedEventArgs e, ListView listView)
        {
            if (e.ItemIndex == previousItemSelectedIndex)
            {
                listView.SelectedItem = null;
                previousItemSelectedIndex = int.MinValue;
            }
            else
            {
                previousItemSelectedIndex = e.ItemIndex;
            }
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
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(SearchPageViewModel.MapRegionsDisplayItem.Description), BindingMode.TwoWay));

                    var selectButton = new Button { Text = "Select" };
                    selectButton.SetBinding(Button.CommandProperty, new Binding(nameof(SearchPageViewModel.MapRegionsDisplayItem.OnSearch), BindingMode.TwoWay));

                    var layout = new StackLayout { Orientation = StackOrientation.Horizontal, Children = { descriptionLabel, selectButton }, Margin = 5 };

                    return new ViewCell { View = layout };

                });

                foreArmLengthTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(SearchPageViewModel.ForeArmLengthDisplayItem.Description), BindingMode.TwoWay));

                    var valueEntry = new Entry { };
                    valueEntry.SetBinding(Entry.TextProperty, new Binding(nameof(SearchPageViewModel.ForeArmLengthDisplayItem.Value), BindingMode.TwoWay));
                    var layout = new StackLayout { Orientation = StackOrientation.Horizontal, Children = { descriptionLabel, valueEntry }, Margin = 5 };

                    return new ViewCell { View = layout };
                });
                OuterCanineWidthTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(SearchPageViewModel.OuterCanineWidthDisplayItem.Description), BindingMode.TwoWay));

                    var valueEntry = new Entry { };
                    valueEntry.SetBinding(Entry.TextProperty, new Binding(nameof(SearchPageViewModel.OuterCanineWidthDisplayItem.Value), BindingMode.TwoWay));
                    var layout = new StackLayout { Orientation = StackOrientation.Horizontal, Children = { descriptionLabel, valueEntry }, Margin = 5 };

                    return new ViewCell { View = layout };
                });
                TailLengthTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(SearchPageViewModel.TailLengthDisplayItem.Description), BindingMode.TwoWay));

                    var valueEntry = new Entry { };
                    valueEntry.SetBinding(Entry.TextProperty, new Binding(nameof(SearchPageViewModel.TailLengthDisplayItem.Value), BindingMode.TwoWay));
                    var layout = new StackLayout { Orientation = StackOrientation.Horizontal, Children = { descriptionLabel, valueEntry }, Margin = 5 };

                    return new ViewCell { View = layout };
                });
                FootWithClawLengthTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(SearchPageViewModel.FootWithClawLengthDisplayItem.Description), BindingMode.TwoWay));

                    var valueEntry = new Entry { };
                    valueEntry.SetBinding(Entry.TextProperty, new Binding(nameof(SearchPageViewModel.FootWithClawLengthDisplayItem.Value), BindingMode.TwoWay));
                    var layout = new StackLayout { Orientation = StackOrientation.Horizontal, Children = { descriptionLabel, valueEntry }, Margin = 5 };

                    return new ViewCell { View = layout };
                });
                PenisLengthTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(SearchPageViewModel.PenisLengthDisplayItem.Description), BindingMode.TwoWay));

                    var valueEntry = new Entry { };
                    valueEntry.SetBinding(Entry.TextProperty, new Binding(nameof(SearchPageViewModel.PenisLengthDisplayItem.Value), BindingMode.TwoWay));
                    var layout = new StackLayout { Orientation = StackOrientation.Horizontal, Children = { descriptionLabel, valueEntry }, Margin = 5 };

                    return new ViewCell { View = layout };
                });
                HeadToBodyLengthTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(SearchPageViewModel.HeadToBodyLengthDisplayItem.Description), BindingMode.TwoWay));

                    var valueEntry = new Entry { };
                    valueEntry.SetBinding(Entry.TextProperty, new Binding(nameof(SearchPageViewModel.HeadToBodyLengthDisplayItem.Value), BindingMode.TwoWay));
                    var layout = new StackLayout { Orientation = StackOrientation.Horizontal, Children = { descriptionLabel, valueEntry }, Margin = 5 };

                    return new ViewCell { View = layout };
                });
                WeightTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(SearchPageViewModel.WeightDisplayItem.Description), BindingMode.TwoWay));

                    var valueEntry = new Entry { };
                    valueEntry.SetBinding(Entry.TextProperty, new Binding(nameof(SearchPageViewModel.WeightDisplayItem.Value), BindingMode.TwoWay));
                    var layout = new StackLayout { Orientation = StackOrientation.Horizontal, Children = { descriptionLabel, valueEntry }, Margin = 5 };

                    return new ViewCell { View = layout };
                });
                ThreeMetTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(SearchPageViewModel.ThreeMetDisplayItem.Description), BindingMode.TwoWay));

                    var valueEntry = new Entry { };
                    valueEntry.SetBinding(Entry.TextProperty, new Binding(nameof(SearchPageViewModel.ThreeMetDisplayItem.Value), BindingMode.TwoWay));
                    var layout = new StackLayout { Orientation = StackOrientation.Horizontal, Children = { descriptionLabel, valueEntry }, Margin = 5 };

                    return new ViewCell { View = layout };
                });
                IsGularPoachPresentTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(SearchPageViewModel.IsGularPoachPresentDisplayItem.Description), BindingMode.TwoWay));

                    var valueCheck = new Switch { };
                    valueCheck.SetBinding(Switch.IsToggledProperty, new Binding(nameof(SearchPageViewModel.IsGularPoachPresentDisplayItem.Value), BindingMode.TwoWay));
                    var layout = new StackLayout { Orientation = StackOrientation.Horizontal, Children = { descriptionLabel, valueCheck }, Margin = 5 };

                    return new ViewCell { View = layout };
                });
                HasFleshyGenitalProjectionsTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(SearchPageViewModel.HasFleshyGenitalProjectionsDisplayItem.Description), BindingMode.TwoWay));

                    var valueCheck = new Switch { };
                    valueCheck.SetBinding(Switch.IsToggledProperty, new Binding(nameof(SearchPageViewModel.HasFleshyGenitalProjectionsDisplayItem.Value), BindingMode.TwoWay));
                    var layout = new StackLayout { Orientation = StackOrientation.Horizontal, Children = { descriptionLabel, valueCheck }, Margin = 5 };

                    return new ViewCell { View = layout };
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

    }
}
