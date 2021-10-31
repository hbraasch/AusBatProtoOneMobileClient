using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AusBatProtoOneMobileClient.Droid;
using AusBatProtoOneMobileClient.Views.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(TransparentWebView), typeof(TransparentWebViewDroid))]
namespace AusBatProtoOneMobileClient.Droid
{
    public class TransparentWebViewDroid : WebViewRenderer
    {
        const int defaultFontSize = 10;
        public TransparentWebViewDroid(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            // Setting the background as transparent
            Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
            var fontSizePercentage = ((TransparentWebView)e.NewElement).FontSizePercentage;
            var percentage = (fontSizePercentage == 0) ? 100.0f: fontSizePercentage;
            Control.Settings.DefaultFontSize = (int)(defaultFontSize * (percentage) /100);
        }
    }
}