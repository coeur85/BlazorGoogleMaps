using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleMapsComponents.Maps
{
    public class AdvancedMarkerOptions : ListableEntityOptionsBase
    {

        public string Content { get; set; }
        public LatLngLiteral Position { get; set; }
        public string Title { get; set; }
    }
}
