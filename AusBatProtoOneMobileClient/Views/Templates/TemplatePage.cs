using AusBatProtoOneMobileClient.ViewModels;
using Mobile.Helpers;
using Mobile.ViewModels;
using Xamarin.Forms;


namespace AusBatProtoOneMobileClient
{
    public class TemplatePage : ContentPageBase
    {
        TemplatePageViewModel viewModel;
        MenuGenerator menu;
        public TemplatePage(TemplatePageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            var mainLayout = new StackLayout { Children = { } };

            Title = "Template";
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


            Content = centeredLayout;

            menu = new MenuGenerator().Configure()
                .AddMenuItem("back", "Back", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(null); }, iconPath: "ic_back.png")
                .AddMenuItem("test", "Test", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnTestMenuPressed.Execute(null); }, iconPath: "ic_check.png");
                

            menu.SetVisibilityFactors(viewModel, "IsLoggedIn")
                .ToShowMenuItem("test", null);

            menu.GenerateToolbarItemsForPage(this);
            menu.SetBinding(MenuGenerator.InvalidateCommandProperty, new Binding(nameof(TemplatePageViewModel.InvalidateMenuCommand), BindingMode.OneWayToSource, source: viewModel));
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
