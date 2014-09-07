﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMetadata.IO
{
    public class FlacMetadataBlock : IFlacMetadataBlock
    {
        public FlacMetadataBlockHeader Header { get; set; }

        public byte[] BlockData { get; set; }

        public void ReadBlockData(BitReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
