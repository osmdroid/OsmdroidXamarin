using System;
using Android.App;
using Android.OS;
using Android.Support.V4.App;

namespace OsmdroidAndroidSample
{
    [Activity(Label = "MapActivity", MainLauncher = true, Icon = "@drawable/icon")]
    public class MapActivity : FragmentActivity
    {
        private const int DialogAboutId = 1;
        private const String MapFragmentTag = "org.osmdroid.MAP_FRAGMENT_TAG";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            var fm = SupportFragmentManager;

            if (fm.FindFragmentByTag(MapFragmentTag) == null)
            {
                var mapFragment = MapFragment.NewInstance();
                fm.BeginTransaction().Add(Resource.Id.map_container, mapFragment, MapFragmentTag).Commit();
            }
        }

        protected override Dialog OnCreateDialog(int id)
        {
            switch (id)
            {
                case DialogAboutId:
                    return new AlertDialog.Builder(this)
                        .SetIcon(Resource.Drawable.Icon)
                        .SetTitle(Resource.String.app_name)
                        .SetMessage(Resource.String.about_message)
                        .SetPositiveButton(Android.Resource.String.Ok, (sender, args) => { })
                        .Create();
                default:
                    return null;
            }
        }
    }
}