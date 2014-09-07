using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMetadata.IO
{
    public class FlacStreamInfo
    {
        /// <summary>
        /// The minimum block size (in samples) used in the stream.
        /// </summary>
        public int MinimumBlockSize { get; set; }
        /// <summary>
        /// The maximum block size (in samples) used in the stream. (Minimum blocksize == maximum blocksize) implies a fixed-blocksize stream.
        /// </summary>
        public int MaximumBlockSize { get; set; }
        /// <summary>
        /// The minimum frame size (in bytes) used in the stream. May be 0 to imply the value is not known.
        /// </summary>
        public int MinimumFrameSize { get; set; }
        /// <summary>
        /// The maximum frame size (in bytes) used in the stream. May be 0 to imply the value is not known.
        /// </summary>
        public int MaximumFrameSize { get; set; }
        /// <summary>
        /// Sample rate in Hz. Though 20 bits are available, the maximum sample rate is limited by the structure of frame headers to 655350Hz. Also, a value of 0 is invalid.
        /// </summary>
        public int SampleRateHz { get; set; }
        /// <summary>
        /// (number of channels)-1. FLAC supports from 1 to 8 channels
        /// </summary>
        public int NumberOfChannels { get; set; }
        /// <summary>
        /// Bits per sample. FLAC supports from 4 to 32 bits per sample. Currently the reference encoder and decoders only support up to 24 bits per sample.
        /// </summary>
        public int BitsPerSample { get; set; }
        /// <summary>
        /// Total samples in stream. 'Samples' means inter-channel sample, i.e. one second of 44.1Khz audio will have 44100 samples regardless of the number of channels. A value of zero here means the number of total samples is unknown.
        /// </summary>
        public long TotalSamples { get; set; }
        /// <summary>
        /// MD5 signature of the unencoded audio data. This allows the decoder to determine if an error exists in the audio data even when the error does not result in an invalid bitstream.
        /// </summary>
        public byte[] Signature { get; set; }
    }
}
