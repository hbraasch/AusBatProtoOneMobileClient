﻿using AusBatProtoOneMobileClient.Models;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TouchTracking;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AusBatProtoOneMobileClient.Views.Components
{
    public class Map : Grid
    {
        float aspect = (float)( 7678.0/ 818.0);

        public static readonly BindableProperty SelectedItemsProperty = BindableProperty.Create("SelectedItems", typeof(ObservableCollection<MapRegion>), typeof(Map), new ObservableCollection<MapRegion>(), propertyChanged: OnSelectedChanged);

        [SuppressPropertyChangedWarnings]
        private static void OnSelectedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var context = bindable as Map;
            context.MakeItemsVisible(newValue as ObservableCollection<MapRegion>);
        }

        public ObservableCollection<MapRegion> SelectedItems
        {
            get { return (ObservableCollection<MapRegion>)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        public static readonly BindableProperty IsSelectableProperty = BindableProperty.Create("IsSelectable", typeof(bool), typeof(Map), false);

        public bool IsSelectable
        {
            get { return (bool)GetValue(IsSelectableProperty); }
            set { SetValue(IsSelectableProperty, value); }
        }

        TouchEffect touchEffect;
        public Map()
        {
            var width = Math.Min(App.screenWidth, App.screenHeight);
            var height = width * aspect;

            RowDefinitions.Add(new RowDefinition { Height = new GridLength(height, GridUnitType.Absolute) });
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(width, GridUnitType.Absolute) });

            BackgroundColor = Color.LightBlue;

            HorizontalOptions = LayoutOptions.Center;
            VerticalOptions = LayoutOptions.Center;

            touchEffect = new TouchEffect();
            touchEffect.TouchAction += OnTouchEffectAction;

            Effects.Add(touchEffect);

            MakeItemsVisible(SelectedItems);
        }

        Image image;
        private void MakeItemsVisible(ObservableCollection<MapRegion> items)
        {
            
            var layeredImages = new List<string>();
            layeredImages.Add("map000.png");
            if (items != null)
            {
                foreach (var item in items)
                {
                    layeredImages.Add($"map{item.Id:000}.png");
                }
            }

            Children.Clear();
            foreach (var layeredImage in layeredImages)
            {
                var image = new Image { Source = layeredImage, 
                    Aspect = Aspect.AspectFit ,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    WidthRequest = Math.Min(App.screenWidth, App.screenHeight),
                    HeightRequest = WidthRequest * aspect,
                };
                Children.Add(image,0,0);
            }
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

        }

        private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            switch (args.Type)
            {
                case TouchActionType.Pressed:
#if true
                    Debug.WriteLine($"Width: {Bounds.Width} Height: {Bounds.Height}");
                    Debug.WriteLine($"Left: {args.Location.X} Top: {args.Location.Y}");
                    Debug.WriteLine($"Left%: {args.Location.X / Bounds.Width:N2} Top%: {args.Location.Y / Bounds.Height:N2}"); 
#endif
                    foreach (var mapRegion in App.dbase.MapRegions)
                    {
                        foreach (var hotspot in mapRegion.Hotspots)
                        {
                            var left = args.Location.X / Bounds.Width;
                            var top = args.Location.Y / Bounds.Height;
                            if (IsInside(hotspot.Center, hotspot.Radius, left, top))
                            {
                                Debug.WriteLine($"Region: {mapRegion.Id} pressed");
                                if (IsSelectable)
                                {
                                    var item = SelectedItems?.ToList().FirstOrDefault(o => o.Id == mapRegion.Id) ?? null;
                                    if (item != null)
                                    {
                                        // Remove selected item
                                        SelectedItems.Remove(item);
                                    }
                                    else
                                    {
                                        // Add selected item
                                        if (SelectedItems == null) SelectedItems = new ObservableCollection<MapRegion>();
                                        SelectedItems.Add(mapRegion);
                                    }
                                    MakeItemsVisible(SelectedItems); 
                                }
                                return;
                            }
                        }
                    }
                    break;
                case TouchActionType.Moved:
                case TouchActionType.Released:
                    break;
            }
        }

        bool IsInside(Point center, float radius, double left, double top)
        {
            // Compare radius of circle with
            // distance of its center from
            // given point
            if ((left - center.X) * (left - center.X) +
                (top - center.Y) * (top - center.Y) <= radius * radius)
                return true;
            else
                return false;
        }
    }



}
