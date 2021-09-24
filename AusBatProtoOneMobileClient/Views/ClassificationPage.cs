using AusBatProtoOneMobileClient.ViewModels;
using Mobile.Helpers;
using Mobile.Models;
using Mobile.ViewModels;
using TreeApps.Models;
using Xamarin.Forms;


namespace AusBatProtoOneMobileClient
{
    public class ClassificationPage<T> : ContentPageBase where T : ClassificationPageViewModel.DisplayItem
    {
        ClassificationPageViewModel viewModel;
        MenuGenerator menu;
        public ClassificationPage(ClassificationPageViewModel viewModel) : base(viewModel) 
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            var treeview = new GhostTreeview<T>
            {
                IsSelectionSingle = true,
                FontSize = Settings.FontSize,
                FontColor = Color.White
            };
            treeview.SetBinding(GhostTreeview<T>.ItemsSourceProperty, new Binding(nameof(ClassificationPageViewModel.DisplayItems), BindingMode.TwoWay, source: viewModel));
            treeview.SetBinding(GhostTreeview<T>.SelectedItemProperty, new Binding(nameof(ClassificationPageViewModel.SelectedItem), BindingMode.TwoWay, source: viewModel));
            treeview.SetBinding(GhostTreeview<T>.ExpandedItemsProperty, new Binding(nameof(ClassificationPageViewModel.ExpandedItems), BindingMode.TwoWay, source: viewModel));
            treeview.SetBinding(GhostTreeview<T>.InvalidateCommandProperty, new Binding(nameof(ClassificationPageViewModel.InvalidateTreeviewCommand), BindingMode.OneWayToSource, source: viewModel));

            var actionButton = new ImageButton { Source = "ic_select.png", BackgroundColor = Color.Transparent };
            actionButton.SetBinding(ImageButton.IsVisibleProperty, new Binding(nameof(ClassificationPageViewModel.IsSpeciesSelected), BindingMode.OneWay));
            actionButton.Clicked += (s, e) => { viewModel.OnSelectClicked.Execute(true); };

            Title = "Classification";
            BackgroundColor = Color.Black;
            var centeredLayout = new AbsoluteLayout
            {
                Children = { treeview, actionButton, activityIndicator },
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = 10
            };
            AbsoluteLayout.SetLayoutFlags(treeview, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(treeview, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(actionButton, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(actionButton, new Rectangle(0.95, .95, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(activityIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(activityIndicator, new Rectangle(0.5, .5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));


            Content = centeredLayout;

            menu = new MenuGenerator().Configure()
                .AddMenuItem("back", "Back", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(null); }, iconPath: "ic_back.png")
                .AddMenuItem("test", "Test", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnTestMenuPressed.Execute(null); }, iconPath: "ic_check.png");
                

            menu.SetVisibilityFactors(viewModel, "IsLoggedIn")
                .ToShowMenuItem("test", null);

            menu.GenerateToolbarItemsForPage(this);
            menu.SetBinding(MenuGenerator.InvalidateCommandProperty, new Binding(nameof(ClassificationPageViewModel.InvalidateMenuCommand), BindingMode.OneWayToSource, source: viewModel));
            menu.FindMenuUnit("back").isVisible = viewModel.IsPageReturnable;
            
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
    }
}
