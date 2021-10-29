using AusBatProtoOneMobileClient.Models;
using AusBatProtoOneMobileClient.ViewModels;
using AusBatProtoOneMobileClient.Views.Components;
using FFImageLoading.Forms;
using Mobile.Helpers;
using Mobile.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace AusBatProtoOneMobileClient
{
    public class DisplaySpeciesDistributionTabPage : ContentPageBase
    {
        DisplaySpeciesTabbedPageViewModel viewModel;

        public DisplaySpeciesDistributionTabPage(DisplaySpeciesTabbedPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

            var image = new CachedImage
            {
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            image.WidthRequest = mainDisplayInfo.Width;
            image.SetBinding(CachedImage.SourceProperty, new Binding(nameof(DisplaySpeciesTabbedPageViewModel.DistributionMapImage), BindingMode.OneWay));

            var centeredLayout = new AbsoluteLayout
            {
                Children = { image },
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            AbsoluteLayout.SetLayoutFlags(image, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(image, new Rectangle(0, 0, 1, 1));

            NavigationPage.SetTitleView(this, new Label { Text = "Distribution", Style = Styles.TitleLabelStyle });
            BackgroundColor = Color.Black;
            Content = centeredLayout;
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
