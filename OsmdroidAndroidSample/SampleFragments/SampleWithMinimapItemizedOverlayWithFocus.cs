using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Osmdroid.Util;
using Osmdroid.Views.Overlay;

namespace OsmdroidAndroidSample.SampleFragments
{
    public class SampleWithMinimapItemizedOverlayWithFocus : BaseSampleFragment
    {
        public static string Title = "Itemized overlay w/focus";

        private const int MenuZoominId = Menu.First;
        private const int MenuZoomoutId = MenuZoominId + 1;
        private const int MenuLastId = MenuZoomoutId + 1; // Always set to last unused id

        public override string GetSampleTitle()
        {
            return Title;
        }

        protected override void AddOverlays()
        {
            base.AddOverlays();

            /* Itemized Overlay */
            /* Create a static ItemizedOverlay showing some Markers on various cities. */
            var items = new List<OverlayItem>
            {
                new OverlayItem("Hannover", "Tiny SampleDescription", new GeoPoint(52370816, 9735936)),
                new OverlayItem("Berlin", "This is a relatively short SampleDescription.",
                    new GeoPoint(52518333, 13408333)),
                new OverlayItem(
                    "Washington",
                    "This SampleDescription is a pretty long one. Almost as long as a the great wall in china.",
                    new GeoPoint(38895000, -77036667)),
                new OverlayItem("San Francisco", "SampleDescription", new GeoPoint(37779300, -122419200))
            };

            /* OnTapListener for the Markers, shows a simple Toast. */
            var overlay = new ItemizedOverlayWithFocus(items, new OnItemGestureListener(Activity), ResourceProxy);

            overlay.SetFocusItemsOnTap(true);
            overlay.SetFocusedItem(0);

            MapView.Overlays.Add(overlay);

            var rotationGestureOverlay = new RotationGestureOverlay(Activity, MapView) {Enabled = false};
            MapView.Overlays.Add(rotationGestureOverlay);

            /* MiniMap */
            var miniMapOverlay = new MinimapOverlay(Activity, MapView.TileRequestCompleteHandler);
            MapView.Overlays.Add(miniMapOverlay);

            // Zoom and center on the focused item.
            MapView.Controller.SetZoom(5);
            var geoPoint = ((OverlayItem) overlay.FocusedItem).Point;
            MapView.Controller.AnimateTo(geoPoint);

            HasOptionsMenu = true;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            // Put overlay items first
            MapView.OverlayManager.OnCreateOptionsMenu(menu, MenuLastId, MapView);

            menu.Add(0, MenuZoominId, Menu.None, "ZoomIn");
            menu.Add(0, MenuZoomoutId, Menu.None, "ZoomOut");

            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override void OnPrepareOptionsMenu(IMenu menu)
        {
            MapView.OverlayManager.OnPrepareOptionsMenu(menu, MenuLastId, MapView);
            base.OnPrepareOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (MapView.OverlayManager.OnOptionsItemSelected(item, MenuLastId, MapView))
                return true;

            switch (item.ItemId)
            {
                case MenuZoominId:
                    MapView.Controller.ZoomIn();
                    return true;

                case MenuZoomoutId:
                    MapView.Controller.ZoomOut();
                    return true;
            }
            return false;
        }

        private class OnItemGestureListener : Object, ItemizedIconOverlay.IOnItemGestureListener
        {
            private readonly Context _context;

            public OnItemGestureListener(Context context)
            {
                _context = context;
            }

            public bool OnItemSingleTapUp(int index, Object item)
            {
                var overlayItem = (OverlayItem) item;
                Toast.MakeText(
                    _context,
                    "Item '" + overlayItem.Title + "' (index=" + index + ") got single tapped up", 
                    ToastLength.Long).Show();
                return true;
            }

            public bool OnItemLongPress(int index, Object item)
            {
                var overlayItem = (OverlayItem) item;
                Toast.MakeText(
                    _context,
                    "Item '" + overlayItem.Title + "' (index=" + index + ") got long pressed", 
                    ToastLength.Long).Show();
                return false;
            }
        }
    }
}