using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMetadata.IO
{
    public class FlacSeektableBlock : IFlacMetadataBlock
    {
        public FlacMetadataBlockHeader Header { get; set; }

        public FlacSeekTable SeekTable { get; set; }

        public void ReadBlockData(BitReader reader)
        {
            int n = Header.BlockLengthInBytes / 18;
            SeekTable = new FlacSeekTable();
            SeekTable.SeekPoints = new byte[n][];
            for (int i = 0; i < n; i++)
            {
                SeekTable.SeekPoints[i] = new byte[18];
                reader.ReadBytes(SeekTable.SeekPoints[i], 18 * 8);
            }
        }
    }
}
