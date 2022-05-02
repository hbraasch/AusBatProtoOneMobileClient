using AusBatProtoOneMobileClient.IOS;
using AusBatProtoOneMobileClient.Models;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(ApkHelperIOS))]

namespace AusBatProtoOneMobileClient.IOS
{
    public class ApkHelperIOS : IApkHelper
    {
        public long GetVersionCode()
        {
            return 1;
        }

        public void LoadApkExpansionFile(string destFullFilename)
        {
            throw new ApplicationException("Hires images [embedded resource] missing from distribution image");
        }
    }
}