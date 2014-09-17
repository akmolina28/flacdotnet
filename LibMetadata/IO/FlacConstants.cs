using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMetadata.IO
{
    public static class FlacConstants
    {
        public static readonly int LEN_MIN_BLOCK_SIZE = 2;
        public static readonly int LEN_MAX_BLOCK_SIZE = 2;
        public static readonly int LEN_MIN_FRAME_SIZE = 3;
        public static readonly int LEN_MAX_FRAME_SIZE = 3;
        public static readonly int LEN_SAMPLE_RATE_BITS = 20;
        public static readonly int LEN_BYTE = 8;
        public static readonly string COM_TITLE = "TITLE=";
        public static readonly string COM_VERSION = "VERSION=";
        public static readonly string COM_ALBUM = "ALBUM=";
        public static readonly string COM_TRACKNUMBER = "TRACKNUMBER=";
        public static readonly string COM_ARTIST = "ARTIST=";
        public static readonly string COM_PERFORMER = "PERFORMER=";
        public static readonly string COM_COPYRIGHT = "COPYRIGHT=";
        public static readonly string COM_LICENSE = "LICENSE=";
        public static readonly string COM_ORGANIZATION = "ORGANIZATION=";
        public static readonly string COM_DESCRIPTION = "DESCRIPTION=";
        public static readonly string COM_GENRE = "GENRE=";
        public static readonly string COM_DATE = "DATE=";
        public static readonly string COM_LOCATION = "LOCATION=";
        public static readonly string COM_CONTACT = "CONTACT=";
        public static readonly string COM_ISRC = "ISRC=";
    }
}
