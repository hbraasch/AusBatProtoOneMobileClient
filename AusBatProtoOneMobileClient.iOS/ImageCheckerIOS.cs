using AusBatProtoOneMobileClient.Helpers;
using AusBatProtoOneMobileClient.iOS;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(ImageCheckerIOS))]
namespace AusBatProtoOneMobileClient.iOS
{
    public class ImageCheckerIOS : IImageChecker
    {
        public bool DoesImageExist(string image)
        {
            var x = UIImage.FromBundle(image);
            return x != null;
        }

        Task<bool> IImageChecker.DoesImageExist(string image)
        {
            return Task.FromResult(DoesImageExist(image));
        }
    }
}