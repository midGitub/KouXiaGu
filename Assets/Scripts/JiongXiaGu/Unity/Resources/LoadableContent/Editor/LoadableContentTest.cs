using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    [TestFixture]
    public class LoadableContentTest
    {

        private readonly LoadableContentFactory factory = new LoadableContentFactory();

        private readonly LoadableContentDescription description = new LoadableContentDescription()
        {
            ID = "001",
            Name = "Test",
            Tags = LoadableContentDescription.JoinTags("map", "terrain"),
            Author = "One",
            Version = "1.22",
            Message = "..."
        };


        [Test]
        public void TestDirectory()
        {
            string directory = Path.Combine(NUnit.TempDirectory, nameof(LoadableContentTest), "Directory0");
            TryDeleteDirectory(directory);
            var v1 = factory.CreateNew(directory, description);
            v1.Unload();
            var v2 = factory.Read(directory);
            Assert.AreEqual(v1.Description, v2.Description);
        }

        private bool TryDeleteDirectory(string directory)
        {
            try
            {
                Directory.Delete(directory, true);
                return true;
            }
            catch (DirectoryNotFoundException)
            {
                return false;
            }
        }


        [Test]
        public void TestZip()
        {
            string file = Path.Combine(NUnit.TempDirectory, nameof(LoadableContentTest), "Zip0.zip");
            TryDeleteFile(file);
            var v1 = factory.CreateNewZip(file, description);
            v1.Unload();
            var v2 = factory.ReadZip(file);
            Assert.AreEqual(v1.Description, v2.Description);
        }

        private bool TryDeleteFile(string file)
        {
            try
            {
                File.Delete(file);
                return true;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }
    }
}
