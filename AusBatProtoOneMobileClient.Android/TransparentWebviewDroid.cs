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
    // https://social.msdn.microsoft.com/Forums/en-US/208a721c-67cd-46a2-a83a-d1e7e7d3aa7b/xamarin-forms-how-to-increase-the-fontsize-of-content-in-webview?forum=xamarinforms
    // https://www.c-sharpcorner.com/article/xamarin-forms-enable-default-zooming-in-webview/
    public class TransparentWebViewDroid : WebViewRenderer
    {
        const int defaultFontSize = 10;
        TransparentWebView data;
        public TransparentWebViewDroid(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            // Setting the background as transparent
            Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
            Control.Settings.AllowUniversalAccessFromFileURLs = true;
            Control.Settings.SetSupportZoom(true);
            Control.Settings.BuiltInZoomControls = true;

            data = ((TransparentWebView)e.NewElement);
            var fontSizePercentage = data.FontSizePercentage;
            var percentage = (fontSizePercentage == 0) ? 100.0f: fontSizePercentage;

            Control.Settings.DefaultFontSize = (int)(defaultFontSize * (percentage) /100);

            e.NewElement.ReloadRequested += OnReloadRequested;
        }

        private void OnReloadRequested(object sender, EventArgs e)
        {
            Control.Settings.DefaultFontSize = (int)(defaultFontSize * (data.FontSizePercentage) / 100);
        }
    }
}