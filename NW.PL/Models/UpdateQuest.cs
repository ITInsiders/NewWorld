using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NW.PL.Models
{
    public class UpdateQuest
    {
        public class Point
        {
            public string Address { get; set; }
            public string Task { get; set; }
            public string Longitude { get; set; }
            public string Latitude { get; set; }

            public double getLongitude() => Convert.ToDouble(Longitude.Replace('.', ','));
            public double getLatitude() => Convert.ToDouble(Latitude.Replace('.', ','));
        }

        public class Prize
        {
            public int Min { get; set; }
            public int Max { get; set; }
            public string Title { get; set; }
        }

        public int? Id { get; set; }
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
        public List<Point> Points { get; set; }
        public string Description { get; set; }
        public List<Prize> Prizes { get; set; }
        public string MaxPeople { get; set; }
        public HttpPostedFileBase Photo { get; set; }
    }
}