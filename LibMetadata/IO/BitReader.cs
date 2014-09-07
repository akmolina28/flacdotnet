using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMetadata.IO
{
    public class BitReader : IDisposable
    {
        private const int SIZE_OF_BYTE = 8;

        private Stream _stream;
        private byte _currentByte;
        private int _currentBit = 0;

        public bool IsLittleEndian { get; set; }
        public bool EndOfStream { get; set; }
        public int SizeOfByte
        {
            get
            {
                return SIZE_OF_BYTE;
            }
        }

        #region Constructor
        protected BitReader()
        {
            IsLittleEndian = false;
            EndOfStream = false;
        }

        public BitReader(Stream stream)
            : this()
        {
            //_stream = File.Open(fileName, FileMode.Open, FileAccess.Read);
            _stream = stream;
            _currentByte = (byte)_stream.ReadByte();
        }

        public BitReader(string filePath) : this(System.IO.File.OpenRead(filePath)) { }

        #endregion
        /// <summary>
        /// Set the position of the reader.
        /// </summary>
        /// <param name="positionInBits">0-based position in bits.</param>
        public void GoToPosition(long positionInBits)
        {
            long bytes = positionInBits / 8;
            _stream.Position = bytes;

            _currentByte = (byte)_stream.ReadByte();
            _currentBit = Convert.ToInt32(positionInBits - (bytes * 8));
        }
        /// <summary>
        /// Advance the reader position.
        /// </summary>
        /// <param name="bits">Number of bits to advance.</param>
        public void AdvancePosition(int bits)
        {
            GoToPosition(GetPosition() + bits);
        }

        public long GetPosition()
        {
            return ((_stream.Position - 1) * 8) + _currentBit;
        }

        #region Read Methods
        /// <summary>
        /// Reads the next bit in the stream and advances the reader by one bit.
        /// </summary>
        /// <returns></returns>
        public bool ReadBit()
        {
            if (_currentBit == 8)
            {
                ReadNextByte();
            }

            if (EndOfStream)
            {
                throw new InvalidOperationException("The end of the stream has been reached -- there are no more bits to read.");
            }

            bool value = (_currentByte & (1 << (7 - _currentBit))) > 0;
            _currentBit++;

            return value;
        }

        public BitArray ReadBits(int n)
        {
            BitArray bar = new BitArray(n);

            for (int i = n - 1; i >= 0; i--)
            {
                bool? set = ReadBit();
                if (set == null)
                {
                    return null;
                }
                else if (set == true)
                {
                    bar.Set(i, true);
                }
                else
                {
                    bar.Set(i, false);
                }
            }

            return bar;
        }
        #endregion
        private void ReadNextByte()
        {
            var r = _stream.ReadByte();
            if (r == -1)
            {
                EndOfStream = true;
            }
            _currentBit = 0;
            _currentByte = (byte)r;
        }



        public void CloseStream()
        {
            _stream.Close();
        }

        public string ReadString(int strLength, int bitsPerChar)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < strLength; i++)
            {
                sb.Append(ReadChar(bitsPerChar));
            }
            return sb.ToString();
        }

        public void ReadBytes(byte[] buffer, int numBits)
        {
            int numBytes = NumBytes(numBits);
            // todo: check if stream length will be exceeded (maybe should do this in readbit)
            if (buffer.Length < numBytes)
            {
                throw new Exception("The buffer provided is to small for the data requested");
            }

            byte[] b = new byte[numBytes];
            BitArray br = ReadBits(numBits);
            br.CopyTo(b, 0);
            if (IsLittleEndian)
            {
                b = ReverseBytes(b);
            }
            Array.Copy(b, buffer, numBytes);
        }

        private int NumBytes(int numBits)
        {
            return (numBits + SIZE_OF_BYTE - 1) / SIZE_OF_BYTE; // divide and round up. neat!
        }

        public char ReadChar(int numBits)
        {
            int numBytes = sizeof(char);
            int maxBits = numBytes * SIZE_OF_BYTE;
            if (numBits > maxBits || numBits < 1)
            {
                throw new Exception("Char must have length between 1 and " + maxBits);
            }
            byte[] buffer = new byte[numBytes];
            ReadBytes(buffer, numBits);
            return BitConverter.ToChar(buffer, 0);
        }

        public Int16 ReadInt16(int numBits)
        {
            if (numBits > 16 || numBits < 1)
            {
                throw new Exception("Int16 must have length between 1 and 16");
            }
            int numBytes = 2;
            byte[] buffer = new byte[numBytes];
            ReadBytes(buffer, numBits);
            return BitConverter.ToInt16(buffer, 0);
        }

        public Int32 ReadInt32(int numBits)
        {
            int numBytes = sizeof(Int32);
            if (numBits > numBytes * SizeOfByte || numBits < 1)
            {
                throw new Exception("Int16 must have length between 1 and 32 bits.");
            }
            byte[] buffer = new byte[numBytes];
            ReadBytes(buffer, numBits);
            return BitConverter.ToInt32(buffer, 0);
        }

        public Int64 ReadInt64(int numBits)
        {
            int numBytes = sizeof(Int64);
            if (numBits > numBytes * SizeOfByte || numBits < 1)
            {
                throw new Exception("Int16 must have length between 1 and 64 bits.");
            }
            byte[] buffer = new byte[numBytes];
            ReadBytes(buffer, numBits);
            return BitConverter.ToInt64(buffer, 0);
        }

        //public char ReadChar(int numBits)
        //{
        //    int numBytes = sizeof(char);
        //    if (numBits > numBytes * SizeOfByte || numBits < 1)
        //    {
        //        throw new Exception("char must have length between 1 and 8 bits.");
        //    }
        //    byte[] buffer = new byte[numBytes];
        //    ReadBytes(buffer, numBits);
        //    return BitConverter.ToChar(buffer, 0);
        //}

        //public byte[] ReadBytes(int n)
        //{
        //    int numBits = n * 8;
        //    BitArray bits = ReadBits(numBits);
        //    byte[] ret = new byte[n];
        //    bits.Copy
        //    bits.CopyTo(ret, 0);
        //    if (IsLittleEndian)
        //    {
        //        ret = ReverseBytes(ret);
        //    }
        //    return ret;
        //}

        //private void CopyBits(this BitArray[] bits, int srcOffset, out byte[] copyTo, int dstOffset = 0, int length)
        //{
        //    for (int i = length - 1; i < length; i++)
        //    {
        //        int byteIndex = 
        //    }
        //}

        //public Int32 ReadInt32()
        //{
        //    return ReadInt32(32);
        //}

        //public Int32 ReadInt32(int numBits)
        //{
        //    if (numBits > 32 || numBits < 1)
        //    {
        //        throw new Exception();
        //    }
        //    Int32 ret;
        //    if (IsLittleEndian)
        //    {
        //        int arraySize = numBits / 8;
        //        if (numBits - (arraySize * 8) > 0)
        //        {
        //            arraySize++;
        //        }
        //        byte[] leBytes = new byte[4];
        //        BitArray leBits = ReadBits(numBits);
        //        BitArray allBits = new BitArray(4 * 8);
        //        int j = numBits - 1;
        //        for (int i = 31; i >= 0; i--)
        //        {
        //            if (j > 0)
        //            {
        //                bool temp = leBits[j];
        //                allBits[i] = temp;
        //                j--;
        //            }
        //            else
        //            {
        //                allBits[i] = false;
        //            }
        //        }
        //        allBits.CopyTo(leBytes, 0);
        //        leBytes = ReverseBytes(leBytes);
        //        ret = BitConverter.ToInt32(leBytes, 0);
        //    }
        //    else
        //    {
        //        BitArray beBits = ReadBits(numBits);
        //        byte[] beBytes = new byte[4];
        //        beBits.CopyTo(beBytes, 0);
        //        ret = BitConverter.ToInt32(beBytes, 0);
        //    }
        //    return ret;
        //}

        //public Int16 ReadInt16(int numBits)
        //{
        //    if (numBits > 16 || numBits < 1)
        //    {
        //        throw new Exception();
        //    }
        //    BitArray bits = ReadBits(numBits);
        //    byte[] bytes = new byte[2];
        //    bits.CopyTo(bytes, 0);
        //    if (IsLittleEndian)
        //    {
        //        bytes = ReverseBytes(bytes);
        //    }
        //    Int16 ret = BitConverter.ToInt16(bytes, 0);
        //    return ret;
        //}

        //public Int64 ReadInt64()
        //{
        //    return ReadInt64(64);
        //}

        //public Int64 ReadInt64(int numBits)
        //{
        //    if (numBits > 64 || numBits < 1)
        //    {
        //        throw new Exception();
        //    }
        //    BitArray bits = ReadBits(numBits);
        //    byte[] bytes = new byte[8];
        //    bits.CopyTo(bytes, 0);
        //    if (IsLittleEndian)
        //    {
        //        bytes = ReverseBytes(bytes);
        //    }
        //    Int64 ret = BitConverter.ToInt64(bytes, 0);
        //    return ret;
        //}

        //public char ReadChar()
        //{
        //    BitArray bits = ReadBits(8);
        //    byte[] bytes = new byte[2];
        //    bits.CopyTo(bytes, 0);
        //    char c = BitConverter.ToChar(bytes, 0);
        //    return c;
        //}

        //public byte[] BitsToBytes(BitArray bar)
        //{
        //    int length = bar.Length / 8;
        //    if (bar.Length % 8 != 0)
        //    {
        //        length++;
        //    }
        //    byte[] ret = new byte[length];

        //    bar.CopyTo(ret, 0);

        //    return ret;
        //}

        private byte[] ReverseBytes(byte[] bytes)
        {
            byte[] ret = new byte[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
            {
                ret[i] = bytes[bytes.Length - 1 - i];
            }
            return ret;
        }

        public void Dispose()
        {
            _stream.Dispose();
        }
    }
}
