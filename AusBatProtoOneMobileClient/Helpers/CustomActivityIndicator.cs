using AusBatProtoOneMobileClient.Models;
using PropertyChanged;
using System;
using System.Threading;
using Xamarin.Forms;

namespace Mobile.Shared
{
    public class CustomActivityIndicator : Frame
    {


        /// <summary>
        /// Used to make visible or invisible
        /// </summary>
        public static readonly BindableProperty IsActiveProperty =
                    BindableProperty.Create(nameof(IsActive), typeof(bool), typeof(CustomActivityIndicator), false, propertyChanged: OnIsActiveChanged);
        
        [SuppressPropertyChangedWarnings]
        private static void OnIsActiveChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var context = (CustomActivityIndicator)bindable;
            context.SetActive((bool)newValue);
        }

        public bool IsActive
        {
            get { return (bool)base.GetValue(IsActiveProperty); }
            set { 
                base.SetValue(IsActiveProperty, value);
                SetActive(value);           
            }
        }

        public static BindableProperty PromptProperty = BindableProperty.Create(nameof(Prompt), typeof(string), typeof(CustomActivityIndicator), "", BindingMode.OneWay, propertyChanged: OnPromptChanged);

        [SuppressPropertyChangedWarnings]
        private static void OnPromptChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var context = (CustomActivityIndicator)bindable;
            if (!string.IsNullOrEmpty((string)newValue))
            {
                context.prompt.Text = (string)newValue;
                context.prompt.IsVisible = true; 
            }
            else
            {
                context.prompt.IsVisible = false;
            }
        }

        public string Prompt
        {
            get { return (string)GetValue(PromptProperty); }
            set { SetValue(PromptProperty, value); }
        }

        private Label prompt;
        private ActivityIndicator activityIndicator;
        private Button button;

        public static BindableProperty CancelCommandProperty = BindableProperty.Create(nameof(CancelCommand), typeof(Action), typeof(CustomActivityIndicator), null, BindingMode.OneWayToSource);

        public Action CancelCommand
        {
            get { return (Action)GetValue(CancelCommandProperty); }
            set { SetValue(CancelCommandProperty, value); }
        }


        public CustomActivityIndicator()
        {
            VerticalOptions = LayoutOptions.Center;
            HorizontalOptions = LayoutOptions.Fill;
            BackgroundColor = BackgroundColor = Color.Gray;
            BorderColor = Constants.APP_COLOUR;
            HasShadow = false;
            CornerRadius = 5;

            prompt = new Label() { IsVisible = false, HorizontalOptions = LayoutOptions.Center, TextColor = Color.White };

            activityIndicator = new ActivityIndicator
            {
                Color = Color.Black,
                HorizontalOptions = LayoutOptions.Center,
            };


            button = new Button()
            {
                Text = "Cancel",
                TextColor = Color.Black,
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Button)),
                HorizontalOptions = LayoutOptions.Center,
                BackgroundColor = Color.Silver,
                BorderWidth = 0,
            };
            button.Clicked += (sender, e) =>
            {
                CancelCommand?.Invoke();
            };

            Content = GenerateLayout();

            SetActive(IsActive);

        }

        private StackLayout GenerateLayout()
        {
            return new StackLayout { Children = { prompt, activityIndicator }, Margin = 5 };
        }

        private void SetActive(bool isActive)
        {
            if (isActive)
            {
                activityIndicator.IsRunning = true;
                activityIndicator.IsVisible = true;
                button.IsVisible = true;
                IsVisible = true;
            }
            else
            {
                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;
                button.IsVisible = false;
                IsVisible = false;
            }
        }
        
    }
}

