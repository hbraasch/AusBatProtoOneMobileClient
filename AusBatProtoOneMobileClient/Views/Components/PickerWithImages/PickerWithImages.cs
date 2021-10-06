﻿using AusBatProtoOneMobileClient.Models;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static AusBatProtoOneMobileClient.ViewModels.ClassificationPageViewModel;
using static AusBatProtoOneMobileClient.Views.Components.PickerWithImagesDisplayOptions;

namespace AusBatProtoOneMobileClient.Views.Components
{
    public class PickerWithImages : Label
    {



        #region *// ItemsSourceProperty
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(List<string>), typeof(PickerWithImages),new List<string>());

        public List<string> ItemsSource
        {
            get { 
                return (List<string>)GetValue(ItemsSourceProperty); }
            set { 
                SetValue(ItemsSourceProperty, value); }
        }
        #endregion

        #region *// ImageItemsSourceProperty
        public static readonly BindableProperty ImagesItemSourceProperty = BindableProperty.Create(nameof(ImagesItemSource), typeof(List<string>), typeof(PickerWithImages), new List<string>());

        public List<string> ImagesItemSource
        {
            get { return (List<string>)GetValue(ImagesItemSourceProperty); }
            set { SetValue(ImagesItemSourceProperty, value); }
        }
        #endregion

        #region *// SelectedItemProperty
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(string), typeof(PickerWithImages), "");

        public string SelectedItem
        {
            get { return (string)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        #endregion

        Page parentPage;

        public PickerWithImages(Page parentPage)
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
                var displayItems = new List<PickerWithImagesDisplayData>();
                for (int i = 0; i < ItemsSource.Count; i++)
                {
                    if (ItemsSource[i] == "") continue;
                    displayItems.Add(new PickerWithImagesDisplayData
                    {
                        Description = ItemsSource[i],
                        ImageSource = ImagesItemSource[i]
                    });
                }
                var page = new PickerWithImagesDisplayOptions()
                {
                    DisplayItems = new ObservableCollection<PickerWithImagesDisplayData>(displayItems),
                    SelectedDisplayItem = null
                };
                await parentPage.Navigation.PushAsync(page);
                await page.WaitUntilExecutionStops();
                SelectedItem = page.SelectedDisplayItem?.Description ?? "";
                Text = SelectedItem;
            };
            GestureRecognizers.Add(tapRecognizer);
        }
    }
}
