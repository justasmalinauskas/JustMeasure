using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Services.Maps;
using Windows.Devices.Geolocation;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.System;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Storage;

namespace JustMeasure
{
    /// <summary>
    /// Main Page of application
    /// </summary>
    public sealed partial class MainPage : Page
    {
        List<BasicGeoposition> geo = new List<BasicGeoposition>();
        private Geolocator _geolocator = null;
        private IList<Data> duom = new List<Data>();
        private Int32 lapuerm;
        double area = 0;
        double distance = 0;
        MapPolygon po = new MapPolygon();
        private Windows.System.Display.DisplayRequest dispRequest = null;
        MeasureUnits ma = new MeasureUnits();

        public MainPage()
        {
            this.InitializeComponent();
            Sekimas();
            UpdateData();
            MapService.ServiceToken = new SecretConfigs().MapToken;
        }
        private async void GetPos()
        {
            var pos = await _geolocator.GetGeopositionAsync();
            MapCtrl.Center = pos.Coordinate.Point;
            MapCtrl.ZoomLevel = 14;
            IList<BasicGeoposition> POS = new List<BasicGeoposition>();
            POS.Add(new BasicGeoposition { Latitude = pos.Coordinate.Point.Position.Latitude, Longitude = pos.Coordinate.Point.Position.Longitude });
            await MapCtrl.TrySetViewAsync(pos.Coordinate.Point);
        }

        private void Tracking_Click(object sender, RoutedEventArgs e)
        {
            if (Tracking.IsChecked == true)
            {
                _geolocator.PositionChanged += OnPositionChanged;
                if (dispRequest == null)
                {
                    if (ApplicationData.Current.LocalSettings.Values["KeepScreenOn"].ToString() == "1")
                    {
                        dispRequest = new Windows.System.Display.DisplayRequest();
                        dispRequest.RequestActive();
                    }
                }
                Clear.IsEnabled = false;
            }
            else
            {
                _geolocator.PositionChanged -= OnPositionChanged;
                if (dispRequest != null)
                {
                    dispRequest.RequestRelease();
                    dispRequest = null;
                }
                Clear.IsEnabled = true;
            }
        }

