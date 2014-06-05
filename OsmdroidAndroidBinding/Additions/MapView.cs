using System;
using Java.Lang;

namespace Osmdroid.Views
{
    public partial class MapView
    {
        public void SetMinZoomLevel(int zoomLevel)
        {
            SetMinZoomLevel(new Integer(zoomLevel));
        }

        public void SetMaxZoomLevel(int zoomLevel)
        {
            SetMaxZoomLevel(new Integer(zoomLevel));
        }

    }
}