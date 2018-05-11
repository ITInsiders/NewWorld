using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NW.BL.DTO;

namespace NW.PL.Models
{
    public struct PlaceRating
    {
        public PlaceDTO Place { get; set; }
        public double Rating { get; set; }
    }
}