using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMetadata.LiveTape
{
    public class Track
    {
        public string Title { get; set; }
        public string Duration { get; set; }
        public int DiscNumber { get; set; }
        public int TrackNumber { get; set; }
        public string Notes { get; set; }
        public string FileName { get; set; }

        private string _originalFileName;

        public Track(string filePath)
        {
            _originalFileName = Path.GetFileName(filePath);
            FileName = _originalFileName;
        }
    }
}
