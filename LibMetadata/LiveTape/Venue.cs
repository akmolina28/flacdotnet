using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMetadata.LiveTape
{
    public enum VenueType
    {
        IndoorArena,
        OutdoorArena,
        Amphitheater,
        Bar,
        MusicHall,
        ConcertHall,
        Club
    }

    public class Venue
    {
        public string Name { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public VenueType Type { get; set; }
    }
}
