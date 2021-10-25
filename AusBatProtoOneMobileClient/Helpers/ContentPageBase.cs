using Mobile.Shared;
using Xamarin.Forms;

namespace Mobile.ViewModels
{
    public class ContentPageBase: ContentPage
    {
        // public ActivityIndicator activityIndicator = new ActivityIndicator { Visual = VisualMarker.Material, IsEnabled = true };
        public CustomActivityIndicator activityIndicator = new CustomActivityIndicator();
        public ContentPageBase(ViewModelBase viewModel)
        {
            viewModel.ProvidePage = () => { return this; };
            activityIndicator.SetBinding(CustomActivityIndicator.IsActiveProperty, new Binding(nameof(ViewModelBase.ActivityIndicatorIsActive), BindingMode.OneWay));
            activityIndicator.SetBinding(CustomActivityIndicator.CancelCommandProperty, new Binding(nameof(ViewModelBase.CancelClickCommand), BindingMode.OneWay));
            activityIndicator.SetBinding(CustomActivityIndicator.PromptProperty, new Binding(nameof(ViewModelBase.ActivityIndicatorPrompt), BindingMode.OneWay));
        }

        public bool IsActivityIndicatorRunning()
        {
            return activityIndicator?.IsActive ?? false;
        }

    }
}
