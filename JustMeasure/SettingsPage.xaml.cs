using System;
using System.Collections.Generic;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JustMeasure
{
    public sealed partial class SettingsPage : Page
    {
        MeasureUnits ma = new MeasureUnits();
        List<string> area = new MeasureUnits().GetAreaList();
        List<string> distance = new MeasureUnits().GetDistanceList();

        public SettingsPage()
        {
            this.InitializeComponent();
            SettingsName();
            SettingsShow();
        }

        private void SettingsShow()
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            AreaSet.ItemsSource = area;
            int ar = Convert.ToInt32(localSettings.Values["AreaSettings"]);
            if (0 <= ar && ar < area.Count)
                AreaSet.SelectedIndex = ar;
            else
                AreaSet.SelectedIndex = 0;

            DisSet.ItemsSource = distance;
            int dis = Convert.ToInt32(localSettings.Values["DistanceSettings"]);
            if (0 <= dis && dis < distance.Count)
                DisSet.SelectedIndex = dis;
            else
                DisSet.SelectedIndex = 0;
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
        private void SettingsName()
        {
            AppName.Text = new LocalizedStrings()["Application"] + ": " + Windows.ApplicationModel.Package.Current.DisplayName;
            Author.Text = new LocalizedStrings()["Author"] + ": " + Windows.ApplicationModel.Package.Current.PublisherDisplayName;
            Date.Text = new LocalizedStrings()["BuildDate"] + ": " + Windows.ApplicationModel.Package.Current.InstalledDate.ToString();
            Version.Text = new LocalizedStrings()["Version"] + ": " + GetApplicationVersion();
            Build.Text = new LocalizedStrings()["Build"] + ": " + GetApplicationBuild();

        }
        public string GetApplicationVersion()
        {
            var ver = Windows.ApplicationModel.Package.Current.Id.Version;
            return ver.Major.ToString() + "." + ver.Minor.ToString();
        }

        public string GetApplicationBuild()
        {
            var ver = Windows.ApplicationModel.Package.Current.Id.Version;
            return ver.Build.ToString();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (s, a) =>
            {
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                    a.Handled = true;
                }
            };
        }

        private void DisSet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values["DistanceSettings"] = DisSet.SelectedIndex;
        }

        private void AreaSet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values["AreaSettings"] = AreaSet.SelectedIndex;
        }

        private void ScreenToggle_Toggled(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (ScreenToggle.IsOn == true)
            {
                 localSettings.Values["KeepScreenOn"] = "1";//On
            }
            else
            {
                localSettings.Values["KeepScreenOn"] = "0";//Off
            }
        }

        private void MapMode_Toggled(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (ScreenToggle.IsOn == true)
            {
                localSettings.Values["MapMode"] = "1";//AerialMode
            }
            else
            {
                localSettings.Values["MapMode"] = "0";//MapMode
            }
        }

        private void AboutClose_Click(object sender, RoutedEventArgs e)
        {
            AboutButton.Flyout.Hide();
        }

        private void MapMode_Loaded(object sender, RoutedEventArgs e)
        {
            string sc = Windows.Storage.ApplicationData.Current.LocalSettings.Values["KeepScreenOn"].ToString();
            if (sc == "1")
                ScreenToggle.IsOn = true;
            else
                ScreenToggle.IsOn = false;
        }

        private void ScreenToggle_Loaded(object sender, RoutedEventArgs e)
        {
            string mm = Windows.Storage.ApplicationData.Current.LocalSettings.Values["Map"].ToString();
            if (mm == "1")
                ScreenToggle.IsOn = true;
            else
                ScreenToggle.IsOn = false;
        }
    }
}