        private async void Clear_Click(object sender, RoutedEventArgs e)
        {
            await QDelete1();
            UpdateData();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(SettingsPage));
            }
        }

        private void ClearData()
        {
            MapCtrl.MapElements.Clear();
            duom.Clear();
            geo.Clear();
            area = 0;
            distance = 0;
            lapuerm = 0;
            GetPos();
        }

        private async void Sekimas()
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:
                    Tracking.IsEnabled = true;
                    _geolocator = new Geolocator { DesiredAccuracyInMeters = 10, ReportInterval = 1000 };
                    _geolocator.StatusChanged += OnStatusChanged;
                    break;

                case GeolocationAccessStatus.Denied:
                    Tracking.IsEnabled = false;
                    Tracking.IsChecked = false;
                    Status.Text = new LocalizedStrings()["LocationDisabled"];
                    NoGPS();
                    break;

                case GeolocationAccessStatus.Unspecified:
                    break;
            }
        }

        async private void OnPositionChanged(Geolocator sender, PositionChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                AddPoint(e.Position);
                if (lapuerm != duom.Count && duom.Count > 1)
                {
                    var var1 = duom[lapuerm].X - duom[0].X;
                    for (double var3 = duom[lapuerm].Y - duom[0].Y; lapuerm < duom.Count - 1; lapuerm++)
                    {
                        double var5 = duom[lapuerm + 1].X - duom[0].X;
                        double var7 = duom[lapuerm + 1].Y - duom[0].Y;
                        distance += Math.Sqrt((var5 - var1) * (var5 - var1) + (var7 - var3) * (var7 - var3));
                        if (this.lapuerm != 0)
                        {
                            area += ((var1 * var7 - var3 * var5) / 2);
                        }
                        var1 = var5;
                        var3 = var7;
                    }
                }
                UpdateData();
            });
            await MapCtrl.TrySetViewBoundsAsync(GeoboundingBox.TryCompute(geo), null, MapAnimationKind.Default);
        }

        async private void OnStatusChanged(Geolocator sender, StatusChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                switch (e.Status)
                {
                    case PositionStatus.Ready:
                        Tracking.IsEnabled = true;
                        GetPos();
                        Status.Text = new LocalizedStrings()["LocationFound"];
                        break;

                    case PositionStatus.Initializing:
                        if (Tracking.IsChecked == false)
                            Tracking.IsEnabled = false;
                        Status.Text = new LocalizedStrings()["LocationInitializing"];
                        break;

                    case PositionStatus.NoData:
                        if (Tracking.IsChecked == false)
                            Tracking.IsEnabled = false;
                        break;

                    case PositionStatus.Disabled:
                        Tracking.IsEnabled = false;
                        Tracking.IsChecked = false;
                        Status.Text = new LocalizedStrings()["LocationDisabled"];
                        NoGPS();
                        break;

                    case PositionStatus.NotInitialized:
                        if (Tracking.IsChecked == false)
                            Tracking.IsEnabled = false;
                        break;

                    case PositionStatus.NotAvailable:
                        if (Tracking.IsChecked == false)
                            Tracking.IsEnabled = false;
                        break;

                    default:
                        if (Tracking.IsChecked == false)
                            Tracking.IsEnabled = false;
                        break;
                }
            });
        }

        private void UpdateData()
        {
            distanceLabel.Text = new LocalizedStrings()["Distance"] + ": " + (Math.Round(distance * ma.distancex(), 2) + ma.DistanceUnits()).ToString();
            areaLabel.Text = new LocalizedStrings()["Area"] + ": " + (Math.Round(area * ma.areax() * Teig(), 2) + ma.AreaUnits()).ToString();
        }
        private async void NoGPS()
        {
            var messageDialog = new MessageDialog(new LocalizedStrings()["LocationDisabled"]);
            messageDialog.Commands.Add(new UICommand(new LocalizedStrings()["TurnOnGps"], new UICommandInvokedHandler(this.GoToSettings)));
            messageDialog.Commands.Add(new UICommand(new LocalizedStrings()["Cancel"]));
            messageDialog.DefaultCommandIndex = 0;
            messageDialog.CancelCommandIndex = 1;
            await messageDialog.ShowAsync();
        }
        private async void GoToSettings(IUICommand command)
        {
            bool result = await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-location"));
        }

        public void AddPoint(Geoposition p)
        {
            double lati = p.Coordinate.Point.Position.Latitude * (System.Math.PI * 6378137 / 180);
            double longi = p.Coordinate.Point.Position.Longitude * (System.Math.PI * 6378137 / 360);
            duom.Add(new Data(lati, longi));
            geo.Add(new BasicGeoposition() { Latitude = p.Coordinate.Point.Position.Latitude, Longitude = p.Coordinate.Point.Position.Longitude });
            MapCtrl.MapElements.Remove(po);
            po.Path = new Geopath(geo);
            MapCtrl.MapElements.Add(po);
        }
        private int Teig()
        {
            if (area < 0)
                return -1;
            else
                return 1;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            int ss = Convert.ToInt32(localSettings.Values["KeepScreenOn"]);
            if (ss != 0 || ss != 1)
                localSettings.Values["KeepScreenOn"] = "1";
            int mm = Convert.ToInt32(localSettings.Values["MapMode"]);
            if (ss != 0 || ss != 1)
                localSettings.Values["MapMode"] = "0";
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            UpdateData();
            int mapmode = Convert.ToInt32(localSettings.Values["MapMode"]);
            if (mapmode == 0)
                MapCtrl.Style = MapStyle.Terrain;
            if (mapmode == 1)
                MapCtrl.Style = MapStyle.Aerial;
            else
                MapCtrl.Style = MapStyle.Terrain;
        }

        private async void SaveToKML_Click(object sender, RoutedEventArgs e)
        {
            await new SaveMaps().SaveKML(geo);
        }

        private async void LoadFromKML_Click(object sender, RoutedEventArgs e)
        {
            if (geo.Count > 0 || duom.Count > 0)
            {
                await QDelete();

            } 
            else
                LoadMap();
        }

        private async System.Threading.Tasks.Task QDelete1()
        {
            var messageDialog = new MessageDialog(new LocalizedStrings()["DeleteMapData"]);
            messageDialog.Commands.Add(new UICommand(new LocalizedStrings()["Yes"], new UICommandInvokedHandler(this.PreLoadpt1)));
            messageDialog.Commands.Add(new UICommand(new LocalizedStrings()["No"]));
            messageDialog.DefaultCommandIndex = 0;
            messageDialog.CancelCommandIndex = 1;
            await messageDialog.ShowAsync();
        }

        private async System.Threading.Tasks.Task QDelete()
        {
            var messageDialog = new MessageDialog(new LocalizedStrings()["DeleteMapData"]);
            messageDialog.Commands.Add(new UICommand(new LocalizedStrings()["Yes"], new UICommandInvokedHandler(this.PreLoadpt2)));
            messageDialog.Commands.Add(new UICommand(new LocalizedStrings()["No"]));
            messageDialog.DefaultCommandIndex = 0;
            messageDialog.CancelCommandIndex = 1;
            await messageDialog.ShowAsync();
        }
        private void PreLoadpt2(IUICommand ui)
        {
            ClearData();
            LoadMap();
        }
        private void PreLoadpt1(IUICommand ui)
        {
            ClearData();
            LoadMap();
        }

        private async void LoadMap()
        {
            await new LoadMaps().LoadKML(geo);
            if (geo.Count < 0)
            {
                po.Path = new Geopath(geo);
                MapCtrl.MapElements.Add(po); ;
            }
            
        }
    }
}
