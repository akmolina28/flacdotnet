using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMetadata.IO
{
    /// <summary>
    /// Padding Block is used to reserve extra metadata space. In the event that more metadata is added,
    /// this space can be allocated so that the entire file doesn't have to be rewritten.
    /// </summary>
    public class FlacPaddingBlock : IFlacMetadataBlock
    {
        public FlacMetadataBlockHeader Header { get; set; }

        public void ReadBlockData(BitReader reader)
        {
            reader.AdvancePosition(Header.BlockLengthInBytes * reader.SizeOfByte);
        }
    }
}
