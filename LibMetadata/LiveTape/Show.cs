using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMetadata.LiveTape
{
    public class Show
    {
        public Artist Artist { get; set; }
        public Venue Venue { get; set; }
        public DateTime Date { get; set; }
        public string Comments { get; set; }
        public List<Track> SetList { get; set; }

        public Show()
        {
            SetList = new List<Track>();
        }
    }
}
