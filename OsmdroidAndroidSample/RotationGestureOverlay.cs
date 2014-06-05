using Android.Content;
using Android.Graphics;
using Android.Views;
using Osmdroid.Views;
using Osmdroid.Views.Overlay;

namespace OsmdroidAndroidSample
{
    internal class RotationGestureOverlay : Overlay, RotationGestureDetector.IRotationListener, IOverlayMenuProvider
    {
        private const bool ShowRotateMenuItems = false;

        private static readonly int MenuEnabled = SafeMenuId;
        private static readonly int MenuRotateCcw = SafeMenuId;
        private static readonly int MenuRotateCw = SafeMenuId;

        private readonly RotationGestureDetector _mRotationDetector;
        private readonly MapView _mMapView;

        public RotationGestureOverlay(Context context, MapView mapView)
            : base(context)
        {
            OptionsMenuEnabled = true;
            _mMapView = mapView;
            _mRotationDetector = new RotationGestureDetector(this);
        }

        public bool OptionsMenuEnabled { get; set; }

        public override void Draw(Canvas canvas, MapView mapView, bool shadow)
        {
            // No drawing necessary
        }

        public override bool OnTouchEvent(MotionEvent motionEvent, MapView mapView)
        {
            if (Enabled)
            {
                _mRotationDetector.OnTouch(motionEvent);
            }
            return base.OnTouchEvent(motionEvent, mapView);
        }

        public void OnRotate(float deltaAngle)
        {
            _mMapView.MapOrientation = _mMapView.MapOrientation + deltaAngle;
        }

        public bool OnCreateOptionsMenu(IMenu menu, int menuIdOffset, MapView mapView)
        {
            menu.Add(0, MenuEnabled + menuIdOffset, Menu.None, "Enable rotation").SetIcon(
                Android.Resource.Drawable.IcMenuInfoDetails);
            if (ShowRotateMenuItems)
            {
                menu.Add(0, MenuRotateCcw + menuIdOffset, Menu.None,
                    "Rotate maps counter clockwise").SetIcon(Android.Resource.Drawable.IcMenuRotate);
                menu.Add(0, MenuRotateCw + menuIdOffset, Menu.None, "Rotate maps clockwise")
                    .SetIcon(Android.Resource.Drawable.IcMenuRotate);
            }
            return true;
        }

        public bool OnOptionsItemSelected(IMenuItem menuItem, int menuIdOffset, MapView mapView)
        {
            if (menuItem.ItemId == MenuEnabled + menuIdOffset)
            {
                if (Enabled)
                {
                    _mMapView.MapOrientation = 0;
                    Enabled = false;
                }
                else
                {
                    Enabled = true;
                    return true;
                }
            }
            else if (menuItem.ItemId == MenuRotateCcw + menuIdOffset)
            {
                _mMapView.MapOrientation -= 10;
            }
            else if (menuItem.ItemId == MenuRotateCw + menuIdOffset)
            {
                _mMapView.MapOrientation += 10;
            }

            return false;
        }

        public bool OnPrepareOptionsMenu(IMenu menu, int menuIdOffset, MapView mapView)
        {
            menu.FindItem(MenuEnabled + menuIdOffset).SetTitle(
                Enabled ? "Disable rotation" : "Enable rotation");
            return false;
        }

    }
}