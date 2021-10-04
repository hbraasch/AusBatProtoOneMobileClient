using AusBatProtoOneMobileClient.Droid;
using AusBatProtoOneMobileClient.Helpers;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;


[assembly: Dependency(typeof(ImageCheckerDroid))]
namespace AusBatProtoOneMobileClient.Droid
{
    public class ImageCheckerDroid : IImageChecker
    {
        Task<bool> IImageChecker.DoesImageExist(string image)
        {
            var context = Android.App.Application.Context;
            var resources = context.Resources;
            var name = Path.GetFileNameWithoutExtension(image);
            int resourceId = resources.GetIdentifier(name.ToLower(), "drawable", context.PackageName);
            return Task.FromResult(resourceId != 0);
        }
    }
}