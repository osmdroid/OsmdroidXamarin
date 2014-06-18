using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Runtime;

namespace Osmdroid.TileProvider.TileSource.Bing
{    
    public partial class BingMapTileSource
    {
        public void SetStyle(string style)
        {
            this.Style = style;
        }

        Java.Lang.Object IStyledTileSource.Style
        {
            get { return new Java.Lang.String(Style); }
            set { Style = (string)value.JavaCast<Java.Lang.String>(); }
        }
    }
}
