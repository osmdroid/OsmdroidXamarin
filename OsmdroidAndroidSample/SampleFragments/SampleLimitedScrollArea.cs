using Android.Content;
using Android.Graphics;
using Android.Views;
using Java.Lang;
using Osmdroid.Util;
using Osmdroid.Views;
using Osmdroid.Views.Overlay;

namespace OsmdroidAndroidSample.SampleFragments
{
    public class SampleLimitedScrollArea : BaseSampleFragment
    {
        public static string Title = "Limited scroll area";

        private const int MenuLimitScrollingId = Menu.First;

        private static readonly BoundingBoxE6 CentralParkBoundingBox;
        private static readonly Paint Paint;

        private ShadeAreaOverlay _shadeAreaOverlay;

        static SampleLimitedScrollArea()
        {
            CentralParkBoundingBox = new BoundingBoxE6(40.796788, -73.949232, 40.768094, -73.981762);
            Paint = new Paint {Color = Color.Argb(50, 255, 0, 0)};
        }

        public override string GetSampleTitle()
        {
            return Title;
        }

        protected override void AddOverlays()
        {
            base.AddOverlays();

            _shadeAreaOverlay = new ShadeAreaOverlay(Activity);
            MapView.OverlayManager.Add(_shadeAreaOverlay);

            SetLimitScrolling(true);
            HasOptionsMenu = true;
        }

        private void SetLimitScrolling(bool limitScrolling)
        {
            if (limitScrolling)
            {
                MapView.Controller.SetZoom(15);
                MapView.ScrollableAreaLimit = CentralParkBoundingBox;
                MapView.SetMinZoomLevel(15);
                MapView.SetMaxZoomLevel(18);
                MapView.Controller.AnimateTo(CentralParkBoundingBox.Center);
                _shadeAreaOverlay.Enabled = true;
                MapView.Invalidate();
            }
            else
            {
                MapView.ScrollableAreaLimit = null;
                MapView.SetMinZoomLevel(null);
                MapView.SetMaxZoomLevel(null);
                _shadeAreaOverlay.Enabled = false;
                MapView.Invalidate();
            }
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            menu.Add(0, MenuLimitScrollingId, Menu.None, "Limit scrolling").SetCheckable(true);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override void OnPrepareOptionsMenu(IMenu menu)
        {
            var item = menu.FindItem(MenuLimitScrollingId);
            item.SetChecked(MapView.ScrollableAreaLimit != null);
            base.OnPrepareOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case MenuLimitScrollingId:
                    SetLimitScrolling(MapView.ScrollableAreaLimit == null);
                    return true;
            }
            return false;
        }

        private class ShadeAreaOverlay : Overlay
        {
            private readonly GeoPoint _mTopLeft;
            private readonly GeoPoint _mBottomRight;
            private readonly Point _mTopLeftPoint = new Point();
            private readonly Point _mBottomRightPoint = new Point();

            public ShadeAreaOverlay(Context ctx) : base(ctx)
            {
                _mTopLeft = new GeoPoint(CentralParkBoundingBox.LatNorthE6, CentralParkBoundingBox.LonWestE6);
                _mBottomRight = new GeoPoint(CentralParkBoundingBox.LatSouthE6, CentralParkBoundingBox.LonEastE6);
            }

            public override void Draw(Canvas c, MapView osmv, bool shadow)
            {
                if (shadow)
                    return;

                var proj = osmv.Projection;

                proj.ToPixels(_mTopLeft, _mTopLeftPoint);
                proj.ToPixels(_mBottomRight, _mBottomRightPoint);

                var area = new Rect(_mTopLeftPoint.X, _mTopLeftPoint.Y, _mBottomRightPoint.X, _mBottomRightPoint.Y);
                c.DrawRect(area, Paint);
            }
        }


    }
}