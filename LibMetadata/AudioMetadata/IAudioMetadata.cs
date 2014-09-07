using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlacDotnetLib.AudioMetadata
{
    public interface IAudioMetadata
    {
        string Title { get; set; }
        string Artist { get; set; }
        string Album { get; set; }
        string Year { get; set; }
        string Comment { get; set; }
        int Track { get; set; }
    }
}
