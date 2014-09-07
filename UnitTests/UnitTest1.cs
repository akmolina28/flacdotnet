using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void FlacReaderTest()
        {
            LibMetadata.IO.FlacFileInfoReader reader = new LibMetadata.IO.FlacFileInfoReader(@"C:\ph1996-07-22.29075.flac16\ph1996-07-22t01.flac");
            LibMetadata.IO.FlacFileInfo fileInfo = (LibMetadata.IO.FlacFileInfo)reader.ReadMetadata();
        }
    }
}
