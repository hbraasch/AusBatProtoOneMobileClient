using Mobile.Helpers;
using PropertyChanged;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mobile.ViewModels
{
    // For a given property, if there is a method of the form On<PropertyName>Changed, then that method will be called

    public class ViewModelBase : INotifyPropertyChanged 
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public Func<ContentPageBase> ProvidePage;

        public ContentPageBase Page => ProvidePage.Invoke();

        public bool ActivityIndicatorIsActive { get; set; }

        public Action CancelClickCommand { get; set; }

        public string ActivityIndicatorPrompt { get; set; }

        public Func<MenuGenerator> ProvideMenuGenerator;

        private MenuGenerator menu;



        public MenuGenerator Menu {
            get 
            {
                if (menu == null) throw  new ApplicationException("ViewModel 'MenuGenerator' property has not been set in associated Page");
                return menu;
            }
            set 
            {
                menu = value;
            }
        }

        public TaskCompletionSource<NavigateReturnType> TaskCompletionSource { get; set; }


        /// <summary>
        /// Used to start the async operation of a page display
        /// </summary>
        /// <returns>returns false if a cancellation occured</returns>
        private async Task<NavigateReturnType> ExecutionStarts()
        {
            TaskCompletionSource = new TaskCompletionSource<NavigateReturnType>();
            return await TaskCompletionSource.Task;
        }

        public enum NavigateReturnType
        {
            IsCancelled, IsAccepted, GotoHome, GotoFilterReset
        }

        /// <summary>
        /// Stops the await on this async task. Returns a value to indicate if user has Accepted or Cancelled
        /// </summary>
        /// <param name="isAccepted"></param>
        private void ExecutionStops(NavigateReturnType isAccepted = NavigateReturnType.IsAccepted)
        {
            if (TaskCompletionSource != null)
            {
                TaskCompletionSource.SetResult(isAccepted);
                TaskCompletionSource = null;
            }
        }

        public bool IsPageReturnable { get; set; } = false;

        /// <summary>
        /// Used to navigate to a new page.
        /// Also keeps track of which page is current, and calls an optional listener when returning
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="toPage"></param>
        /// <param name="toPageViewModel"></param>
        /// <param name="onReturnFromPageListener">By defining a common delegate, allows one to do the same action on all returns</param>
        /// <returns></returns>
        internal async Task<NavigateReturnType> NavigateToPageAsync(Page toPage, ViewModelBase toPageViewModel) 
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                NavigationPage.SetHasBackButton(toPage, false);
            }
            else
            {
                NavigationPage.SetHasBackButton(toPage, true);
            }
            // Async navigation operation
            Debug.WriteLine($"Navigated to: {toPage.GetType().Name}");
            toPageViewModel.IsPageReturnable = true;
            var navigationPage = new NavigationPage(toPage) { BarBackgroundColor = Color.Black, BarTextColor = Color.White };
            await Page.Navigation.PushModalAsync(navigationPage, false);
            var result = await toPageViewModel.ExecutionStarts();
            if (Page.Navigation.ModalStack.Count > 0)
            {
                await Page.Navigation.PopModalAsync();
            }
         
            return result;
        }

   

        internal void NavigateBack(NavigateReturnType isAccepted = NavigateReturnType.IsCancelled)
        {
            ExecutionStops(isAccepted);
        }

        public void ActivityIndicatorStart(Action onCancel)
        {
            CancelClickCommand = onCancel;
            ActivityIndicatorIsActive = true;
        }
        public async Task ActivityIndicatorStart(string prompt)
        {
            ActivityIndicatorPrompt = prompt;
            ActivityIndicatorIsActive = true;
            await DoEvents();
        }

        public void ActivityIndicatorStart(string prompt, Action onCancel)
        {
            ActivityIndicatorPrompt = prompt;
            CancelClickCommand = onCancel;
            ActivityIndicatorIsActive = true;
        }

        public async Task ActivityIndicatorStart()
        {
            ActivityIndicatorIsActive = true;
            await DoEvents();
        }

        public void ActivityIndicatorStop()
        {
            ActivityIndicatorPrompt = "";
            ActivityIndicatorIsActive = false;
        }

        public void SetActivityIndicatorPrompt (string prompt)
        {
            ActivityIndicatorPrompt = prompt;
        }

        public bool IsActivityIndicatorRunning()
        {
            return Page?.IsActivityIndicatorRunning()?? false;

        }

        public async Task DisplayAlert(string title, string message, string cancel)
        {
            await Page?.DisplayAlert(title, message, cancel);
        }

        public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            return await Page?.DisplayAlert(title, message, accept, cancel);
        }

        public static Task DoEvents()
        {
            var tcs = new TaskCompletionSource<object>();
            Task.Factory.StartNew(() => {
                Thread.Sleep(1);
                tcs.SetResult(null);
            });
            return tcs.Task;
        }
    }
}
