using NUnit.Framework;
using System.IO;
using System.Linq;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    [TestFixture]
    public class LoadableContentTest
    {
        private readonly LoadableContentFactory factory = new LoadableContentFactory();

        private readonly XmlSerializer<LoadableContentDescription> xmlSerializer = new XmlSerializer<LoadableContentDescription>();

        private readonly LoadableContentDescription description0 = new LoadableContentDescription()
        {
            ID = "000",
            Name = "Test",
            Tags = LoadableContentDescription.JoinTags("map", "terrain"),
            Author = "One",
            Version = "1.22",
            Message = "...",
            AssetBundles = new AssetBundleDescription[] 
            {
                new AssetBundleDescription("terrain", @"AssetBundles\terrain")
            },
        };

        [Test]
        public void TestDirectory()
        {
            LoadableContentDescription descr;
            string directory = Path.Combine(NUnit.TempDirectory, nameof(LoadableContentTest), "Directory0");
            TryDeleteDirectory(directory);

            using (var v1 = factory.CreateNew(directory, description0))
            {
                descr = v1.OriginalDescription;
                ContentReadWriteTest(v1);
            }

            using (var v2 = factory.Read(directory))
            {
                Assert.AreEqual(descr, v2.OriginalDescription);
            }
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
            LoadableContentDescription descr;
            string file = Path.Combine(NUnit.TempDirectory, nameof(LoadableContentTest), "Zip0.zip");
            TryDeleteFile(file);

            using (var v1 = factory.CreateNewZip(file, description0))
            {
                descr = v1.OriginalDescription;
                ContentReadWriteTest(v1);
            }

            using (var v2 = factory.ReadZip(file))
            {
                Assert.AreEqual(descr, v2.OriginalDescription);
            }
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



        private readonly string description1Path = "d1.xml";
        private readonly LoadableContentDescription description1 = new LoadableContentDescription()
        {
            ID = "001",
            Tags = LoadableContentDescription.JoinTags("map", "terrain"),
        };

        private readonly string description2Path = "d2.xml";
        private readonly LoadableContentDescription description2 = new LoadableContentDescription()
        {
            ID = "002",
            Name = "Test",
        };

        private readonly string description3Path = "d3.xml";
        private readonly LoadableContentDescription description3 = new LoadableContentDescription()
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
                using (Stream stream1 = content.GetOutStream(description1Path),
                    stream2 = content.GetOutStream(description2Path),
                    stream3 = content.GetOutStream(description3Path))
                {
                    xmlSerializer.Serialize(stream1, description1);
                    xmlSerializer.Serialize(stream2, description2);
                    xmlSerializer.Serialize(stream3, description3);
                }
            }

            Assert.AreEqual(content.EnumerateFiles().Count(), 4);

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

        private void AreEqual(LoadableContentDescription v1, LoadableContentDescription v2)
        {
            Assert.AreEqual(v1.ID, v2.ID);
            Assert.AreEqual(v1.Name, v2.Name);
            Assert.AreEqual(v1.Tags, v2.Tags);
        }

        [Test]
        public void AssetBundleLoadTest()
        {
            using (LoadableContent loadableContent = LoadableResource.GetCore(factory))
            {
                var assetBundle = loadableContent.GetOrLoadAssetBundle("terrain");
                Assert.NotNull(assetBundle);
                assetBundle.LoadAsset<Texture2D>("HeightMap_85");
            }
        }
    }
}
