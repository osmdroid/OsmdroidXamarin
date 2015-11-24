using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Osmdroid;
using Osmdroid.TileProvider.TileSource;
using Osmdroid.Views.Overlay.Compass;
using Osmdroid.Views.Overlay.MyLocation;
using Osmdroid.Util;
using Osmdroid.Views;
using Osmdroid.Views.Overlay;
using OsmdroidAndroidSample.SampleFragments;

namespace OsmdroidAndroidSample
{
    public class MapFragment : Fragment
    {
        private const int DialogAboutId = 1;

        private const int MenuSamples = Menu.First + 1;
        private const int MenuAbout = MenuSamples + 1;

        private const int MenuLastId = MenuAbout + 1;

        private ISharedPreferences _prefs;
        private MapView _mapView;
        private MyLocationNewOverlay _myLocationOverlay;
        private CompassOverlay _compassOverlay;
        private MinimapOverlay _minimapOverlay;
        private ScaleBarOverlay _scaleBarOverlay;
        private RotationGestureOverlay _rotationGestureOverlay;
        private IResourceProxy _resourceProxy;

        public static MapFragment NewInstance()
        {
            var fragment = new MapFragment();
            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _resourceProxy = new ResourceProxyImpl(inflater.Context.ApplicationContext);
            _mapView = new MapView(inflater.Context, _resourceProxy);
            return _mapView;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            var context = Activity;
            var dm = context.Resources.DisplayMetrics;
            // mResourceProxy = new ResourceProxyImpl(getActivity().getApplicationContext());

            _prefs = context.GetSharedPreferences(OpenStreetMapConstants.PrefsName, FileCreationMode.Private);

            _compassOverlay = new CompassOverlay(context, new InternalCompassOrientationProvider(context),
                _mapView);
            _myLocationOverlay = new MyLocationNewOverlay(context, new GpsMyLocationProvider(context),
                _mapView);

            _minimapOverlay = new MinimapOverlay(Activity, _mapView.TileRequestCompleteHandler)
            {
                Width = dm.WidthPixels/5,
                Height = dm.HeightPixels/5
            };

            _scaleBarOverlay = new ScaleBarOverlay(context);
            _scaleBarOverlay.SetCentred(true);
            _scaleBarOverlay.SetScaleBarOffset(dm.WidthPixels/2, 10);

            _rotationGestureOverlay = new RotationGestureOverlay(context, _mapView) {Enabled = false};

            _mapView.SetBuiltInZoomControls(true);
            _mapView.SetMultiTouchControls(true);
            _mapView.Overlays.Add(_myLocationOverlay);
            _mapView.Overlays.Add(_compassOverlay);
            _mapView.Overlays.Add(_minimapOverlay);
            _mapView.Overlays.Add(_scaleBarOverlay);
            _mapView.Overlays.Add(_rotationGestureOverlay);

            _mapView.Controller.SetZoom(_prefs.GetInt(OpenStreetMapConstants.PrefsZoomLevel, 1));
            _mapView.ScrollTo(_prefs.GetInt(OpenStreetMapConstants.PrefsScrollX, 0),
                _prefs.GetInt(OpenStreetMapConstants.PrefsScrollY, 0));

            _myLocationOverlay.EnableMyLocation();
            _compassOverlay.EnableCompass();

            HasOptionsMenu = true;
        }

        public override void OnPause()
        {
            var edit = _prefs.Edit();
            edit.PutString(OpenStreetMapConstants.PrefsTileSource, _mapView.TileProvider.TileSource.Name());
            edit.PutInt(OpenStreetMapConstants.PrefsScrollX, _mapView.ScrollX);
            edit.PutInt(OpenStreetMapConstants.PrefsScrollY, _mapView.ScrollY);
            edit.PutInt(OpenStreetMapConstants.PrefsZoomLevel, _mapView.ZoomLevel);
            edit.PutBoolean(OpenStreetMapConstants.PrefsShowLocation, _myLocationOverlay.IsMyLocationEnabled);
            edit.PutBoolean(OpenStreetMapConstants.PrefsShowCompass, _compassOverlay.IsCompassEnabled);
            edit.Commit();

            _myLocationOverlay.DisableMyLocation();
            _compassOverlay.DisableCompass();

            base.OnPause();
        }

        public override void OnResume()
        {
            base.OnResume();
            var tileSourceName = _prefs.GetString(OpenStreetMapConstants.PrefsTileSource,
                TileSourceFactory.DefaultTileSource.Name());
            try
            {
                var tileSource = TileSourceFactory.GetTileSource(tileSourceName);
                _mapView.SetTileSource(tileSource);
            }
            catch (Java.Lang.IllegalArgumentException)
            {
                _mapView.SetTileSource(TileSourceFactory.DefaultTileSource);
            }
            if (_prefs.GetBoolean(OpenStreetMapConstants.PrefsShowLocation, false))
            {
                _myLocationOverlay.EnableMyLocation();
            }
            if (_prefs.GetBoolean(OpenStreetMapConstants.PrefsShowCompass, false))
            {
                _compassOverlay.EnableCompass();
            }
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            // Put overlay items first
            _mapView.OverlayManager.OnCreateOptionsMenu(menu, MenuLastId, _mapView);

            // Put samples next
            var samplesSubMenu = menu.AddSubMenu(0, MenuSamples, Menu.None, Resource.String.samples)
                .SetIcon(Android.Resource.Drawable.IcMenuGallery);
            var sampleFactory = SampleFactory.GetInstance();
            for (var a = 0; a < sampleFactory.Count(); a++)
            {
                var f = sampleFactory.GetSample(a);
                samplesSubMenu.Add(f.GetSampleTitle()).SetOnMenuItemClickListener(new StartSampleFragment(this, f));
            }

            // Put "About" menu item last
            menu.Add(0, MenuAbout, (int) MenuCategory.Secondary, Resource.String.about)
                .SetIcon(Android.Resource.Drawable.IcMenuInfoDetails);

            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override void OnPrepareOptionsMenu(IMenu menu)
        {
            _mapView.OverlayManager.OnPrepareOptionsMenu(menu, MenuLastId, _mapView);
            base.OnPrepareOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (_mapView.OverlayManager.OnOptionsItemSelected(item, MenuLastId, _mapView))
                return true;

            switch (item.ItemId)
            {
                case MenuAbout:
                    Activity.ShowDialog(DialogAboutId);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        private class StartSampleFragment : Java.Lang.Object, IMenuItemOnMenuItemClickListener
        {
            private readonly Fragment _currentFragment;
            private readonly BaseSampleFragment _selectedFragment;

            public StartSampleFragment(Fragment currentFragment, BaseSampleFragment selectedFragment)
            {
                _currentFragment = currentFragment;
                _selectedFragment = selectedFragment;
            }

            public bool OnMenuItemClick(IMenuItem item)
            {
                _currentFragment.FragmentManager
                    .BeginTransaction()
                    .Hide(_currentFragment)
                    .Add(Android.Resource.Id.Content, _selectedFragment, "SampleFragment")
                    .AddToBackStack(null)
                    .Commit();
                return true;
            }
        }

    }
}