using AusBatProtoOneMobileClient.ViewModels;
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

            var criteriaListView = new ListView { SelectionMode = ListViewSelectionMode.Single };
            criteriaListView.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(SearchPageViewModel.CriteriaDisplayItems), BindingMode.TwoWay));
            criteriaListView.SetBinding(ListView.SelectedItemProperty, new Binding(nameof(SearchPageViewModel.CriteriaSelectedItem), BindingMode.TwoWay));
            criteriaListView.ItemTapped += (s, e) => { ToggleSelection(e, criteriaListView); };
            criteriaListView.Refreshing += (s, e) => {};

            var resultsListView = new ListView { SelectionMode = ListViewSelectionMode.Single };
            resultsListView.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(SearchPageViewModel.ResultsDisplayItems), BindingMode.TwoWay));
            resultsListView.SetBinding(ListView.SelectedItemProperty, new Binding(nameof(SearchPageViewModel.ResultsSelectedItem), BindingMode.TwoWay));
            resultsListView.ItemTapped += (s, e) => { ToggleSelection(e, resultsListView); };
            resultsListView.Refreshing += (s, e) => { };

            var layout = new StackLayout { Children = { searchButton, criteriaListView, resultsListView } };

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

        public class ListViewDataSearch: ViewCell
        {
            CheckBox checkBox;

            public static readonly BindableProperty OnCheckChangedCommandProperty = BindableProperty.Create(nameof(OnCheckChangedCommand), typeof(Command), typeof(ListViewDataSearch), null);
            public Command OnCheckChangedCommand
            {
                get { return (Command)GetValue(OnCheckChangedCommandProperty); }
                set { SetValue(OnCheckChangedCommandProperty, value); }
            }

            public class DisplayItem
            {
                public int Id { get; set; }
                public bool IsChecked { get; set; }
                public string Description { get; set; }
                public object Content { get; set; } 
                public Command OnCheckChangedCommand { get; set; }

            }


            public ListViewDataSearch()
            {
                SetBinding(OnCheckChangedCommandProperty, new Binding(nameof(DisplayItem.OnCheckChangedCommand), BindingMode.TwoWay));

                var descriptionLabel = new Label {  VerticalTextAlignment = TextAlignment.Center };
                descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(DisplayItem.Description), BindingMode.TwoWay));

                checkBox = new CheckBox { Visual = VisualMarker.Material };
                checkBox.SetBinding(CheckBox.IsCheckedProperty, new Binding(nameof(DisplayItem.IsChecked), BindingMode.TwoWay));

                View = new StackLayout { Orientation = StackOrientation.Horizontal, Children = { checkBox, descriptionLabel }, Margin = 5};

            }

            protected override void OnAppearing()
            {
                base.OnAppearing();
                checkBox.CheckedChanged += (s, e) => { OnCheckChangedCommand?.Execute(null); };
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
