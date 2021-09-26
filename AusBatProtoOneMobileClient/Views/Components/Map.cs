using AusBatProtoOneMobileClient.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TouchTracking;
using Xamarin.Forms;

namespace AusBatProtoOneMobileClient.Views.Components
{
    public class Map : Grid
    {
        float aspect = (float)( 378.0/ 398.0);

        public static readonly BindableProperty SelectedItemsProperty = BindableProperty.Create("SelectedItems", typeof(ObservableCollection<MapRegion>), typeof(Map), new ObservableCollection<MapRegion>(), propertyChanged: OnSelectedChanged);

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

        public Map()
        {
            absoluteLayout = new AbsoluteLayout();
            frame = new Frame();
            frame.Content = absoluteLayout;
            frame.BorderColor = Color.Red;
            frame.Padding = 0;
            frame.Margin = 0;
            frame.CornerRadius = 0;
            frame.BackgroundColor = Color.Transparent;
            frame.HorizontalOptions = LayoutOptions.Center;
            frame.VerticalOptions = LayoutOptions.Center;


            TouchEffect touchEffect = new TouchEffect();
            touchEffect.TouchAction += OnTouchEffectAction;
            frame.Effects.Add(touchEffect);

            RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            Children.Add(frame, 0, 0);
            HorizontalOptions = LayoutOptions.Center;
            VerticalOptions = LayoutOptions.Center;

            MakeItemsVisible(SelectedItems);
        }

        AbsoluteLayout absoluteLayout;
        Frame frame;
        private void MakeItemsVisible(ObservableCollection<MapRegion> items)
        {

            var layeredImages = new List<string>();
            layeredImages.Add("Map000.png");
            if (items != null)
            {
                foreach (var item in items)
                {
                    layeredImages.Add($"Map{item.Id:000}.png");
                }
            }

            absoluteLayout.Children.Clear();
            foreach (var layeredImage in layeredImages)
            {
                var image = new Image { Source = layeredImage, 
                    Aspect = Aspect.AspectFit ,
                };
                absoluteLayout.Children.Add(image);
                AbsoluteLayout.SetLayoutFlags(image, AbsoluteLayoutFlags.All);
                AbsoluteLayout.SetLayoutBounds(image, new Rectangle(0, 0, 1, 1));
            }
            

        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            WidthRequest = 300;
            HeightRequest = WidthRequest * aspect;
        }

        private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            switch (args.Type)
            {
                case TouchActionType.Pressed:
#if false
                    Debug.WriteLine($"Width: {frame.Bounds.Width} Height: {frame.Bounds.Height}");
                    Debug.WriteLine($"Left: {args.Location.X} Top: {args.Location.Y}");
                    Debug.WriteLine($"Left%: {args.Location.X / frame.Bounds.Width:N2} Top%: {args.Location.Y / frame.Bounds.Height:N2}"); 
#endif
                    foreach (var mapRegion in App.dbase.MapRegions)
                    {
                        foreach (var hotspot in mapRegion.Hotspots)
                        {
                            var left = args.Location.X / frame.Bounds.Width;
                            var top = args.Location.Y / frame.Bounds.Height;
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
