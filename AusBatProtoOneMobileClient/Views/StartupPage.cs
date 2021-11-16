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
            introButton.Clicked += (s, e) => { viewModel.OnIntroButtonClicked.Execute(null); };

            var familyKeyButton = new Button
            {
                Text = "Key to Families",
                Style = Styles.RoundedButtonStyle,
                BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5)
            };
            familyKeyButton.Clicked += (s, e) => { viewModel.OnFamilyKeyButtonClicked.Execute(null); };

            var generaKeyButton = new Button
            {
                Text = "Keys to Genera and Species",
                Style = Styles.RoundedButtonStyle,
                BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5)
            };
            generaKeyButton.Clicked += (s, e) => { viewModel.OnGeneraKeyButtonClicked.Execute(null); };

            var speciesAtozButton = new Button
            {
                Text = "Species A-Z",
                Style = Styles.RoundedButtonStyle,
                BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5)
            };
            speciesAtozButton.Clicked += (s, e) => { viewModel.OnSpeciesAtoZClicked.Execute(null); };

            var speciesByFamilyButton = new Button
            {
                Text = "Species by Family",
                Style = Styles.RoundedButtonStyle,
                BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5)
            };
            speciesByFamilyButton.Clicked += (s, e) => { viewModel.OnSpeciesByFamilyClicked.Execute(null); };

            var areaListingButton = new Button
            {
                Text = "Area Listings",
                Style = Styles.RoundedButtonStyle,
                BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5)
            };
            areaListingButton.Clicked += (s, e) => { viewModel.OnAreaListingsClicked.Execute(null); };

            var aboutButton = new Button
            {
                Text = "About",
                Style = Styles.RoundedButtonStyle,
                BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5)
            };
            aboutButton.Clicked += (s, e) => { viewModel.OnAboutButtonClicked.Execute(null); };



            var grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
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
            grid.Children.Add(familyKeyButton, 0, 1);
            grid.Children.Add(generaKeyButton, 0, 2);
            grid.Children.Add(speciesAtozButton, 0, 3);
            grid.Children.Add(speciesByFamilyButton, 0, 4);
            grid.Children.Add(areaListingButton, 0, 5);
            grid.Children.Add(aboutButton, 0, 6);

            var mainLayout = new StackLayout { Children = { grid }, VerticalOptions = LayoutOptions.Center };

            var backgroundImage = new Image { Aspect = Aspect.AspectFill, Source = Constants.BACKGROUND_IMAGE };

            BackgroundColor = Color.Black;
            NavigationPage.SetTitleView(this, new Label { Text = Constants.APP_NAME, Style = Styles.TitleLabelStyle });

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
                .AddMenuItem("sightings", "Sightings", ToolbarItemOrder.Secondary, (menuItem) => { viewModel.OnSightingsPressed.Execute(null); })
                .AddMenuItem("dev", "Development", ToolbarItemOrder.Secondary, (menuItem) => { viewModel.OnSightingsPressed.Execute(null); })
                .AddSubMenuItem("dev", "init", "Init", ToolbarItemOrder.Secondary, (menuItem) => { viewModel.OnInitPressed.Execute(null); });

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
