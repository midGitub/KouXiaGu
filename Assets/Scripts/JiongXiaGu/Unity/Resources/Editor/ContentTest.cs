using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            }
            Directory.CreateDirectory(rootDirectory);
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
        /// 内容读写测试;
        /// </summary>
        private void ContentReadWriteTest(Content content)
        {
            using (var dis = content.BeginUpdate())
            {
                using (Stream stream1 = content.GetOutputStream(description1Path))
                {
                    xmlSerializer.Serialize(stream1, description1);
                }

                using (Stream stream2 = content.GetOutputStream(description2Path),
                    stream3 = content.GetOutputStream(description3Path))
                {
                    xmlSerializer.Serialize(stream2, description2);
                    xmlSerializer.Serialize(stream3, description3);

                    try
                    {
                        using (Stream stream4 = content.GetOutputStream(description2Path))
                        {
                            xmlSerializer.Serialize(stream4, description3);
                        }
                        Assert.Fail("应该抛出异常;");
                    }
                    catch (IOException)
                    {
                    }
                }
            }

            Assert.AreEqual(content.EnumerateFiles().Count(), 3);
            Assert.True(content.Contains(description1Path));

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

            using (var dis = content.BeginUpdate())
            {
                content.Remove(description1Path);
                content.Remove(description2Path);
                content.Remove(description3Path);
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
