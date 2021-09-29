using Mobile.Helpers;
using Mobile.ViewModels;
using Xamarin.Forms;
using static DocGenOneMobileClient.Views.SpeciesByFamilyPageViewModel;

namespace DocGenOneMobileClient.Views
{
    public class SpeciesByFamilyPage : ContentPageBase
    {
        bool isFirstAppearance = true; 
        SpeciesByFamilyPageViewModel viewModel;
        MenuGenerator menu;
        ImageButton actionButton;

        public SpeciesByFamilyPage(SpeciesByFamilyPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            var listView = new ListView { 
                SelectionMode = ListViewSelectionMode.Single,
                IsGroupingEnabled = true,
                GroupDisplayBinding = new Binding (nameof(SpeciesByFamilyPageViewModel.PageTypeGroup.FamilyName)),
                GroupShortNameBinding = new Binding(nameof(SpeciesByFamilyPageViewModel.PageTypeGroup.ShortFamilyName))
            };
            listView.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(SpeciesByFamilyPageViewModel.PageTypeGroup.All), BindingMode.TwoWay));
            listView.SetBinding(ListView.SelectedItemProperty, new Binding(nameof(SpeciesByFamilyPageViewModel.SelectedItem), BindingMode.TwoWay));
            listView.ItemTapped += (s, e) => {  };
            listView.ItemTemplate = new DataTemplate(typeof(ListViewDataTemplate));

            actionButton = new ImageButton { Source = "ic_action.png", BackgroundColor = Color.Transparent };
            // actionButton.Clicked += (s,e) => { viewModel.OnAddMenuPressed.Execute(true); };

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

            Title = "SpeciesByFamilyPage";

            Content = finalLayout;

            menu = new MenuGenerator().Configure()
                .AddMenuItem("back", "Back", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(null); });
 
            menu.GenerateToolbarItemsForPage(this);
            menu.SetBinding(MenuGenerator.InvalidateCommandProperty, new Binding(nameof(SpeciesByFamilyPageViewModel.InvalidateMenu), BindingMode.OneWayToSource, source: viewModel));

            
        }


        public class ListViewDataTemplate: ViewCell
        {

            public ListViewDataTemplate()
            {

                var speciesNameLabel = new Label {  VerticalTextAlignment = TextAlignment.Center };
                speciesNameLabel.SetBinding(Label.TextProperty, new Binding(nameof(PageModel.SpeciesName), BindingMode.TwoWay));

                var friendlyNameLabel = new Label { VerticalTextAlignment = TextAlignment.Center };
                friendlyNameLabel.SetBinding(Label.TextProperty, new Binding(nameof(PageModel.FriendlyName), BindingMode.TwoWay));

                View = new StackLayout { Orientation = StackOrientation.Vertical, Children = { speciesNameLabel, friendlyNameLabel }, Margin = 5};

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
