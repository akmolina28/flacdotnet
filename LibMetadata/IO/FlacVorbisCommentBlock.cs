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
        private List<FlacVorbisComment> _comments { get; set; }

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
        //public List<FlacVorbisComment> VorbisComments // todo: wrap this into an object to restrict ICollection functions
        //{
        //    get
        //    {
        //        return _comments;
        //    }
        //}

        public FlacMetadataBlockHeader Header { get; set; }

        public void ReadBlockData(BitReader reader)
        {
            reader.IsLittleEndian = true; // all integers are encoded little endian following the oggvorbis spec
            VendorLength = reader.ReadInt32(32);
            reader.IsLittleEndian = false;

            VendorString = reader.ReadString(VendorLength, 8);

            reader.IsLittleEndian = true;
            UserCommentListLength = reader.ReadInt32(32);

            _comments = new List<FlacVorbisComment>();
            for (int i = 0; i < UserCommentListLength; i++)
            {
                reader.IsLittleEndian = true;
                int commentLength = reader.ReadInt32(32);
                reader.IsLittleEndian = false;

                FlacVorbisComment nextComment = new FlacVorbisComment(reader.ReadString(commentLength, 8));
                _comments.Add(nextComment);
            }
        }

        public void AddComment(string text, bool overwrite = false)
        {
            FlacVorbisComment newComment = new FlacVorbisComment(text);
            int newLength = newComment.ToString().Length;
            int blockLengthDifference = newLength;

            if (overwrite && _comments.Any(t => t.FieldName == newComment.FieldName))
            {
                FlacVorbisComment existingComment = _comments.First(t => t.FieldName == newComment.FieldName);
                int oldLength = existingComment.ToString().Length;
                existingComment.Data = newComment.Data;
                blockLengthDifference = newLength - oldLength;
            }
            else
            {
                _comments.Add(newComment);
            }

            Header.BlockLengthInBytes += blockLengthDifference;
        }

        public string GetComment(string fieldName)
        {
            string ret = null;
            if (_comments.Any(t => t.FieldName == fieldName))
            {
                ret = _comments.First(t => t.FieldName == fieldName).Data;
            }
            return ret;
        }
    }
}
