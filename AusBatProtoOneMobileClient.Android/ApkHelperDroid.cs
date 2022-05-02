using AusBatProtoOneMobileClient.Droid;
using AusBatProtoOneMobileClient.Models;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(ApkHelperDroid))]

namespace AusBatProtoOneMobileClient.Droid
{
    public class ApkHelperDroid : IApkHelper
    {
        /* 
            For testing, place copy of hires_images.zip on test device 
                        - folder: Android\data\com.treeapps.ausbat\files\Android\obb\com.treeapps.ausbat
                        - file: main.{apkVersion}.com.treaaps.ausbat.obb
        */
        public void LoadApkExpansionFile(string destFullFilename)
        {
            try
            {
                var context = Android.App.Application.Context;
                var sharedStore = context.GetExternalFilesDir(null).AbsolutePath;
                var packageName = "com.treeapps.ausbat";
                var storageFullFilename = $"{sharedStore}/Android/obb/{packageName}";
                var apkVersion = GetVersionCode();
                var filename = $"main.{apkVersion}.{packageName}.obb";
                var fileFullname = Path.Combine(storageFullFilename, filename);
                if (!File.Exists(fileFullname))
                {
                    throw new ApplicationException($"Apk expansion file [{fileFullname}] does not exist");
                }
                #region *// Copy expansion file to 
                File.Copy(fileFullname, destFullFilename, true);
                #endregion
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to load Apk extension file", ex);
            }
        }

        public long GetVersionCode()
        {
            var context = Android.App.Application.Context;
            return context.PackageManager.GetPackageInfo(context.PackageName, 0).LongVersionCode;

        }

    }
}