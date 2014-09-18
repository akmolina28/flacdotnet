using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMetadata.IO
{
    public class FlacVorbisComment
    {
        private string _fieldName { get; set; }
        private string _data { get; set; }

        public string FieldName
        {
            get
            {
                return _fieldName;
            }
        }
        public string Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }

        public FlacVorbisComment(string text)
        {
            bool valid = false;
            string fieldName = "";
            string data = "";
            if (text.Contains('='))
            {
                int i = text.IndexOf('=');
                if (i > 0)
                {
                    fieldName = text.Substring(0, i);
                    data = text.Substring(i + 1);
                    valid = true;
                }
            }
            if (!valid)
            {
                throw new System.IO.IOException("Invalid vorbis comment format!");
            }
            SetFieldNameAndData(fieldName, data);
        }

        public FlacVorbisComment(string fieldName, string data)
        {
            SetFieldNameAndData(fieldName, data);
        }

        private void SetFieldNameAndData(string fieldName, string data)
        {
            foreach (char c in fieldName)
            {
                if (c < 0x20 || c > 0x7D || c == 0x3D || c == 0x7E)
                {
                    throw new Exception("Field name contains one or more invalid characters.");
                }
            }
            _fieldName = fieldName;
            _data = data;
        }

        public override string ToString()
        {
            return string.Format("{0}={1}", _fieldName, _data);
        }
    }
}
