﻿using LibMetadata.AudioMetadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMetadata.IO
{
    public class FlacFileInfoReader : IAudioFileInfoReader
    {
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

        //public enum BlockType
        //{
        //    STREAMINFO = 0,
        //    PADDING = 1,
        //    APPLICATION = 2,
        //    SEEKTABLE = 3,
        //    VORBIS_COMMENT = 4,
        //    CUESHEET = 5,
        //    PICTURE = 6,
        //    INVALID = 127
        //}
        #region Constants
        
        #endregion

        //#region Flac Blocks
        //// streaminfo block
        //// -------------------------------------
        //public int MinimumBlockSize { get; private set; }
        //public int MaximumBlockSize { get; private set; }
        //public int MinimumFrameSize { get; private set; }
        //public int MaximumFrameSize { get; private set; }
        //public int SampleRateHz { get; private set; }
        //public int NumberOfChannels { get; private set; }
        //public int BitsPerSample { get; private set; }
        //public long SamplesPerStream { get; private set; }
        //public byte[] Signature { get; private set; }
        //// -------------------------------------

        //// padding block
        //// -------------------------------------
        //public int PaddingLengthInBytes { get; private set; }
        //// -------------------------------------


        //// flac comments block
        //// -------------------------------------
        //public string VendorComment { get; private set; }
        //public List<string> Comments { get; private set; }
        //// -------------------------------------
        //#endregion

        #region Private Variables
        private int _totalMetadataSpace = 0;
        private string _vendorString;
        private long _flacCommentPosition;
        private BitReader _bitReader = null;
        private string _filePath;
        #endregion

        public FlacFileInfoReader(string filePath)
        {
            _filePath = filePath;
            _bitReader = new BitReader(_filePath);
        }

        public IAudioFileInfo ReadFileInfo()
        {
            if (_bitReader == null)
            {
                if (string.IsNullOrWhiteSpace(FilePath))
                {
                    throw new Exception("FilePath property is null. You must set the FilePath of the flac file first.");
                }
                else
                {
                    _bitReader = new BitReader(FilePath);
                }
            }
            FlacFileInfo ret = new FlacFileInfo();
            ret.FilePath = _filePath;

            ReadStreamMarker();
            ret.MetadataLengthInBytes += 4;

            FlacMetadataBlockHeader header;
            do
            {
                header = ReadMetadataBlockHeader();
                ret.MetadataLengthInBytes += header.BlockLengthInBytes;
                switch (header.BlockType)
                {
                    case FlacMetadataBlockType.STREAMINFO:
                        FlacStreamInfoBlock streaminfo = new FlacStreamInfoBlock();
                        streaminfo.Header = header;
                        streaminfo.ReadBlockData(_bitReader);
                        ret.StreamInfoBlock = streaminfo;
                        break;
                    case FlacMetadataBlockType.SEEKTABLE:
                        FlacSeektableBlock seektable = new FlacSeektableBlock();
                        seektable.Header = header;
                        seektable.ReadBlockData(_bitReader);
                        ret.SeekTableBlock = seektable;
                        break;
                    case FlacMetadataBlockType.VORBIS_COMMENT:
                        FlacVorbisCommentBlock vorbisComments = new FlacVorbisCommentBlock();
                        vorbisComments.Header = header;
                        vorbisComments.ReadBlockData(_bitReader);
                        ret.VorbisCommentBlock = vorbisComments;
                        //ParseVorbisComments(ret);
                        break;
                    case FlacMetadataBlockType.PADDING:
                        FlacPaddingBlock padding = new FlacPaddingBlock();
                        padding.Header = header;
                        padding.ReadBlockData(_bitReader);
                        ret.PaddingBlock = padding;
                        break;
                    default:
                        FlacMetadataBlock block = new FlacMetadataBlock();
                        block.Header = header;
                        block.ReadBlockData(_bitReader);
                        ret.OtherBlocks.Add(block);
                        break;
                }
            }
            while (!header.IsLastMetadataBlock);

            return ret;
        }

        #region Private Methods
        private void CheckBitReader()
        {
            if (_bitReader == null)
            {
                throw new Exception("BitReader is null.");
            }
        }
        private void ReadStreamMarker()
        {
            CheckBitReader();
            string mark = _bitReader.ReadString(4, 8);
            if (mark != "fLaC")
            {
                throw new Exception("Stream marker not found at head of stream. Make sure this is valid flac file and the position of the stream is at the beginning");
            }
        }
        private FlacMetadataBlockHeader ReadMetadataBlockHeader()
        {
            CheckBitReader();
            FlacMetadataBlockHeader ret = new FlacMetadataBlockHeader();
            ret.IsLastMetadataBlock = _bitReader.ReadBit();
            ret.BlockType = (FlacMetadataBlockType)_bitReader.ReadInt16(7);
            ret.BlockLengthInBytes = _bitReader.ReadInt32(24);
            return ret;
        }
        #endregion
    }
}
