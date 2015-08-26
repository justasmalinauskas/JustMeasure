using System;
using Windows.Storage.Pickers;

namespace JustMeasure
{
    public class LoadMaps
    {

        public async System.Threading.Tasks.Task LoadKML(System.Collections.Generic.List<Windows.Devices.Geolocation.BasicGeoposition> mapdata)
        {
            Windows.Data.Xml.Dom.XmlLoadSettings loadSettings = new Windows.Data.Xml.Dom.XmlLoadSettings();
            loadSettings.ElementContentWhiteSpace = false;
            Windows.Data.Xml.Dom.XmlDocument kml = new Windows.Data.Xml.Dom.XmlDocument();
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.List;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".kml");
            Windows.Storage.IStorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                string[] stringSeparators = new string[] { ",17" };
                kml = await Windows.Data.Xml.Dom.XmlDocument.LoadFromFileAsync(file);
                var element = kml.GetElementsByTagName("coordinates").Item(0);
                string ats  = element.FirstChild.GetXml();

                string[] atsa = ats.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                int size = atsa.Length-1;
                for (int i = 0; i < size; i++)
                {
                    string[] coord = atsa[i].Split(',');
                    double longi = Convert.ToDouble(coord[0]);
                    double lati = Convert.ToDouble(coord[1]);
                    mapdata.Add(new Windows.Devices.Geolocation.BasicGeoposition() { Latitude = lati, Longitude = longi });
                }
            }
        }
    }
}
