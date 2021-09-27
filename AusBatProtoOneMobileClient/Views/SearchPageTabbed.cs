using AusBatProtoOneMobileClient.ViewModels;
using DocGenOneMobileClient.Views;
using Mobile.Helpers;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;

namespace AusBatProtoOneMobileClient
{
    public class SearchPageTabbed : Xamarin.Forms.TabbedPage
    {
        SearchPageViewModel viewModel;
        MenuGenerator menu;

        SearchCriteriaTabPage SearchPageCriteriaTabPage;
        SearchResultsTabPage SearchPageResultsTabPage;

        public SearchPageTabbed(SearchPageViewModel viewModel)
        {
            this.viewModel = viewModel;

            Title = "Search";
            SearchPageCriteriaTabPage = new SearchCriteriaTabPage(viewModel);
            SearchPageResultsTabPage = new SearchResultsTabPage(viewModel);

            Children.Add(SearchPageCriteriaTabPage);
            Children.Add(SearchPageResultsTabPage);

            BarBackgroundColor = Xamarin.Forms.Color.Black;
            BarTextColor = Xamarin.Forms.Color.White;

            menu = new MenuGenerator().Configure()
                .AddMenuItem("back", "Back", Xamarin.Forms.ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(null); }, iconPath: "ic_back.png");

            menu.GenerateToolbarItemsForPage(this);
            menu.SetBinding(MenuGenerator.InvalidateCommandProperty, new Xamarin.Forms.Binding(nameof(SearchPageViewModel.InvalidateMenu), Xamarin.Forms.BindingMode.OneWayToSource, source: viewModel));          

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
