using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMetadata.IO
{
    public class FlacFileInfo : IAudioFileInfo
    {
        #region Constants
        //private const int DEFAULT_PADDING_SIZE = 2048;
        //private const int LEN_MIN_BLOCK_SIZE = 2;
        //private const int LEN_MAX_BLOCK_SIZE = 2;
        //private const int LEN_MIN_FRAME_SIZE = 3;
        //private const int LEN_MAX_FRAME_SIZE = 3;
        //private const int LEN_SAMPLE_RATE_BITS = 20;
        //private const int LEN_BYTE = 8;
        //private readonly string COM_TITLE = "TITLE";
        //private readonly string COM_VERSION = "VERSION";
        //private readonly string COM_ALBUM = "ALBUM";
        //private readonly string COM_TRACK = "TRACKNUMBER";
        //private readonly string COM_ARTIST = "ARTIST";
        //private readonly string COM_PERFORMER = "PERFORMER";
        //private readonly string COM_COPYRIGHT = "COPYRIGHT";
        //private readonly string COM_LICENSE = "LICENSE";
        //private readonly string COM_ORGANIZATION = "ORGANIZATION";
        //private readonly string COM_DESCRIPTION = "DESCRIPTION";
        //private readonly string COM_GENRE = "GENRE";
        //private readonly string COM_DATE = "DATE";
        //private readonly string COM_LOCATION = "LOCATION";
        //private readonly string COM_CONTACT = "CONTACT";
        //private readonly string COM_ISRC = "ISRC";
        #endregion

        #region Private Members
        private string _filePath { get; set; }
        /// <summary>
        /// This bool is true if the padding block has been exhausted and the entire file must be rewritten
        /// </summary>
        private bool _metadataOverflow { get; set; }
        //private string _title { get; set; }
        //private string _artist { get; set; }
        //private string _album { get; set; }
        //private string _year { get; set; }
        //private string _comment { get; set; }
        //private int _track { get; set; }
        #endregion

        public string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                _filePath = value;
            }
        }
        public string Title
        {
            get
            {
                return VorbisCommentBlock.GetComment(FlacConstants.COM_TITLE);
            }
            set
            {
                UpdateVorbisComments(FlacConstants.COM_TITLE, value);
            }
        }
        public string Artist
        {
            get
            {
                return VorbisCommentBlock.GetComment(FlacConstants.COM_ARTIST);
            }
            set
            {
                UpdateVorbisComments(FlacConstants.COM_ARTIST, value);
            }
        }
        public string Album
        {
            get
            {
                return VorbisCommentBlock.GetComment(FlacConstants.COM_ALBUM);
            }
            set
            {
                UpdateVorbisComments(FlacConstants.COM_ALBUM, value);
            }
        }
        public string Year
        {
            get
            {
                return VorbisCommentBlock.GetComment(FlacConstants.COM_YEAR);
            }
            set
            {
                UpdateVorbisComments(FlacConstants.COM_YEAR, value);
            }
        }
        public string Comment
        {
            get
            {
                return VorbisCommentBlock.GetComment(FlacConstants.COM_COMMENT);
            }
            set
            {
                UpdateVorbisComments(FlacConstants.COM_COMMENT, value);
            }
        }
        public int Track
        {
            get
            {
                return System.Convert.ToInt32(VorbisCommentBlock.GetComment(FlacConstants.COM_TRACKNUMBER));
            }
            set
            {
                UpdateVorbisComments(FlacConstants.COM_TRACKNUMBER, value.ToString());
            }
        }

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



        public int MetadataLengthInBytes { get; set; }
        public FlacStreamInfoBlock StreamInfoBlock { get; set; }
        public FlacSeektableBlock SeekTableBlock { get; set; }
        public FlacVorbisCommentBlock VorbisCommentBlock { get; set; }
        public FlacPaddingBlock PaddingBlock { get; set; }public List<IFlacMetadataBlock> OtherBlocks { get; set; }

        public FlacFileInfo()
        {
            OtherBlocks = new List<IFlacMetadataBlock>();
            _metadataOverflow = false;
        }
        public FlacFileInfo(string filePath)
            : this()
        {
            _filePath = filePath;
        }

        private void UpdateVorbisComments(string fieldName, string data)
        {
            int blockLengthDifference = VorbisCommentBlock.AddComment(string.Format("{0}={1}", fieldName, data), true);
            PaddingBlock.Header.BlockLengthInBytes -= blockLengthDifference;
        }
    }
}
