

using AusBatProtoOneMobileClient.Helpers;
using AusBatProtoOneMobileClient.UWP;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Xamarin.Forms;
using System;
using System.Diagnostics;

[assembly: Dependency(typeof(ImageCheckerUwp))]
namespace AusBatProtoOneMobileClient.UWP
{
    public class ImageCheckerUwp : IImageChecker
    {
        StorageFolder InstallationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
        
        public async Task<bool> DoesImageExistAsync(string image)
        {
            string fname = $"Assets\\{image}";
            StorageFile file = await InstallationFolder.GetFileAsync(fname);
            if (File.Exists(file.Path))
            {
                return true;
            }
            return false;
        }

        bool IImageChecker.DoesImageExist(string image)
        {
            Debugger.Break();   
            return DoesImageExistAsync(image).Result; // Untested
        }
    }
}


