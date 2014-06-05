using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Osmdroid;
using Osmdroid.Util;
using Osmdroid.Views;

namespace OsmdroidAndroidSample.SampleFragments
{
    public abstract class BaseSampleFragment : Fragment
    {
        protected MapView MapView;
        protected IResourceProxy ResourceProxy;

        public abstract string GetSampleTitle();

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ResourceProxy = new ResourceProxyImpl(inflater.Context.ApplicationContext);
            MapView = new MapView(inflater.Context, 256, ResourceProxy);
            return MapView;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            AddOverlays();

            MapView.SetBuiltInZoomControls(true);
            MapView.SetMultiTouchControls(true);
        }

        /// <summary>
        /// An appropriate place to override and add overlays.
        /// </summary>
        protected virtual void AddOverlays()
        {
            //
        }

    }
}