using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMetadata.LiveTape
{
    public enum EncodingType
    {
        Mp3, Flac, Shorten
    }

    public class DownloadedShow
    {
        public string DownloadDirectory { get; set; }
        public Show Show { get; set; }
        public EncodingType Encoding { get; set; }
        public string ShowInfo { get; set; }
        public int NumTracks { get { return Show.SetList.Count; } }

        public DownloadedShow(string downloadDirectory)
        {
            Show = new Show();

            if (!Directory.Exists(downloadDirectory))
            {
                throw new DirectoryNotFoundException();
            }
            DownloadDirectory = downloadDirectory;
            SetEncodingType();
            ReadAudioFiles();
            ReadTextFiles();
        }

        private void ReadAudioFiles()
        {
            string extension;
            switch (Encoding)
            {
                case EncodingType.Flac:
                    extension = ".flac";
                    break;
                case EncodingType.Mp3:
                    extension = ".mp3";
                    break;
                case EncodingType.Shorten:
                    extension = ".shn";
                    break;
                default:
                    return;
            }

            foreach(string file in Directory.GetFiles(DownloadDirectory, "*" + extension))
            {
                Show.SetList.Add(new Track(Path.GetFileName(file)));
            }
        }

        private void ReadTextFiles()
        {
            foreach (string s in Directory.GetFiles(DownloadDirectory, "*.txt"))
            {
                using (StreamReader sr = new StreamReader(File.OpenRead(s)))
                {
                    ShowInfo += sr.ReadToEnd();
                }
            }
        }

        private void SetEncodingType()
        {
            foreach (string file in Directory.GetFiles(DownloadDirectory))
            {
                string extension = Path.GetExtension(file);
                switch (extension.TrimStart(new char[] { '.' }).ToUpper())
                {
                    case "FLAC":
                        Encoding = EncodingType.Flac;
                        return;
                    case "MP3":
                        Encoding = EncodingType.Mp3;
                        return;
                    case "SHN":
                        Encoding = EncodingType.Shorten;
                        return;
                    default:
                        break;
                }
            }
        }
    }
}
