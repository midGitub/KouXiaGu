using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SharpCompress.Archives.Zip;
using SharpCompress.Archives;

namespace JiongXiaGu.Unity.Resources
{

    [TestFixture]
    public class SharpCompressTest
    {

        [Test]
        public void Test()
        {
            string file0 = "1.txt";
            string file1 = "2.txt";

            using (var memoryStream = new MemoryStream())
            {
                using (ZipArchive zipArchive = ZipArchive.Create())
                {
                    zipArchive.AddEntry(file0, new MemoryStream(Encoding.UTF8.GetBytes("test1")));
                    zipArchive.SaveTo(memoryStream);
                }

                using (ZipArchive zipArchive = ZipArchive.Open(memoryStream))
                {
                    var zipEntry = GetEntry(zipArchive, file0);
                    Assert.NotNull(zipEntry);

                    zipArchive.AddEntry(file1, new MemoryStream(Encoding.UTF8.GetBytes("test2")));
                    zipEntry = GetEntry(zipArchive, file1);
                    Assert.NotNull(zipEntry);
                }

                using (ZipArchive zipArchive = ZipArchive.Open(memoryStream))
                {
                    var zipEntry = GetEntry(zipArchive, file0);
                    Assert.NotNull(zipEntry);

                    zipEntry = GetEntry(zipArchive, file1);
                    Assert.IsNull(zipEntry);
                }
            }
        }

        private ZipArchiveEntry GetEntry(ZipArchive zipArchive, string name)
        {
            var zipEntry = zipArchive.Entries.FirstOrDefault(entry => string.Equals(entry.Key, name, StringComparison.OrdinalIgnoreCase));
            return zipEntry;
        }
    }
}
