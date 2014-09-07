using LibMetadata.AudioMetadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMetadata.IO
{
    public interface IAudioFileInfoReader
    {

        string FilePath { get; set; }
        IAudioFileInfo ReadMetadata();
    }
}
