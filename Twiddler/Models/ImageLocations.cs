using System;

namespace Twiddler.Models
{
    public struct ImageLocations
    {
        public Uri Thumbnail { get; set; }
        public Uri FullSize { get; set; }
        public Uri Link { get; set; }
    }
}