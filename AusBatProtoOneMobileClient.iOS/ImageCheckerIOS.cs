using AusBatProtoOneMobileClient.Helpers;
using AusBatProtoOneMobileClient.iOS;
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
    }
}