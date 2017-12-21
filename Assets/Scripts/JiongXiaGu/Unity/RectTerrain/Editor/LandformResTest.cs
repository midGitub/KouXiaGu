using JiongXiaGu.Unity.Resources;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace JiongXiaGu.Unity.RectTerrain
{

    /// <summary>
    /// 地形资源读写单元测试;
    /// </summary>
    [TestFixture]
    public class LandformResTest
    {
        private readonly LandformDescriptionSerializer landformDescrSerializer = new LandformDescriptionSerializer();

        private const string assetBundleName = "terrain";

        private DescriptionCollection<LandformDescription> DefaultLandformDescription = new DescriptionCollection<LandformDescription>()
        {
            Version = "0.01",
            Message = "Test",
            Descriptions = new List<LandformDescription>()
            {
                new LandformDescription()
                {
                    ID = "1",
                    Name = "Unknow1",
                    DiffuseTex = new AssetInfo(assetBundleName, "DiffuseTex1"),
                    DiffuseBlendTex = new AssetInfo(assetBundleName, "DiffuseBlendTex1"),
                    HeightTex = new AssetInfo(assetBundleName, "HeightTex1"),
                    HeightBlendTex = new AssetInfo(assetBundleName, "HeightBlendTex1"),
                },
                new LandformDescription()
                {
                    ID = "2",
                    Name = "Unknow2",
                    DiffuseTex = new AssetInfo(new AssetPath("000", "DiffuseTex2")),
                    DiffuseBlendTex = new AssetInfo("DiffuseBlendTex2"),
                    HeightTex = new AssetInfo("HeightTex2"),
                    HeightBlendTex = new AssetInfo("HeightBlendTex2"),
                }
            },
        };

        [Test]
        public void ReadWriteTest()
        {
            using (var content = GetContent())
            {
                landformDescrSerializer.Serialize(content, DefaultLandformDescription);
                var description = landformDescrSerializer.Deserialize(content);
                AreEqual(description, DefaultLandformDescription);
            }
        }

        private Content GetContent()
        {
            string directory = Path.Combine(NUnit.TempDirectory, nameof(LandformResTest));
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }
            Directory.CreateDirectory(directory);
            return new ContentDirectory(directory);
        }

        private void AreEqual(DescriptionCollection<LandformDescription> v1, DescriptionCollection<LandformDescription> v2)
        {
            Assert.AreEqual(v1.Version, v2.Version);
            Assert.AreEqual(v1.Descriptions[0].DiffuseTex, v2.Descriptions[0].DiffuseTex);
            Assert.AreEqual(v1.Descriptions[0].HeightBlendTex, v2.Descriptions[0].HeightBlendTex);
        }
    }
}
