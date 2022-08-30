using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AusBatProtoOneMobileClient.Models
{
    // For Android. Helper to assist with providing additional data, the APK file exceeds 100 MB. In our case we need to upload the
    // [hires_images.zip] file as a APK extension file to the Google Play store. Read here for details...
    // https://docs.microsoft.com/en-us/xamarin/android/deploy-test/publishing/publishing-to-google-play/apk-expansion-files
    // https://developer.android.com/google/play/expansion-files
    // When using [App Center] to distribute Android apps, the 100 MB limit does not exist, so the [hires_images.zip] can be added to the APK as a embedded resource file
    // The app version code must be increased with each APK upload to Google Play

    // This size limitation does not apply to iOS 
    public interface IApkHelper
    {
        void LoadApkExpansionFile(string destFullFilename);
        long GetVersionCode();
    }

    public class ApkHelper
    {
        /// <summary>
        /// Used to copy and rename the locally installed APK extension file to the app working directory, so it can be unzipped.
        /// </summary>
        /// <param name="destFullFilename"></param>
        public static void LoadApkExpansionFile(string destFullFilename) => DependencyService.Get<IApkHelper>().LoadApkExpansionFile(destFullFilename);
        public static long GetVersionCode() => DependencyService.Get<IApkHelper>().GetVersionCode();
    }
}
