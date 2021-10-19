using AusBatProtoOneMobileClient.ViewModels;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using Mobile.Helpers;
using Mobile.ViewModels;
using Xamarin.Forms;


namespace AusBatProtoOneMobileClient
{
    public class DisplayImagePage : ContentPageBase
    {
        DisplayImagePageViewModel viewModel;
        MenuGenerator menu;
        public DisplayImagePage(DisplayImagePageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            var image = new CachedImage
            {
                Aspect = Aspect.AspectFit,
                ErrorPlaceholder = "bat.png"
            };
            image.SetBinding(CachedImage.SourceProperty, new Binding(nameof(DisplayImagePageViewModel.ImageSource), BindingMode.OneWay));

            Title = "Image display";
            var centeredLayout = new AbsoluteLayout
            {
                Children = { image, activityIndicator },
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = 5
            };
            AbsoluteLayout.SetLayoutFlags(image, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(image, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(activityIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(activityIndicator, new Rectangle(0.5, .5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));


            Content = centeredLayout;
            BackgroundColor = Color.Black;

            menu = new MenuGenerator().Configure()
                .AddMenuItem("back", "Back", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(null); });

            menu.GenerateToolbarItemsForPage(this);
            
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
