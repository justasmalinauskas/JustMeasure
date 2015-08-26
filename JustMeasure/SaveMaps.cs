using System;
using System.IO;
using System.Xml;

namespace JustMeasure
{
    public class SaveMaps
    {

        public async System.Threading.Tasks.Task SaveKML(System.Collections.Generic.List<Windows.Devices.Geolocation.BasicGeoposition> mapdata)
        {
            #region XML generating

            XmlDocument xDoc = new XmlDocument();
            XmlDeclaration xDec = xDoc.CreateXmlDeclaration("1.0", "utf-8", null);
         
            XmlElement kml = xDoc.CreateElement("kml");
            kml.SetAttribute("xmlns", @"http://www.opengis.net/kml/2.2");
            xDoc.InsertBefore(xDec, xDoc.DocumentElement);
            xDoc.AppendChild(kml);
            XmlElement Document = xDoc.CreateElement("Document");
            kml.AppendChild(Document);

            XmlElement name = xDoc.CreateElement("name");
            XmlText nameTextMain = xDoc.CreateTextNode(DateTime.Today.ToString());
            Document.AppendChild(name);
            name.AppendChild(nameTextMain);

            XmlElement description = xDoc.CreateElement("description");
            XmlText destext = xDoc.CreateTextNode("Created by JustMeasure app.");
            Document.AppendChild(description);
            description.AppendChild(destext);

            XmlElement Style = xDoc.CreateElement("Style");
            Style.SetAttribute("id", "style1");
            Document.AppendChild(Style);

            XmlElement LineStyle = xDoc.CreateElement("LineStyle");
            Style.AppendChild(LineStyle);

            XmlElement LineStyleColor = xDoc.CreateElement("color");
            XmlText linestylecolor = xDoc.CreateTextNode("ffffffff");
            LineStyle.AppendChild(LineStyleColor);
            LineStyleColor.AppendChild(linestylecolor);

            XmlElement LineStyleWidth = xDoc.CreateElement("width");
            XmlText linestylewidth = xDoc.CreateTextNode("5");
            LineStyle.AppendChild(LineStyleWidth);
            LineStyleWidth.AppendChild(linestylewidth);

            XmlElement Polystyle = xDoc.CreateElement("PolyStyle");
            Style.AppendChild(Polystyle);

            XmlElement polystylecolor = xDoc.CreateElement("color");
            XmlText polyscolor = xDoc.CreateTextNode("330000ff");
            Polystyle.AppendChild(polystylecolor);
            polystylecolor.AppendChild(polyscolor);

            XmlElement Placemark = xDoc.CreateElement("Placemark");
            Document.AppendChild(Placemark);

            XmlElement styleUrl = xDoc.CreateElement("styleUrl");
            XmlText styleurl = xDoc.CreateTextNode("#style1");
            Placemark.AppendChild(styleUrl);
            styleUrl.AppendChild(styleurl);

            XmlElement Polygon = xDoc.CreateElement("Polygon");
            Placemark.AppendChild(Polygon);

            XmlElement extrude = xDoc.CreateElement("extrude");
            XmlText extrudetext = xDoc.CreateTextNode("1");
            Polygon.AppendChild(extrude);
            extrude.AppendChild(extrudetext);

            XmlElement altitudeMode = xDoc.CreateElement("altitudeMode");
            XmlText relativeToGround = xDoc.CreateTextNode("relativeToGround");
            Polygon.AppendChild(altitudeMode);
            altitudeMode.AppendChild(relativeToGround);

            XmlElement outerBoundaryIs = xDoc.CreateElement("outerBoundaryIs");
            Polygon.AppendChild(outerBoundaryIs);

            XmlElement LinearRing = xDoc.CreateElement("LinearRing");
            outerBoundaryIs.AppendChild(LinearRing);

            XmlElement coordinates = xDoc.CreateElement("coordinates");
            LinearRing.AppendChild(coordinates);

            for (int i = 0; i < mapdata.Count; i++)
            {
                string coord = mapdata[i].Longitude + "," + mapdata[i].Latitude + "\n";
                XmlText cord = xDoc.CreateTextNode(coord);
                coordinates.AppendChild(cord);
            }
            #endregion
            Windows.Storage.Pickers.FileSavePicker savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("KML file", new System.Collections.Generic.List<string>() { ".kml" });
            savePicker.SuggestedFileName = DateTime.Now.ToString();
            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                Windows.Storage.CachedFileManager.DeferUpdates(file);
                using (Stream fileStream = await file.OpenStreamForWriteAsync())
                {
                   xDoc.Save(fileStream);
                }
                Windows.Storage.Provider.FileUpdateStatus status = await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    SuccessOrNot("SuccessFile");
                }
                else
                {
                    SuccessOrNot("FailFile");
                }
            }
        }
        private async void SuccessOrNot(string status)
        {
            var messageDialog = new Windows.UI.Popups.MessageDialog(new LocalizedStrings()[status]);
            messageDialog.Commands.Add(new Windows.UI.Popups.UICommand(new LocalizedStrings()["Ok"]));
            messageDialog.DefaultCommandIndex = 0;
            await messageDialog.ShowAsync();
        }
    }
}
