using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NW.PL.Models
{
    public class ZonesLoad
    {
        //public class Features
        //{
        //    public string type = "Feature";
        //    public int id { get; set; }
        //    public Geometry geometry { get; set; }
        //    public Options options { get; set; }
        //    public Properties properties { get; set; }
        //}
        //public class Geometry
        //{
        //    public string type = "Polygon";
        //    public List<double[]>[] coordinates = new List<double[]>[1] { new List<double[]>() };
        //}
        //public class Options
        //{
        //    public string strokeColor = "#FFFFFF";
        //    public string fillColor { get; set; }
        //}
        //public class Properties
        //{
        //    public string name { get; set; }
        //    public DateTime dateTime { get; set; }
        //}

        //public string type = "FeatureCollection";
        //public List<Features> features = new List<Features>();
            public double ArgLongtude { get; set; }
            public double ArgLatitude { get; set; }
            public double[][] Bounds { get; set; }
            public string Name { get; set; }
            public List<double[]> coordinates = new List<double[]>();
            public string fillColor { get; set; }
    }
}