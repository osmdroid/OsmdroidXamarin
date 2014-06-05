namespace OsmdroidAndroidSample.SampleFragments
{
    public class SampleFactory
    {
        private readonly BaseSampleFragment[] _mSamples;

        private static SampleFactory _instance;

        public SampleFactory()
        {
            _mSamples = new BaseSampleFragment[]{ new SampleWithMinimapItemizedOverlayWithFocus(),
                new SampleLimitedScrollArea(), new SampleFragmentXmlLayout() };
        }

        public static SampleFactory GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SampleFactory();
            }
            return _instance;
        }

        public int Count()
        {
            return _mSamples.Length;
        }

        public BaseSampleFragment GetSample(int i)
        {
            return _mSamples[i];
        }
    }
}