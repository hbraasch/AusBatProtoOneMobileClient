using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AusBatProtoOneMobileClient.Helpers
{
    public interface IImageChecker
    {
        Task<bool> DoesImageExist(string image);
    }

    public class ImageChecker
    {
        public static async Task<bool> DoesImageExist(string image) => await DependencyService.Get<IImageChecker>().DoesImageExist(image);

        public static bool IsResourceAvailable(string resource)
        {
            var assembly = typeof(ImageChecker).GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                System.Diagnostics.Debug.WriteLine("found resource: " + res);
                if (resource == res)
                {
                    return true;
                }
            }
            return false;
        }
    }



    
}
