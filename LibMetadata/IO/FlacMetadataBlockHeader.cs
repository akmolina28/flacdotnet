using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMetadata.IO
{
    public class FlacMetadataBlockHeader
    {
        public bool IsLastMetadataBlock { get; set; }
        public FlacMetadataBlockType BlockType { get; set; }
        public int BlockLengthInBytes { get; set; }
    }
}
