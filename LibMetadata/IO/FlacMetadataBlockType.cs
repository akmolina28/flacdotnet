using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMetadata.IO
{
    public enum FlacMetadataBlockType
    {
        STREAMINFO = 0,
        PADDING = 1,
        APPLICATION = 2,
        SEEKTABLE = 3,
        VORBIS_COMMENT = 4,
        CUESHEET = 5,
        PICTURE = 6
    }
}
