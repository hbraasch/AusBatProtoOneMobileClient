using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AusBatProtoOneMobileClient.Models
{
    public class Styles
    {
        public static Style ContentPageStyle = new Style(typeof(ContentPage))
        {
            Setters = {
                new Setter { Property = ContentPage.BackgroundImageSourceProperty,   Value = "background.png" }
            }
        };
        public static Style NavigationPageStyle = new Style(typeof(NavigationPage))
        {
            Setters = {
                new Setter { Property = NavigationPage.BarBackgroundColorProperty,   Value = Color.Black },
                new Setter { Property = NavigationPage.BarTextColorProperty,   Value = Color.White }
            }
        };

        public static Style RoundedButtonStyle = new Style(typeof(Button))
        {
            Setters = {
                new Setter { Property = Button.FontSizeProperty,   Value = Device.GetNamedSize(NamedSize.Medium, typeof(Button)) },
                new Setter { Property = Button.BorderWidthProperty,   Value = 3 },
                new Setter { Property = Button.BorderColorProperty,   Value = Color.FromHex("6D5110")},
                new Setter { Property = Button.CornerRadiusProperty,   Value = 5 }
            }
        };
    }
}
