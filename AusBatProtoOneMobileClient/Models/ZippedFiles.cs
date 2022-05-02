using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using static Xamarin.Essentials.Permissions;

namespace AusBatProtoOneMobileClient.Models
{
    public class ZippedFiles
    {
        public static string FullFolderName { get; set; }
        public static Task Extract()
        {
            var tcs = new TaskCompletionSource<object>();
            Task.Factory.StartNew(async () => {
                try
                {
                    var workFolder = GetWorkFolderFullName();
                    FullFolderName = Path.Combine(workFolder, Constants.HIRES_IMAGES_FOLDER_NAME);
                    if (Directory.Exists(FullFolderName))
                    {
                        Directory.Delete(FullFolderName, true);
                    }

                    var zipPath = Path.Combine(workFolder, Constants.HIRES_IMAGES_ZIP_FILE_NAME);

                    #region *// Blob file management
                    var resourceName = $"AusBatProtoOneMobileClient.Data.SpeciesImages.Hires.{Constants.HIRES_IMAGES_ZIP_FILE_NAME}";
                    var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
                    if (resource == null)
                    {
                        // Embedded resource file exist, so extract
                        WriteResourceToFile(resourceName, zipPath);
                    }
                    else
                    {

                        #region *// Request access to storage
                        var status = await CheckAndRequestPermissionAsync(new Permissions.StorageRead());
                        if (status != PermissionStatus.Granted)
                        {
                            throw new ApplicationException("Permission denied to access external storage");
                        }
                        status = await CheckAndRequestPermissionAsync(new Permissions.StorageWrite());
                        if (status != PermissionStatus.Granted)
                        {
                            throw new ApplicationException("Permission denied to access external storage");
                        }
                        #endregion
                        // APK expension file should exist, extract
                        ApkHelper.LoadApkExpansionFile(zipPath);
                    }
                    #endregion



                    
                    ZipFile.ExtractToDirectory(zipPath, FullFolderName);
                    File.Delete(zipPath);
                    tcs.SetResult(null);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            return tcs.Task;
        }

        public static async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission)
            where T : BasePermission
        {
            var status = await permission.CheckStatusAsync();
            if (status != PermissionStatus.Granted)
            {
                status = await permission.RequestAsync();
            }

            return status;
        }

        private static void WriteResourceToFile(string resourceName, string fileName)
        {
            using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    resource.CopyTo(file);
                }
            }
        }

        private static string GetWorkFolderFullName()
        {
            var workFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            workFolder = Path.Combine(workFolder, "WorkFolder");
            if (!Directory.Exists(workFolder))
            {
                Directory.CreateDirectory(workFolder);
            }
            return workFolder;
        }

        public static string GetFullFilename(string filename)
        {
            if (string.IsNullOrEmpty(FullFolderName))
            {
                var workFolder = GetWorkFolderFullName();
                FullFolderName = Path.Combine(workFolder, Constants.HIRES_IMAGES_FOLDER_NAME);
            }
            return Path.Combine(FullFolderName, filename);
        }
    }
}
