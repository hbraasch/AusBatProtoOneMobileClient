using AusBatProtoOneMobileClient.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using TreeApp.Helpers;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Mobile.Models
{
    public class Settings
    {
        internal static void Clear()
        {

        }

        public static DbaseVersion CurrentDataVersion 
        { 
            get 
            {
                var versionJson = Preferences.Get("DataVersion", "");
                if (versionJson == "") return DbaseVersion.MinValue;
                return JsonConvert.DeserializeObject<DbaseVersion>(versionJson);
            } 
            set 
            {
                var versionJson = JsonConvert.SerializeObject(value);
                Preferences.Set("DataVersion", versionJson); 
            } 
        }

        public static float HtmlFontSizePercentage
        {
            get
            {
                return Preferences.Get("HtmlFontSizePercentage", (DeviceInfo.Platform == DevicePlatform.Android) ? 100.0f : 300.0f);
            }
            set
            {
                Preferences.Set("HtmlFontSizePercentage", value); ;
            }
        }
    }
}
