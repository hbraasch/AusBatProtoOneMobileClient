using AusBatProtoOneMobileClient.Helpers;
using AusBatProtoOneMobileClient.iOS;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ViewCell), typeof(TransparentViewCellRenderer))]
namespace AusBatProtoOneMobileClient.iOS
{
    public class TransparentViewCellRenderer : ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell pCell, UITableViewCell pReusableCell, UITableView pTableView)
        {
            UITableViewCell lCell = base.GetCell(pCell, pReusableCell, pTableView);
            if (lCell != null)
            {
                SetBackgroundColor(lCell, pCell, UIColor.Black.ColorWithAlpha(0.9f));
            }
            return lCell;
        }
    }
}