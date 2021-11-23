﻿using AusBatProtoOneMobileClient.iOS;
using AusBatProtoOneMobileClient.Views.Components;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TransparentWebView), typeof(TransparentWebViewIOS))]
namespace AusBatProtoOneMobileClient.iOS
{
    public class TransparentWebViewIOS : WkWebViewRenderer
    {
        float defaultFontSizePercent = 100;
        static float fontSizePercent = 0;
        TransparentWebView data;
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            // Setting the background as transparent
            this.Opaque = false;
            this.BackgroundColor = Color.Transparent.ToUIColor();

            data = ((TransparentWebView)e.NewElement);
            fontSizePercent = (data == null) ? defaultFontSizePercent : data.FontSizePercentage;

            this.NavigationDelegate = new NavigationDelegat(data);
        }

        // https://social.msdn.microsoft.com/Forums/en-US/208a721c-67cd-46a2-a83a-d1e7e7d3aa7b/xamarin-forms-how-to-increase-the-fontsize-of-content-in-webview?forum=xamarinforms
        public class NavigationDelegat : WKNavigationDelegate
        {
            TransparentWebView data;
            public NavigationDelegat(TransparentWebView data)
            {
                this.data = data;
            }

            public override void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
            {
                string fontSize = (fontSizePercent == 0)? $"100%": $"{data.FontSizePercentage}%"; 
                string stringsss = String.Format(@"document.getElementsByTagName('body')[0].style.webkitTextSizeAdjust= '{0}'", fontSize);
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
                webView.EvaluateJavaScript(stringsss, handler);

            }
        }
    }
}