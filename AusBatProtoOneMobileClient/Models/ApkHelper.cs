using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AusBatProtoOneMobileClient.Models
{
    // https://docs.microsoft.com/en-us/xamarin/android/deploy-test/publishing/publishing-to-google-play/apk-expansion-files
    // https://developer.android.com/google/play/expansion-files
    public interface IApkHelper
    {
        void LoadApkExpansionFile(string destFullFilename);
    }

    public class ApkHelper
    {
        public static void LoadApkExpansionFile(string destFullFilename) => DependencyService.Get<IApkHelper>().LoadApkExpansionFile(destFullFilename);
    }
}
