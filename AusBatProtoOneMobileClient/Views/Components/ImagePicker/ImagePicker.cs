using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using static AusBatProtoOneMobileClient.Views.Components.ImagePickerDisplayOptions;

namespace AusBatProtoOneMobileClient.Views.Components
{
    public class ImagePicker : Label
    {



        #region *// ItemsSourceProperty
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(List<string>), typeof(ImagePicker),new List<string>());

        public List<string> ItemsSource
        {
            get { 
                return (List<string>)GetValue(ItemsSourceProperty); }
            set { 
                SetValue(ItemsSourceProperty, value); }
        }
        #endregion

        #region *// ImageItemsSourceProperty
        public static readonly BindableProperty ImageItemsSourceProperty = BindableProperty.Create(nameof(ImageItemsSource), typeof(List<string>), typeof(ImagePicker), new List<string>());

        public List<string> ImageItemsSource
        {
            get { return (List<string>)GetValue(ImageItemsSourceProperty); }
            set { SetValue(ImageItemsSourceProperty, value); }
        }
        #endregion

        #region *// SelectedItemProperty
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(string), typeof(ImagePicker), "");

        public string SelectedItem
        {
            get { return (string)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        #endregion

        #region *// OnChangedProperty
        public static readonly BindableProperty OnChangedProperty = BindableProperty.Create(nameof(OnChanged), typeof(ICommand), typeof(ImagePicker), null);

        public ICommand OnChanged
        {
            get { return (ICommand)GetValue(OnChangedProperty); }
            set { SetValue(OnChangedProperty, value); }
        }
        #endregion

        #region *// PromptProperty
        public static readonly BindableProperty PromptProperty = BindableProperty.Create(nameof(Prompt), typeof(string), typeof(ImagePicker), "Options");

        public string Prompt
        {
            get {return (string)GetValue(PromptProperty);}
            set {SetValue(PromptProperty, value);}
        }
        #endregion

        Page parentPage;

        public ImagePicker(Page parentPage)
        {
            this.parentPage = parentPage;
            BackgroundColor = Color.DarkGray.MultiplyAlpha(0.5);
            TextColor = Color.White;
            Padding = 5;
            FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
            VerticalTextAlignment = TextAlignment.Center;
            Text = SelectedItem;
            var tapRecognizer = new TapGestureRecognizer();
            tapRecognizer.Tapped += async (s, e) => {
                var displayItems = new ObservableCollection<PickerWithImagesDisplayData>();
                for (int i = 0; i < ItemsSource.Count; i++)
                {
                    if (ItemsSource[i] == "") continue;
                    var imageSource = (ImageItemsSource.Count == 0) ? "": ImageItemsSource[i];
                    displayItems.Add(new PickerWithImagesDisplayData
                    {
                        Description = ItemsSource[i],
                        ImageSource = imageSource
                    });
                }
                var page = new ImagePickerDisplayOptions(displayItems, Prompt);
                await parentPage.Navigation.PushAsync(page);
                await page.WaitUntilExecutionStops();
                SelectedItem = page.SelectedDisplayItem?.Description ?? "";
                Text = SelectedItem;
                if (!string.IsNullOrEmpty(Text))  OnChanged?.Execute(null);
            };
            GestureRecognizers.Add(tapRecognizer);
        }
    }
}
