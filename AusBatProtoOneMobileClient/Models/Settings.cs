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

        public static double FontSize { get { return Preferences.Get("FontSize", Device.GetNamedSize(NamedSize.Small,typeof(Label))); } set { Preferences.Set("FontSize", (double) value); } }

        

        internal static List<int> LoadPersistedListItems(string key)
        {
            var persistedExpansionItemsJson = Preferences.Get(key, "");
            if (string.IsNullOrEmpty(persistedExpansionItemsJson)) return new List<int>();
            return JsonConvert.DeserializeObject<List<int>>(persistedExpansionItemsJson);
        }

        internal static void SavePersistedListItems(string key, List<int> values)
        {
            var persistedExpansionItemsJson = JsonConvert.SerializeObject(values);
            Preferences.Set(key, persistedExpansionItemsJson);
        }
    }
}
