using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMetadata.IO
{
    public class BitWriter
    {
        private Stream _stream;
        private BitArray _currentByte;
        private int _bitOffset;
        
        public Encoding Encoding { get; set; }
        public bool IsLittleEndian { get; set; }

        public long ByteOffset
        {
            get
            {
                return _stream.Position;
            }
        }

        public long BitOffset
        {
            get
            {
                return _bitOffset + (ByteOffset * 8);
            }
        }



        public BitWriter() 
        {
            Encoding = Encoding.UTF8;
            IsLittleEndian = false;
        }

        public BitWriter(Stream stream) : this()
        {
            _stream = stream;
            if (_stream.Length > 0)
            {
                _currentByte = new BitArray(new byte[] { PeekByte() });
            }
            else
            {
                _currentByte = new BitArray(8, false);
            }
        }

        //public void WriteLong(long l, int numBits)
        //{
        //    if (numBits > 64 || numBits < 1)
        //    {
        //        throw new Exception();
        //    }
        //    byte[] bytes = BitConverter.GetBytes(l);
        //    if (IsLittleEndian)
        //    {
        //        bytes = ReverseBytes(bytes);
        //    }
        //    BitArray bits = new BitArray(bytes);
        //    for (int j = numBits - 1; j >= 0; j--)
        //    {
        //        WriteBit(bits[j]);
        //    }
        //}

        //public void WriteInt(int i, int numBits)
        //{
        //    if (numBits > 32 || numBits < 1)
        //    {
        //        throw new Exception();
        //    }

        //    byte[] bytes = BitConverter.GetBytes(i);

        //    WriteBytes(bytes, numBits);
        //}

        public void WriteBytes(byte[] bytes, int lengthInBits)
        {
            if (IsLittleEndian)
            {
                bytes = ReverseBytes(bytes);
                int length = bytes.Length * 8;

                BitArray leBits = new BitArray(bytes);
                for (int j = length - 1; j >= length - lengthInBits; j--)
                {
                    WriteBit(leBits[j]);
                }
            }
            else
            {
                BitArray beBits = new BitArray(bytes);
                for (int j = lengthInBits - 1; j >= 0; j--)
                {
                    WriteBit(beBits[j]);
                }
            }
        }

        private byte[] ReverseBytes(byte[] bytes)
        {
            byte[] ret = new byte[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
            {
                ret[i] = bytes[bytes.Length - 1 - i];
            }
            return ret;
        }

        public void GoToPosition(long positionInBits)
        {
            SetPosition(positionInBits);
            _currentByte = new BitArray(new byte[] { PeekByte() });
        }

        private void SetPosition(long positionInBits)
        {
            long bytes = positionInBits / 8;
            _stream.Position = bytes;
            int bits = Convert.ToInt32(positionInBits - bytes * 8);
            _bitOffset = bits;
        }

        public void WriteBit(bool bit)
        {
            if (_bitOffset == 8)
            {
                byte[] b = new byte[1];
                _currentByte.CopyTo(b, 0);
                _stream.WriteByte(b[0]);
                _bitOffset = 0;

                b[0] = PeekByte();
                _currentByte = new BitArray(b);

            }
            //bool value = (_currentByte & (1 << (7 - _currentBit))) > 0;
#if DEBUG
            bool oldBit = _currentByte[7 - _bitOffset];
#endif
            _currentByte[7 - _bitOffset] = bit;
            _bitOffset++;
        }

        public long GetPosition()
        {
            return ((_stream.Position) * 8) + _bitOffset;
        }

        private byte PeekByte()
        {
            long pos = GetPosition();
            byte b = ReadByte();
            SetPosition(pos);
            return b;
        }

        private byte ReadByte()
        {
            byte b = (byte)_stream.ReadByte();
            return b;
        }

        public void WriteChar(char c)
        {
            byte[] bytes = Encoding.GetBytes(new char[] { c });
            BitArray bar = new BitArray(bytes);
            for (int i = bar.Length - 1; i >= 0; i--)
            {
                WriteBit(bar[i]);
            }
        }

        public void WriteString(string s)
        {
            foreach (char c in s)
            {
                WriteChar(c);
            }
        }

        public void CloseStream()
        {
            // todo: end of file problem. append extra 0s from curr byte?
            byte[] b = new byte[1];
            _currentByte.CopyTo(b, 0);
            _stream.WriteByte(b[0]);
            _stream.Close();
        }
    }
}
