using Android.OS;
using Android.Views;
using Osmdroid.Views;

namespace OsmdroidAndroidSample.SampleFragments
{
    public class SampleFragmentXmlLayout : BaseSampleFragment
    {
        public static string Title = "MapView in XML layout";

        public override string GetSampleTitle()
        {
            return Title;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var v = inflater.Inflate(Resource.Layout.MapView, null);
            MapView = v.FindViewById<MapView>(Resource.Id.mapview);
            return v;
        }
    }
}