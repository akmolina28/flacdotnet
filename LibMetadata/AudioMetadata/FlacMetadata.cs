using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlacDotnetLib.AudioMetadata
{
    public class FlacMetadata : IAudioMetadata
    {
        // default tags
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Year { get; set; }
        public string Comment { get; set; }
        public int Track { get; set; }

        // flac tags
        public string Version { get; set; }
        public string Performer { get; set; }
        public string Copyright { get; set; }
        public string License { get; set; }
        public string Organization { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public string Date { get; set; }
        public string Location { get; set; }
        public string Contact { get; set; }
        public string ISRC { get; set; }
        public System.Collections.Specialized.NameValueCollection CustomAttributes { get; set; }
    }
}
