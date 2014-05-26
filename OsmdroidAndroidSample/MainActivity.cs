using System;

using Android.App;
using Android.OS;
using Osmdroid.Api;
using Osmdroid.TileProvider.TileSource;
using Osmdroid.Util;
using Osmdroid.Views;

namespace OsmdroidAndroidSample
{
    [Activity(Label = "OsmdroidAndroidSample", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private IMapController _mapController;
        private MapView _mapView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            _mapView = FindViewById<MapView>(Resource.Id.mapview);
            _mapView.SetTileSource(TileSourceFactory.DefaultTileSource);
            _mapView.SetBuiltInZoomControls(true);

            _mapController = _mapView.Controller;
            _mapController.SetZoom(25);

            var centreOfMap = new GeoPoint(51496994, -134733);
            _mapController.SetCenter(centreOfMap);
        }
    }
}

