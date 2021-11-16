using FFImageLoading.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AusBatProtoOneMobileClient.Views.Components
{
    public class ImageWithTap : Image
    {
        #region *// SelectedItemProperty
        public static readonly BindableProperty OnTappedProperty = BindableProperty.Create(nameof(OnTapped), typeof(Action<ImageSource>), typeof(CachedImageWithTap), null);

        public Action<ImageSource> OnTapped
        {
            get { return (Action<ImageSource>)GetValue(OnTappedProperty); }
            set { SetValue(OnTappedProperty, value); }
        }
        #endregion
        public ImageWithTap()
        {
            var tapRecognizer = new TapGestureRecognizer();
            tapRecognizer.Tapped +=  (s, e) => {
                OnTapped?.Invoke(((ImageSource)Source));
            };
            GestureRecognizers.Add(tapRecognizer);
        }
    }
}
