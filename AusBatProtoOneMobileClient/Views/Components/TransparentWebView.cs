using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AusBatProtoOneMobileClient.Views.Components
{
    public class TransparentWebView : WebView
    {

        public static readonly BindableProperty FontSizePercentageProperty = BindableProperty.Create("FontSizePercentage", typeof(float), typeof(TransparentWebView), 100.0f);

        public float FontSizePercentage
        {
            get { return (float)GetValue(FontSizePercentageProperty); }
            set { SetValue(FontSizePercentageProperty, value); }
        }

    }
}
