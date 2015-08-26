using System;
using System.Collections.Generic;
using readsets = Windows.Storage.ApplicationData;

namespace JustMeasure
{
    public class MeasureUnits
    {
        List<string> area = new List<string>()
        {
            new LocalizedStrings()["m2"] + " - " + new LocalizedStrings()["m2text"],
            new LocalizedStrings()["ha"] + " - " + new LocalizedStrings()["hatext"],
            new LocalizedStrings()["km2"] + " - " + new LocalizedStrings()["km2text"],
            new LocalizedStrings()["ft2"] + " - " + new LocalizedStrings()["ft2text"],
            new LocalizedStrings()["yd2"] + " - " + new LocalizedStrings()["yd2text"],
            new LocalizedStrings()["mi2"] + " - " + new LocalizedStrings()["mi2text"]
        };

        List<string> distance = new List<string>()
        {
            new LocalizedStrings()["m"] + " - " + new LocalizedStrings()["mtext"],
            new LocalizedStrings()["km"] + " - " + new LocalizedStrings()["kmtext"],
            new LocalizedStrings()["mi"] + " - " + new LocalizedStrings()["mitext"],
            new LocalizedStrings()["ft"] + " - " + new LocalizedStrings()["fttext"],
            new LocalizedStrings()["yd"] + " - " + new LocalizedStrings()["ydtext"],
            new LocalizedStrings()["ch"] + " - " + new LocalizedStrings()["chtext"],
        };

        public List<string> GetAreaList()
        {
            return area;
        }

        public List<string> GetDistanceList()
        {
            return distance;
        }

        public string AreaUnits()
        {
            int areaset = Convert.ToInt32(readsets.Current.LocalSettings.Values["AreaSettings"]);
            if (areaset == 0)
            {
                return new LocalizedStrings()["m2"];
            }
            if (areaset == 1)
            {
                return new LocalizedStrings()["ha"];
            }
            if (areaset == 2)
            {
                return new LocalizedStrings()["km2"];
            }
            if (areaset == 3)
            {
                return new LocalizedStrings()["ft2"];
            }
            if (areaset == 4)
            {
                return new LocalizedStrings()["yd2"];
            }
            if (areaset == 5)
            {
                return new LocalizedStrings()["mi2"];
            }
            else
            {
                return new LocalizedStrings()["m2"];
            }
        }
        public double areax()
        {
            int areaset = Convert.ToInt32(readsets.Current.LocalSettings.Values["AreaSettings"]);
            if (areaset == 0)
            {
                return 1;
            }
            if (areaset == 1)
            {
                return 0.0001;
            }
            if (areaset == 2)
            {
                return 0.000001;
            }
            if (areaset == 3)
            {
                return 10.76386870663506;
            }
            if (areaset == 4)
            {
                return 1.19599004630108;
            }
            if (areaset == 5)
            {
                return 1 / 2589988.11;
            }
            else
            {
                return 1;
            }
        }
        public double distancex()
        {
            int disset = Convert.ToInt32(readsets.Current.LocalSettings.Values["DistanceSettings"]);
            if (disset == 0)
            {
                return 1;
            }
            if (disset == 1)
            {
                return 0.001;
            }
            if (disset == 2)
            {
                return 0.621371192237334 / 1000;
            }
            if (disset == 3)
            {
                return 3.280839895013123;
            }
            if (disset == 4)
            {
                return 1.093613298337708;
            }
            if (disset == 5)
            {
                return 0.0497096953789867;
            }
            else
            {
                return 1;
            }
        }
        public string DistanceUnits()
        {
            int disset = Convert.ToInt32(readsets.Current.LocalSettings.Values["DistanceSettings"]);
            if (disset == 0)
            {
                return new LocalizedStrings()["m"];
            }
            if (disset == 1)
            {
                return new LocalizedStrings()["km"];
            }
            if (disset == 2)
            {
                return new LocalizedStrings()["mi"];
            }
            if (disset == 3)
            {
                return new LocalizedStrings()["ft"];
            }
            if (disset == 4)
            {
                return new LocalizedStrings()["yd"];
            }
            if (disset == 5)
            {
                return new LocalizedStrings()["ch"];
            }
            else
            {
                return new LocalizedStrings()["m"];
            }
        }
    }
}
