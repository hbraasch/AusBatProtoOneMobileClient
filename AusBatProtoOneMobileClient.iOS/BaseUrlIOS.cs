using AusBatProtoOneMobileClient.Helpers;
using AusBatProtoOneMobileClient.iOS;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(BaseUrl_iOS))]
namespace AusBatProtoOneMobileClient.iOS
{
    // https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/webview?tabs=macos
    public class BaseUrl_iOS : IBaseUrl
    {
        public string Get()
        {
            return NSBundle.MainBundle.BundlePath;
        }
    }


}