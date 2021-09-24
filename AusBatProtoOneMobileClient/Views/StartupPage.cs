using AusBatProtoOneMobileClient.Models;
using AusBatProtoOneMobileClient.ViewModels;
using Mobile.Helpers;
using Mobile.ViewModels;
using Xamarin.Forms;


namespace AusBatProtoOneMobileClient
{
    public class StartupPage : ContentPageBase
    {
        StartupPageViewModel viewModel;
        MenuGenerator menu;
        public StartupPage(StartupPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            var introButton = new Button
            {
                Text = "Introduction",
                Style = Styles.RoundedButtonStyle,
                BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5)
            };

            var keyToFamiliesButton = new Button
            {
                Text = "Key to Families",
                Style = Styles.RoundedButtonStyle,
                BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5)
            };
            keyToFamiliesButton.Clicked += (s, e) => { viewModel.OnClassificationClicked.Execute(null); };

            var identificationKeysButton = new Button
            {
                Text = "Identification Keys",
                Style = Styles.RoundedButtonStyle,
                BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5)
            };
            identificationKeysButton.Clicked += (s, e) => { viewModel.OnIdentificationKeysClicked.Execute(null); };

            var searchButton = new Button
            {
                Text = "Search",
                Style = Styles.RoundedButtonStyle,
                BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5)
            };

            var grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                },
                ColumnDefinitions = 
                { 
                    new ColumnDefinition{Width = new GridLength(1, GridUnitType.Auto) }
                },
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions =LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.Transparent
            };
            grid.Children.Add(introButton, 0, 0);
            grid.Children.Add(keyToFamiliesButton, 0, 1);
            grid.Children.Add(identificationKeysButton, 0, 2);
            grid.Children.Add(searchButton, 0, 3);

            var mainLayout = new StackLayout { Children = { grid }, VerticalOptions = LayoutOptions.Center };

            var backgroundImage = new Image { Aspect = Aspect.AspectFill, Source = "background.png" };

            Title = Constants.APP_NAME;
            BackgroundImageSource = "background.png";

            var centeredLayout = new AbsoluteLayout
            {
                Children = {backgroundImage, grid, activityIndicator },
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            AbsoluteLayout.SetLayoutFlags(backgroundImage, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(backgroundImage, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(grid, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(grid, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(activityIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(activityIndicator, new Rectangle(0.5, .5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));


            Content = centeredLayout;

            menu = new MenuGenerator().Configure()
                .AddMenuItem("test", "Test", ToolbarItemOrder.Secondary, (menuItem) => { viewModel.OnGenerateMockDataPressed.Execute(null); }, iconPath: "ic_check.png");
                

            menu.SetVisibilityFactors(viewModel, "IsLoggedIn")
                .ToShowMenuItem("test", null);

            menu.GenerateToolbarItemsForPage(this);
            menu.SetBinding(MenuGenerator.InvalidateCommandProperty, new Binding(nameof(StartupPageViewModel.InvalidateMenuCommand), BindingMode.OneWayToSource, source: viewModel));
            
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
