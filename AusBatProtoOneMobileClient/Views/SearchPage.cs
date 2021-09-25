using Mobile.Helpers;
using Mobile.ViewModels;
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

            var criteriaListView = new CollectionView { SelectionMode = SelectionMode.Single };
            criteriaListView.SetBinding(CollectionView.ItemsSourceProperty, new Binding(nameof(SearchPageViewModel.CriteriaDisplayItems), BindingMode.TwoWay));
            criteriaListView.SetBinding(CollectionView.SelectedItemProperty, new Binding(nameof(SearchPageViewModel.CriteriaSelectedItem), BindingMode.TwoWay));
            criteriaListView.ItemTemplate = new CriteriaDataTemplateSelector();


#if false
            var resultsListView = new ListView { SelectionMode = ListViewSelectionMode.Single };
            resultsListView.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(SearchPageViewModel.ResultsDisplayItems), BindingMode.TwoWay));
            resultsListView.SetBinding(ListView.SelectedItemProperty, new Binding(nameof(SearchPageViewModel.ResultsSelectedItem), BindingMode.TwoWay));
            resultsListView.ItemTapped += (s, e) => { ToggleSelection(e, resultsListView); };
            resultsListView.Refreshing += (s, e) => { }; 
#endif

            var layout = new StackLayout { Children = { searchButton, criteriaListView } };

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
                .AddMenuItem("back", "Back", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(null); }, iconPath: "ic_back.png")
                .AddMenuItem("edit", "Edit", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnEditMenuPressed.Execute(null); }, iconPath: "ic_edit.png")
                .AddMenuItem("rename", "Rename", ToolbarItemOrder.Secondary, (menuItem) => { viewModel.OnRenameMenuPressed.Execute(null); })
                .AddMenuItem("delete", "Delete", ToolbarItemOrder.Secondary, (menuItem) => { viewModel.OnDeleteMenuPressed.Execute(null); });

            menu.SetVisibilityFactors(viewModel, "IsSelected", "IsChecked")
                .ToShowMenuItem("edit", true, null)
                .ToShowMenuItem("delete", true, null)
                .ToShowMenuItem("delete", null, true)
                .ToShowMenuItem("rename", true, null)
                .ToShowMenuItem("rename", null, true);

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
            DataTemplate floatValueTemplate;

            public CriteriaDataTemplateSelector()
            {
                regionTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(SearchPageViewModel.CriteriaDisplayItemBase.Description), BindingMode.TwoWay), source: viewModel);

                    // var selectButton = new Button { Text = "Select" };

                    // var layout = new StackLayout { Orientation = StackOrientation.Horizontal, Children = { descriptionLabel, selectButton }, Margin = 5 };

                    return new ViewCell { View = descriptionLabel };

                });

                floatValueTemplate = new DataTemplate(() => {
                    var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center };
                    descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(SearchPageViewModel.CriteriaDisplayItemBase.Description), BindingMode.TwoWay));

                    // var selectButton = new Button { Text = "Select" };
                    // var layout = new StackLayout { Orientation = StackOrientation.Horizontal, Children = { descriptionLabel, selectButton }, Margin = 5 };

                    return new ViewCell { View = descriptionLabel };
                });
            }

            protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
            {
                if (item is SearchPageViewModel.MapRegionsDisplayItem)
                {
                    return regionTemplate;
                }
                else
                {
                    if (item is SearchPageViewModel.ForeArmLengthDisplayItem)
                    {
                        return floatValueTemplate;
                    }
                    else
                    {
                        return new DataTemplate();
                    }
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
