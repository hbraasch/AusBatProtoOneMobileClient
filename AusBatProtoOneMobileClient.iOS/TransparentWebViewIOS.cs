using AusBatProtoOneMobileClient.iOS;
using AusBatProtoOneMobileClient.Views.Components;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TransparentWebView), typeof(TransparentWebViewIOS))]
namespace AusBatProtoOneMobileClient.iOS
{
    public class TransparentWebViewIOS : WkWebViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            // Setting the background as transparent
            this.Opaque = false;
            this.BackgroundColor = Color.Transparent.ToUIColor();
        }
    }
}