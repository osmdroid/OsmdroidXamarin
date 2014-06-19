using Android.Graphics;
using Osmdroid.Api;

namespace Osmdroid.Google.Wrapper
{
    public partial class MapView
    {
        public void SetBackgroundColor(Color color)
        {
            ((IMapView)this).SetBackgroundColor(color);
        }
    }
}