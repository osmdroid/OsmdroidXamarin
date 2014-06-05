using System;

using Android.App;
using Android.OS;
using Osmdroid.TileProvider.TileSource;
using Osmdroid.Util;
using Osmdroid.Views;

namespace ComponentSample
{
    [Activity(Label = "ComponentSample", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            var mapView = FindViewById<MapView>(Resource.Id.mapview);
            mapView.SetTileSource(TileSourceFactory.DefaultTileSource);
            mapView.SetBuiltInZoomControls(true);

            var mapController = mapView.Controller;
            mapController.SetZoom(25);

            var centreOfMap = new GeoPoint(51496994, -134733);
            mapController.SetCenter(centreOfMap);
        }
    }
}

