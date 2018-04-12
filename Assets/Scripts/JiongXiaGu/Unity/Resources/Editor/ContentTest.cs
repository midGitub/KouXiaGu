//#define Zip

using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;

namespace JiongXiaGu.Unity.Resources
{

    [TestFixture]
    public class ContentTest : NUnit
    {
        public ContentTest() : base(nameof(ContentTest))
        {
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
            string directory = Path.Combine(RootDirectory, "Directory");

            using (DirectoryContent content = DirectoryContent.Create(directory))
            {
                ContentReadWriteTest(content);
            }
        }

        [Test]
        public void TestZipContent()
        {
            string file = Path.Combine(RootDirectory, "Zip.zip");

            using (SharpZipLibContent content = SharpZipLibContent.CreateNew(file))
            {
                ContentReadWriteTest(content);
            }
        }


        private readonly XmlSerializer<Description> xmlSerializer = new XmlSerializer<Description>();

        private readonly string description1Path = "d1.xml";
        private readonly string description1PathA = "d1.Xml";
        private readonly string description1PathB = "D1.xml";
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

        private readonly string description4Path = @"D3/D4/d4.xml";
        private readonly Description description4 = new Description()
        {
            ID = "004",
        };

        /// <summary>
        /// 内容读写测试;
        /// </summary>
        private void ContentReadWriteTest(Content content)
        {
            IContentEntry entry1;
            using (var dis = content.BeginUpdateAuto())
            {
                using (Stream stream1 = content.GetOutputStream(description1Path, out entry1))
                {
                    xmlSerializer.Serialize(stream1, description1);
                }
                Assert.IsNotNull(entry1);

                using (Stream stream2 = content.GetOutputStream(description2Path),
                    stream3 = content.GetOutputStream(description3Path),
                    stream4 = content.GetOutputStream(description4Path))
                {
                    xmlSerializer.Serialize(stream2, description2);
                    xmlSerializer.Serialize(stream3, description3);
                    xmlSerializer.Serialize(stream4, description4);
                }

                using (var stream3 = content.GetOutputStream(description3Path))
                {
                    xmlSerializer.Serialize(stream3, description3);
                }
            }

            Assert.AreEqual(4, content.EnumerateFiles().Count());
            Assert.AreEqual(2, content.EnumerateEntries("d*", SearchOption.TopDirectoryOnly).Count());
            Assert.AreEqual(1, content.EnumerateEntries("D3", "d*", SearchOption.TopDirectoryOnly).Count());
            Assert.AreEqual(1, content.EnumerateEntries(@"D3\D4", "*", SearchOption.TopDirectoryOnly).Count());
            Assert.AreEqual(0, content.EnumerateEntries(@"D4", "*", SearchOption.TopDirectoryOnly).Count());

            Assert.True(content.Contains(description1Path));
            Assert.True(content.Contains(description1PathA));
            Assert.True(content.Contains(description1PathB));

            using (Stream stream1 = content.GetInputStream(entry1),
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

            using (var  stream3_1 = content.GetInputStream(description3Path))
            {
                var d3_1 = xmlSerializer.Deserialize(stream3_1);
                AreEqual(d3_1, description3);
            }

            using (var dis = content.BeginUpdateAuto())
            {
                Assert.IsTrue(content.Remove(description1Path));
                Assert.IsFalse(content.Remove("123.ddd"));
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
