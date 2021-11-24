using AusBatProtoOneMobileClient.Models;
using AusBatProtoOneMobileClient.ViewModels;
using Mobile.Helpers;
using Mobile.ViewModels;
using Xamarin.Forms;


namespace AusBatProtoOneMobileClient
{
    public class DisplayMeasurementsPage : ContentPageBase
    {
        DisplayMeasurementsPageViewModel viewModel;
        MenuGenerator menu;
        public DisplayMeasurementsPage(DisplayMeasurementsPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            var grid = new Grid() { Margin = new Thickness(5, 5, 5, 5), BackgroundColor = Color.Transparent };
            foreach (var row in viewModel.MeasurementsTable.Rows)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }
            foreach (var col in viewModel.MeasurementsTable.Rows[0].Columns)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
            }

            int left = 0, top = 0;
            foreach (var row in viewModel.MeasurementsTable.Rows)
            {
                foreach (var col in row.Columns)
                {
                    if (left == 0 && top == 0) { left++; continue; };
                    grid.Children.Add(new Label { Text = col.Value, TextColor = Color.White, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) }, left++, top);
                }
                left = 0;
                top++;
            }

            var mainLayout = new StackLayout { Children = { grid }, VerticalOptions = LayoutOptions.CenterAndExpand };

            var centeredLayout = new AbsoluteLayout
            {
                Children = { mainLayout, activityIndicator },
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = 5
            };
            AbsoluteLayout.SetLayoutFlags(mainLayout, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(mainLayout, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(activityIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(activityIndicator, new Rectangle(0.5, .5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            NavigationPage.SetTitleView(this, new Xamarin.Forms.Label { Text = "Template", Style = Styles.TitleLabelStyle });
            Content = centeredLayout;

            menu = new MenuGenerator().Configure()
                .AddMenuItem("back", "Back", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(null); }, iconPath: "ic_back.png");
                
            menu.GenerateToolbarItemsForPage(this);
            menu.SetBinding(MenuGenerator.InvalidateCommandProperty, new Binding(nameof(DisplayMeasurementsPageViewModel.InvalidateMenuCommand), BindingMode.OneWayToSource, source: viewModel));
            menu.FindMenuUnit("back").isVisible = viewModel.IsPageReturnable;
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.Appearance.Execute(null);
        }

        protected override bool OnBackButtonPressed()
        {
            viewModel.OnBackButtonPressed.Execute(null);
            return viewModel.isBackCancelled;
        }
    }
}
