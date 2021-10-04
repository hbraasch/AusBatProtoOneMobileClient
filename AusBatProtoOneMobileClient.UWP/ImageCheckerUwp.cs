

using AusBatProtoOneMobileClient.Helpers;
using AusBatProtoOneMobileClient.UWP;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Xamarin.Forms;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using Windows.ApplicationModel;
using System.Collections.Generic;

[assembly: Dependency(typeof(ImageCheckerUwp))]
namespace AusBatProtoOneMobileClient.UWP
{
    public class ImageCheckerUwp : IImageChecker
    {
        StorageFolder InstallationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
        IReadOnlyList<IStorageItem> itemsInFolder;
        public async Task<bool> DoesImageExistAsync(string image)
        {
            var imageName = Path.GetFileNameWithoutExtension(image);

            // Get the app's installation folder.
            StorageFolder appFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;

            // Get the files and folders in the current folder.
            if (itemsInFolder == null)
            {
                itemsInFolder = await appFolder.GetFilesAsync(); 
            }

            // Iterate over the results and print the list of items
            // to the Visual Studio Output window.
            foreach (IStorageItem item in itemsInFolder)
            {
                if (item.IsOfType(StorageItemTypes.File))
                {
                    if (item.Name.IndexOf(imageName) != -1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        Task<bool> IImageChecker.DoesImageExist(string image)
        {
            // Debugger.Break();

            var tcs = new TaskCompletionSource<bool>();
            Task.Factory.StartNew(async () => { 
                var result = await DoesImageExistAsync(image);
                tcs.SetResult(result);
            
            });
            return tcs.Task;
        }

    }
}


