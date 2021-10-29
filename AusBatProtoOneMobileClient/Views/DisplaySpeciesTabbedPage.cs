using AusBatProtoOneMobileClient.Models;
using AusBatProtoOneMobileClient.ViewModels;
using Mobile.Helpers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;

namespace AusBatProtoOneMobileClient
{
    public class DisplaySpeciesTabbedPage : Xamarin.Forms.TabbedPage
    {
        DisplaySpeciesTabbedPageViewModel viewModel;
        MenuGenerator menu;

        DisplaySpeciesImageTabPage displayBatImagesTabPage;
        DisplaySpeciesDetailsTabPage displayBatDetailsTabPage;
        DisplaySpeciesDistributionTabPage displayBatRegionsTabPage;
        DisplaySpeciesCallTabPage displayBatCallTabPage; 
        public DisplaySpeciesTabbedPage(DisplaySpeciesTabbedPageViewModel viewModel)
        {
            this.viewModel = viewModel;

            displayBatImagesTabPage = new DisplaySpeciesImageTabPage(viewModel);
            displayBatDetailsTabPage = new DisplaySpeciesDetailsTabPage(viewModel);
            displayBatRegionsTabPage = new DisplaySpeciesDistributionTabPage(viewModel);
            displayBatCallTabPage = new DisplaySpeciesCallTabPage(viewModel);


            #region *// Provide icons. IOS does not accept Resizetizer icons. IOS needs them in Resources folder set as [Bundled Resource]
            if (DeviceInfo.Platform != DevicePlatform.iOS)
            {
                displayBatRegionsTabPage.IconImageSource = "world.png";
                displayBatDetailsTabPage.IconImageSource = "text.png";
                displayBatImagesTabPage.IconImageSource = "photos.png";
                displayBatCallTabPage.IconImageSource = "sounds.png";
            }
            else
            {
                displayBatRegionsTabPage.IconImageSource = "world_ios.png";
                displayBatDetailsTabPage.IconImageSource = "text_ios.png";
                displayBatImagesTabPage.IconImageSource = "photos_ios.png";
                displayBatCallTabPage.IconImageSource = "sounds_ios.png";
            }
            #endregion  

            Children.Add(displayBatImagesTabPage);
            Children.Add(displayBatDetailsTabPage);
            Children.Add(displayBatRegionsTabPage);
            if (viewModel.Species.CallImages.Count != 0)
            {
                Children.Add(displayBatCallTabPage); 
            }

            // On<Android>().SetToolbarPlacement(Xamarin.Forms.PlatformConfiguration.AndroidSpecific.ToolbarPlacement.Bottom);
            NavigationPage.SetTitleView(this, new Xamarin.Forms.Label { Text = $"{viewModel.Species.Name}", Style = Styles.TitleLabelStyle });
            BarBackgroundColor = Xamarin.Forms.Color.Black;
            BarTextColor = Xamarin.Forms.Color.White;
            
            //On<Windows>().SetHeaderIconsEnabled(true);

            menu = new MenuGenerator().Configure()
                .AddMenuItem("home", "Home", Xamarin.Forms.ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnHomeMenuPressed.Execute(null); })
                .AddMenuItem("back", "Back", Xamarin.Forms.ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(null); })
                .AddMenuItem("resetFilter", "Reset", Xamarin.Forms.ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnResetMenuPressed.Execute(null); })
                .AddMenuItem("addSighting", "Add sighting", Xamarin.Forms.ToolbarItemOrder.Secondary, (menuItem) => { viewModel.OnAddSightingMenuPressed.Execute(null); });

            menu.SetVisibilityFactors(viewModel, "IsHomeEnabled", "IsResetFilterEnabled")
                .ToShowMenuItem("home", true, null )
                .ToShowMenuItem("resetFilter", null, true);

            menu.GenerateToolbarItemsForPage(this);
            menu.SetBinding(MenuGenerator.InvalidateCommandProperty, new Xamarin.Forms.Binding(nameof(DisplaySpeciesTabbedPageViewModel.InvalidateMenuCommand), Xamarin.Forms.BindingMode.OneWayToSource, source: viewModel));          

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
