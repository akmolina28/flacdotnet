using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMetadata.IO
{
    public class FlacFileInfo : IAudioFileInfo
    {
        public string FilePath { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Year { get; set; }
        public string Comment { get; set; }
        public int Track { get; set; }

        public int MetadataLengthInBytes { get; set; }
        public FlacStreamInfoBlock StreamInfoBlock { get; set; }
        public FlacSeektableBlock SeekTableBlock { get; set; }
        public FlacVorbisCommentBlock VorbisCommentBlock { get; set; }
        public FlacPaddingBlock PaddingBlock { get; set; }
    }
}
