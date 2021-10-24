using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AusBatProtoOneMobileClient.Models
{
    public class HiresImages
    {
        public static string FullFolderName { get; set; }
        public static Task Extract()
        {
            var tcs = new TaskCompletionSource<object>();
            Task.Factory.StartNew(() => {
                try
                {
                    var workFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
                    var zipPath = Path.Combine(workFolder, Constants.HIRES_IMAGES_ZIP_FILE_NAME);
                    FullFolderName = Path.Combine(workFolder, Constants.HIRES_IMAGES_FOLDER_NAME);
                    if (Directory.Exists(FullFolderName))
                    {
                        Directory.Delete(FullFolderName, true);
                    }
                    WriteResourceToFile($"AusBatProtoOneMobileClient.Data.SpeciesImages.Hires.{Constants.HIRES_IMAGES_ZIP_FILE_NAME}", zipPath);
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

        public static string GetFullFilename(string filename)
        {
            if (string.IsNullOrEmpty(FullFolderName))
            {
                var workFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
                FullFolderName = Path.Combine(workFolder, Constants.HIRES_IMAGES_FOLDER_NAME);
            }
            return Path.Combine(FullFolderName, filename);
        }
    }
}
