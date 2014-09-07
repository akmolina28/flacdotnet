using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMetadata.IO
{
    /// <summary>
    /// This block is for storing a list of human-readable name/value pairs. Values are encoded using UTF-8. It is an implementation of the Vorbis comment specification (without the framing bit). This is the only officially supported tagging mechanism in FLAC. There may be only one VORBIS_COMMENT block in a stream. In some external documentation, Vorbis comments are called FLAC tags to lessen confusion.
    /// </summary>
    public class FlacVorbisCommentBlock : IFlacMetadataBlock
    {
        /// <summary>
        /// 32-bit integer length of the vendor string.
        /// </summary>
        public int VendorLength { get; set; }
        /// <summary>
        /// UTF-8 string as [VendorLength] octets.
        /// </summary>
        public string VendorString { get; set; }
        /// <summary>
        /// 32-bit integer number of comments.
        /// </summary>
        public int UserCommentListLength { get; set; }
        /// <summary>
        /// User comments.
        /// </summary>
        public List<FlacVorbisComment> VorbisComments { get; set; }

        public FlacMetadataBlockHeader Header { get; set; }

        public void ReadBlockData(BitReader reader)
        {
            reader.IsLittleEndian = true; // all integers are encoded little endian following the oggvorbis spec
            VendorLength = reader.ReadInt32(32);
            reader.IsLittleEndian = false;

            VendorString = reader.ReadString(VendorLength, 8);

            reader.IsLittleEndian = true;
            UserCommentListLength = reader.ReadInt32(32);

            VorbisComments = new List<FlacVorbisComment>();
            for (int i = 0; i < UserCommentListLength; i++)
            {
                reader.IsLittleEndian = true;
                int commentLength = reader.ReadInt32(32);
                reader.IsLittleEndian = false;

                FlacVorbisComment nextComment = new FlacVorbisComment()
                {
                    Length = commentLength,
                    Comment = reader.ReadString(commentLength, 8)
                };
                VorbisComments.Add(nextComment);
            }
        }
    }
}
