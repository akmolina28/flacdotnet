using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibMetadata.IO;
using System.IO;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void FlacReaderTest()
        {
            LibMetadata.IO.FlacFileInfoReader reader = new LibMetadata.IO.FlacFileInfoReader(@"C:\ph1996-07-22.29075.flac16\ph1996-07-22t01.flac");
            //LibMetadata.IO.FlacFileInfoReader reader = new LibMetadata.IO.FlacFileInfoReader(@"E:\Music\test\03 The Conch.flac");
            LibMetadata.IO.FlacFileInfo fileInfo = (LibMetadata.IO.FlacFileInfo)reader.ReadFileInfo();
        }

        [TestMethod]
        public void TapeInfoTests()
        {
            DateTime actual = TapeInfoHelper.GetDateFromFileName("ph1996-07-22.29075.flac16");
            DateTime expected = new DateTime(1996, 7, 22);
            Assert.AreEqual(actual, expected);

            //using (Stream s = File.OpenRead(@"C:\ph1996-07-22.29075.flac16\ph1996-07-22.txt"))
            //{
            //    using (StreamReader sr = new StreamReader(s))
            //    {
            //        string text = sr.ReadToEnd()
            //        string[] titles = TapeInfoHelper.GetTitles(System.IO., "Sample in a Jar");
            //    }
            //}
            string text = @"Phish
07-22-96 
Tanzbrunnen 
Cologne, Germany

Unknown AUD > ? > CD > EAC > Mastering > FLAC16
Thanks to Volker Skrzeba for the source discs.
Mastering, tracking, FLAC16 by Marmar- imthemarmar@gmail.com

Running Time: 00:54:45.29
 
01. Sample in a Jar*
02. Poor Heart
03. Cavern
04. Maze
05. Bouncing Around the Room
06. Stash^
07. A Day in the Life
08. You Enjoy Myself**

Phish opened for Santana. 

* the song fades in...not much of the beginning chords are lost. There are also some adjustments/rumble as the taper gets situated.
** there is a cut in the middle at 00:47:10.23 > 00:47:11.67 ...durring the WUDMTF part...this could not be repaired and was left as-is
^ this is where the distortion/dropouts get really bad....I did my best to edit them, but some are still audible

*NOTE*
This is not the best sounding show, but it is all we have and is being released for those who must have every show......There were numerous areas of dropouts/distortion in the entire show. I tried my best to remove all the really bad ones, but a few (minor) areas still remain.

**Mastering**
Reduced volume by 1.2db > RBass 65Hz > BBE 0/4/0 > Q1 EQ 31Hz Hi-pass filter > L3 > FLAC16";

            string[] titles = TapeInfoHelper.GetTitles(text, "Sample in a Jar");
        }
    }
}
