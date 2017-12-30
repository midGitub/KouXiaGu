using NUnit.Framework;
using System.IO;
using System.Linq;

namespace JiongXiaGu.Unity.Resources
{

    [TestFixture]
    public class ContentTest
    {
        private readonly string rootDirectory;

        public ContentTest()
        {
            rootDirectory = Path.Combine(NUnit.TempDirectory, nameof(ContentTest));
            if (Directory.Exists(rootDirectory))
            {
                Directory.Delete(rootDirectory, true);
                Directory.CreateDirectory(rootDirectory);
            }
        }

        [Test]
        public void TestMemoryContent()
        {
            using (MemoryContent content = new MemoryContent())
            {
                ContentReadWriteTest(content);
            }
        }

        [Test]
        public void TestDirectoryContent()
        {
            string directory = Path.Combine(rootDirectory, "Directory");

            using (DirectoryContent content = new DirectoryContent(directory))
            {
                ContentReadWriteTest(content);
            }
        }

        [Test]
        public void TestZipContent()
        {
            string file = Path.Combine(rootDirectory, "Zip.zip");

            using (ZipContent content = ZipContent.CreateNew(file))
            {
                ContentReadWriteTest(content);
            }
        }

        private readonly XmlSerializer<Description> xmlSerializer = new XmlSerializer<Description>();

        private readonly string description1Path = "d1.xml";
        private readonly Description description1 = new Description()
        {
            ID = "001",
            Tags = "map, terrain",
        };

        private readonly string description2Path = "d2.xml";
        private readonly Description description2 = new Description()
        {
            ID = "002",
            Name = "Test",
        };

        private readonly string description3Path = @"D3\d3.xml";
        private readonly Description description3 = new Description()
        {
            ID = "003",
        };

        /// <summary>
        /// 添加内容;
        /// </summary>
        private void ContentReadWriteTest(Content content)
        {
            using (var dis = content.BeginUpdate())
            {
                using (Stream stream1 = content.CreateOutputStream(description1Path),
                    stream2 = content.CreateOutputStream(description2Path),
                    stream3 = content.CreateOutputStream(description3Path))
                {
                    xmlSerializer.Serialize(stream1, description1);
                    xmlSerializer.Serialize(stream2, description2);
                    xmlSerializer.Serialize(stream3, description3);
                }
            }

            Assert.AreEqual(content.EnumerateFiles().Count(), 3);

            using (Stream stream1 = content.GetInputStream(description1Path),
                stream2 = content.GetInputStream(description2Path),
                stream3 = content.GetInputStream(description3Path))
            {
                var d1 = xmlSerializer.Deserialize(stream1);
                AreEqual(d1, description1);

                var d2 = xmlSerializer.Deserialize(stream2);
                AreEqual(d2, description2);

                var d3 = xmlSerializer.Deserialize(stream3);
                AreEqual(d3, description3);
            }
        }

        private void AreEqual(Description v1, Description v2)
        {
            Assert.AreEqual(v1.ID, v2.ID);
            Assert.AreEqual(v1.Name, v2.Name);
            Assert.AreEqual(v1.Tags, v2.Tags);
        }

        public struct Description
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string Tags { get; set; }
        }
    }
}
