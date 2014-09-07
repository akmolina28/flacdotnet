using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMetadata.IO
{
    /// <summary>
    /// This block has information about the whole stream, like sample rate, number of channels, total number of samples, etc. It must be present as the first metadata block in the stream. Other metadata blocks may follow, and ones that the decoder doesn't understand, it will skip.
    /// </summary>
    public class FlacStreamInfoBlock : IFlacMetadataBlock
    {
        public FlacMetadataBlockHeader Header { get; set; }

        public FlacStreamInfo StreamInfo { get; set; }

        public void ReadBlockData(BitReader reader)
        {
            StreamInfo = new FlacStreamInfo();

            StreamInfo.MinimumBlockSize = reader.ReadInt16(16);
            StreamInfo.MaximumBlockSize = reader.ReadInt16(16);

            StreamInfo.MinimumFrameSize = reader.ReadInt32(24);
            StreamInfo.MinimumFrameSize = reader.ReadInt32(24);

            StreamInfo.SampleRateHz = reader.ReadInt32(20);
            StreamInfo.NumberOfChannels = reader.ReadInt16(3);
            StreamInfo.BitsPerSample = reader.ReadInt16(5) + 1;
            StreamInfo.TotalSamples = reader.ReadInt64(36);

            StreamInfo.Signature = new byte[16];
            reader.ReadBytes(StreamInfo.Signature, 128);
        }
    }
}
