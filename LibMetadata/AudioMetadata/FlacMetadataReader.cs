using FlacDotnetLib.AudioMetadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlacDotnetLib.AudioMetadata
{
    public class FlacMetadataReader : IMetadataReader
    {
        public string FileName { get; set; }

        #region Constants
        public enum BlockType
        {
            STREAMINFO = 0,
            PADDING = 1,
            APPLICATION = 2,
            SEEKTABLE = 3,
            VORBIS_COMMENT = 4,
            CUESHEET = 5,
            PICTURE = 6,
            INVALID = 127
        }
        private const int LEN_MIN_BLOCK_SIZE = 2;
        private const int LEN_MAX_BLOCK_SIZE = 2;
        private const int LEN_MIN_FRAME_SIZE = 3;
        private const int LEN_MAX_FRAME_SIZE = 3;
        private const int LEN_SAMPLE_RATE_BITS = 20;
        private const int LEN_BYTE = 8;
        private readonly string COM_TITLE = "TITLE=";
        private readonly string COM_VERSION = "VERSION=";
        private readonly string COM_ALBUM = "ALBUM=";
        private readonly string COM_TRACKNUMBER = "TRACKNUMBER=";
        private readonly string COM_ARTIST = "ARTIST=";
        private readonly string COM_PERFORMER = "PERFORMER=";
        private readonly string COM_COPYRIGHT = "COPYRIGHT=";
        private readonly string COM_LICENSE = "LICENSE=";
        private readonly string COM_ORGANIZATION = "ORGANIZATION=";
        private readonly string COM_DESCRIPTION = "DESCRIPTION=";
        private readonly string COM_GENRE = "GENRE=";
        private readonly string COM_DATE = "DATE=";
        private readonly string COM_LOCATION = "LOCATION=";
        private readonly string COM_CONTACT = "CONTACT=";
        private readonly string COM_ISRC = "ISRC=";
        #endregion

        #region Flac Blocks
        // streaminfo block
        // -------------------------------------
        public int MinimumBlockSize { get; private set; }
        public int MaximumBlockSize { get; private set; }
        public int MinimumFrameSize { get; private set; }
        public int MaximumFrameSize { get; private set; }
        public int SampleRateHz { get; private set; }
        public int NumberOfChannels { get; private set; }
        public int BitsPerSample { get; private set; }
        public long SamplesPerStream { get; private set; }
        public byte[] Signature { get; private set; }
        // -------------------------------------

        // padding block
        // -------------------------------------
        public int PaddingLengthInBytes { get; private set; }
        // -------------------------------------


        // flac comments block
        // -------------------------------------
        public string VendorComment { get; private set; }
        public List<string> Comments { get; private set; }
        // -------------------------------------
        #endregion

        #region Private Variables
        private int _totalMetadataSpace = 0;
        private string _vendorString;
        private long _flacCommentPosition;
        #endregion

        public IAudioMetadata ReadMetadata()
        {
            throw new NotImplementedException();
        }

        #region Private Methods

        #endregion
    }
}
