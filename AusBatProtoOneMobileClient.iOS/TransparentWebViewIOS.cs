using AusBatProtoOneMobileClient.iOS;
using AusBatProtoOneMobileClient.Views.Components;
using Foundation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UIKit;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TransparentWebView), typeof(TransparentWebViewIOS))]
namespace AusBatProtoOneMobileClient.iOS
{

    // https://social.msdn.microsoft.com/Forums/en-US/208a721c-67cd-46a2-a83a-d1e7e7d3aa7b/xamarin-forms-how-to-increase-the-fontsize-of-content-in-webview?forum=xamarinforms
    // https://www.c-sharpcorner.com/article/xamarin-forms-enable-default-zooming-in-webview/
    public class TransparentWebViewIOS : WkWebViewRenderer
    {

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            // Setting the background as transparent
            this.Opaque = false;
            this.BackgroundColor = Color.Transparent.ToUIColor();

            var data = ((TransparentWebView)e.NewElement);
            if (data != null) this.NavigationDelegate = new NavigationDelegat(data);
        }
       
        public class NavigationDelegat : WKNavigationDelegate
        {
            TransparentWebView data;
            public NavigationDelegat(TransparentWebView data)
            {
                this.data = data;
            }

            public override void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
            {

                if (webView.Url.AbsoluteString.Contains("measurement"))
                {
                    var index = ExtractIndex(webView.Url.AbsoluteString);
                    data?.OnMeasurementClickedIOS.Invoke(index);
                    return;
                }

                string fontSize = (data.FontSizePercentage == 0)? $"100%": $"{data.FontSizePercentage}%"; 
                string jscript = String.Format(@"document.getElementsByTagName('body')[0].style.webkitTextSizeAdjust= '{0}'", fontSize);
                WKJavascriptEvaluationResult handler = (NSObject result, NSError err) => {
                    if (err != null)
                    {
                        System.Console.WriteLine(err);
                    }
                    if (result != null)
                    {
                        System.Console.WriteLine(result);
                    }
                };
                webView.EvaluateJavaScript(jscript, handler);

                // Exit

                int ExtractIndex(string url)
                {
                    var firstSplit = url.Split('?');
                    if (firstSplit.Length != 2) throw new ApplicationException("Url is incomplete");
                    var secondSplit = firstSplit[1].Split('=');
                    if (firstSplit.Length != 2) throw new ApplicationException("Url has no index parameter");
                    var success = int.TryParse(secondSplit[1], out var value);
                    if (!success) throw new ApplicationException("Url index parameter is not a number");
                    return value;
                }
            }

            public override void DidFailNavigation(WKWebView webView, WKNavigation navigation, NSError error)
            {
                Debug.WriteLine("Failed navigation");
            }
        }
    }
}