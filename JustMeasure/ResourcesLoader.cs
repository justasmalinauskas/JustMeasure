namespace JustMeasure
{
    public class LocalizedStrings
    {
        private static Windows.ApplicationModel.Resources.ResourceLoader _localizedResources = new Windows.ApplicationModel.Resources.ResourceLoader();

        public Windows.ApplicationModel.Resources.ResourceLoader LocalizedResources { get { return _localizedResources; } }

        private string GetVal(string key)
        {
            return LocalizedResources.GetString(key);
        }

        public string this[string key]
        {
            get
            {
                return GetVal(key);
            }
        }
    }
}
