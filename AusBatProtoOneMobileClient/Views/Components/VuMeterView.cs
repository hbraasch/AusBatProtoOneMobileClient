using AusBatProtoOneMobileClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AusBatProtoOneMobileClient.Views.Components
{
    public class VuMeterView: Frame
    {
        public static readonly BindableProperty ValueProperty = BindableProperty.Create("Value", typeof(double), typeof(VuMeterView), 0.0d, propertyChanged: OnValueChanged);

        private static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var context = bindable as VuMeterView;
            if (context.Value == 0)
            {
                context.ClearProgressBar();
            }
            else
            {
                context.SetProgressBar((double) newValue);
            }
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly BindableProperty IsIsDisplayingProperty = BindableProperty.Create("IsDisplaying", typeof(bool), typeof(VuMeterView), true);


        public bool IsDisplaying
        {
            get { return (bool)GetValue(IsIsDisplayingProperty); }
            set { SetValue(IsIsDisplayingProperty, value); }
        }

        ProgressBar progressBar;
        public VuMeterView()
        {
            BindingContext = this;

            progressBar = new ProgressBar { HorizontalOptions = LayoutOptions.CenterAndExpand, WidthRequest = 500, ProgressColor = Color.Yellow, Margin = 5 };
            progressBar.SetBinding(ProgressBar.ProgressProperty, new Binding(nameof(Value), BindingMode.OneWay));

            ClearProgressBar();

            BorderColor = Constants.APP_COLOUR;
            CornerRadius = 5;
            Margin = 5;
            BackgroundColor = Color.Transparent;
            SetBinding(Frame.IsVisibleProperty, new Binding(nameof(IsDisplaying), BindingMode.OneWay));

            Content = progressBar;
        }

        void ClearProgressBar()
        {
            progressBar.ProgressColor = Color.DarkGray;
            progressBar.Progress = 1;
        }

        void SetProgressBar(double value)
        {
            progressBar.ProgressColor = Color.Yellow;
            progressBar.Progress = value;
        }
    }
}
