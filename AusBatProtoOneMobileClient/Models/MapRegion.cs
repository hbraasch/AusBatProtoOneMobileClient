using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AusBatProtoOneMobileClient.Models
{
    public class MapRegion
    {
        public int Id { get; set; }
        public List<HotSpotItem> Hotspots { get; set; } = new List<HotSpotItem>();
        public class HotSpotItem
        {
            public Point Center { get; set; }
            public float Radius { get; set; }
        }

    }
}
