using AusBatProtoOneMobileClient.Models;
using Mobile.Helpers;
using Mobile.ViewModels;
using Xamarin.Forms;

namespace DocGenOneMobileClient.Views
{
    public class TemplateListPage : ContentPageBase
    {
        bool isFirstAppearance = true; 
        TemplateListListsPageViewModel viewModel;
        MenuGenerator menu;
        int previousItemSelectedIndex = int.MinValue;
        ImageButton actionButton;

        public TemplateListPage(TemplateListListsPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            var listView = new ListView { SelectionMode = ListViewSelectionMode.Single, SeparatorColor = Constants.APP_COLOUR };
            listView.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(TemplateListListsPageViewModel.DisplayItems), BindingMode.TwoWay));
            listView.SetBinding(ListView.SelectedItemProperty, new Binding(nameof(TemplateListListsPageViewModel.SelectedItem), BindingMode.TwoWay));
            listView.ItemTapped += (s, e) => { ToggleSelection(e, listView); };
            listView.ItemTemplate = new DataTemplate(typeof(ListViewDataTemplate));
            listView.IsPullToRefreshEnabled = true;
            listView.Refreshing += (s, e) => {};

            actionButton = new ImageButton { Source = "ic_action.png", BackgroundColor = Color.Transparent };
            actionButton.Clicked += (s,e) => { viewModel.OnAddMenuPressed.Execute(true); };

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

            NavigationPage.SetTitleView(this, new Xamarin.Forms.Label { Text = "TemplateListPage:", VerticalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Start, TextColor = Color.White });
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
            menu.SetBinding(MenuGenerator.InvalidateCommandProperty, new Binding(nameof(TemplateListListsPageViewModel.InvalidateMenu), BindingMode.OneWayToSource, source: viewModel));
            menu.FindMenuUnit("back").isVisible = viewModel.IsPageReturnable;
            
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

        public class ListViewDataTemplate: ViewCell
        {
            CheckBox checkBox;

            public static readonly BindableProperty OnCheckChangedCommandProperty = BindableProperty.Create(nameof(OnCheckChangedCommand), typeof(Command), typeof(ListViewDataTemplate), null);
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


            public ListViewDataTemplate()
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
