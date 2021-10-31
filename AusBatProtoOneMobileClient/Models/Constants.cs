﻿using System;
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
        public const string BACKGROUND_IMAGE = "background.png";
        public const float NUMERIC_ENTRY_TRIGGER_DELAY_IN_SEC = 2.5f;

        public const string HIRES_IMAGES_FOLDER_NAME = "HiresImages";
        public const string HIRES_IMAGES_ZIP_FILE_NAME = "hires_images.zip";

        public static float HTML_FONT_SIZE_PERCENTAGE = (DeviceInfo.Platform == DevicePlatform.Android) ? 150.0f: 300.0f;

    }
}
