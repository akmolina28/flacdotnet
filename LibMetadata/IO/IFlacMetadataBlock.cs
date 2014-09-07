using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMetadata.IO
{
    public interface IFlacMetadataBlock
    {
        FlacMetadataBlockHeader Header { get; set; }

        void ReadBlockData(BitReader reader);
    }
}
