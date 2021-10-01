using AusBatProtoOneMobileClient.ViewModels;
using Mobile.Helpers;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;

namespace AusBatProtoOneMobileClient
{
    public class DisplayBatTabbed : Xamarin.Forms.TabbedPage
    {
        DisplayBatTabbedPageViewModel viewModel;
        MenuGenerator menu;

        DisplayBatImageTabPage displayBatImagesTabPage;
        DisplayBatDetailsTabPage displayBatDetailsTabPage;
        DisplayBatRegionsTabPage displayBatRegionsTabPage;
        DisplayBatCallTabPage displayBatCallTabPage;
        public DisplayBatTabbed(DisplayBatTabbedPageViewModel viewModel)
        {
            this.viewModel = viewModel;


            Title = viewModel.bat.Name;
            displayBatImagesTabPage = new DisplayBatImageTabPage(viewModel);
            displayBatImagesTabPage.IconImageSource = "ic_photos.png";
            displayBatDetailsTabPage = new DisplayBatDetailsTabPage(viewModel);
            displayBatDetailsTabPage.IconImageSource = "ic_details.png";
            displayBatRegionsTabPage = new DisplayBatRegionsTabPage(viewModel);
            displayBatRegionsTabPage.IconImageSource = "ic_regions.png";
            displayBatCallTabPage = new DisplayBatCallTabPage(viewModel);
            displayBatCallTabPage.IconImageSource = "ic_sounds.png";
            Children.Add(displayBatImagesTabPage);
            Children.Add(displayBatDetailsTabPage);
            if (viewModel.bat.MapRegions.Count != 0)
            {
                Children.Add(displayBatRegionsTabPage); 
            }
            if (viewModel.bat.Calls.Count != 0)
            {
                Children.Add(displayBatCallTabPage); 
            }

            // On<Android>().SetToolbarPlacement(Xamarin.Forms.PlatformConfiguration.AndroidSpecific.ToolbarPlacement.Bottom);
            BarBackgroundColor = Xamarin.Forms.Color.Black;
            BarTextColor = Xamarin.Forms.Color.White;
            
            //On<Windows>().SetHeaderIconsEnabled(true);

            menu = new MenuGenerator().Configure()
                .AddMenuItem("back", "Back", Xamarin.Forms.ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(null); }, iconPath: "ic_back.png");

            menu.GenerateToolbarItemsForPage(this);
            menu.SetBinding(MenuGenerator.InvalidateCommandProperty, new Xamarin.Forms.Binding(nameof(DisplayBatTabbedPageViewModel.InvalidateMenuCommand), Xamarin.Forms.BindingMode.OneWayToSource, source: viewModel));          

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
