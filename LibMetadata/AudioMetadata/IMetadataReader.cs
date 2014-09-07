using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlacDotnetLib.AudioMetadata
{
    public interface IMetadataReader
    {
        string FileName { get; set; }
        IAudioMetadata ReadMetadata();
    }
}
