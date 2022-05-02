using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AusBatProtoOneMobileClient.Models
{
    public class Constants
    {
        public const string APP_NAME = "AusBat Fieldguide";
        public const float MASTER_IMAGE_SCALE_FACTOR = 10;
        public static Color APP_COLOUR = Color.FromHex("e6ae0f");  // 6D5110
        public static Color SPECIES_COLOUR = Color.White;
        public static Color COMMON_NAME_COLOUR = Color.LightGray.MultiplyAlpha(0.8);
        public static Color IMAGE_CIRCLE_COLOUR = APP_COLOUR;
        public static int IMAGE_CIRCLE_BORDER_SIZE = 10;
        public const string BACKGROUND_IMAGE = "background.png";
        public const float NUMERIC_ENTRY_TRIGGER_DELAY_IN_SEC = 2.5f;

        public const string HIRES_IMAGES_FOLDER_NAME = "HiresImages";
        public const string HIRES_IMAGES_ZIP_FILE_NAME = "hires_images.zip";

        public static float MEDIUM_FONT_HEIGHT_MM = (DeviceInfo.Platform == DevicePlatform.Android) ? 3.0f : 3.0f; // Measured on reference device
        public static float HTML_FONT_REFERENCE_HEIGHT_MM = (DeviceInfo.Platform == DevicePlatform.Android) ? 1.4f : 1.0f; // Reference HTML text measured at 100% on reference device


    }
}
