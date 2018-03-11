using JiongXiaGu.Unity.Resources;
using JiongXiaGu.Unity.Resources.Binding;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace JiongXiaGu.Unity.RectTerrain
{

    /// <summary>
    /// 地形资源读写单元测试;
    /// </summary>
    [TestFixture]
    public class RectTerrainResourceDescriptionTest
    {
        private readonly string rootDirectory;

        public RectTerrainResourceDescriptionTest()
        {
            rootDirectory = Path.Combine(NUnit.TempDirectory, nameof(RectTerrainResourceDescriptionTest));
            if (Directory.Exists(rootDirectory))
            {
                Directory.Delete(rootDirectory, true);
                Directory.CreateDirectory(rootDirectory);
            }
        }

        private const string assetBundleName = "terrain";

        private RectTerrainResourceDescription DefaultDescription = new RectTerrainResourceDescription()
        {
            Landform = new DescriptionCollection<LandformDescription>()
            {
                Version = "0.01",
                Message = "Test",
                Descriptions = new List<LandformDescription>()
                {
                    new LandformDescription()
                    {
                        ID = "1",
                        LocName = "Unknow1",
                        DiffuseTex = new AssetInfo(assetBundleName, "DiffuseTex1"),
                        DiffuseBlendTex = new AssetInfo(assetBundleName, "DiffuseBlendTex1"),
                        HeightTex = new AssetInfo(assetBundleName, "HeightTex1"),
                        HeightBlendTex = new AssetInfo(assetBundleName, "HeightBlendTex1"),
                    },
                    new LandformDescription()
                    {
                        ID = "2",
                        LocName = "Unknow2",
                        DiffuseTex = new AssetInfo(assetBundleName, "DiffuseTex2"),
                        DiffuseBlendTex = new AssetInfo(assetBundleName, "DiffuseBlendTex2"),
                        HeightTex = new AssetInfo(assetBundleName, "HeightTex2"),
                        HeightBlendTex = new AssetInfo(assetBundleName, "HeightBlendTex2"),
                    }
                },
            },
        };
        
        private readonly BindingSerializer<RectTerrainResourceDescription> bindingSerializer = new BindingSerializer<RectTerrainResourceDescription>();

        [Test]
        public void ReadWriteTest()
        {
            using (var content = new DirectoryContent(rootDirectory))
            {
                bindingSerializer.Serialize(content, DefaultDescription);
                var description = (RectTerrainResourceDescription)bindingSerializer.Deserialize(content);
                AreEqual(description, DefaultDescription);
            }
        }

        private void AreEqual(RectTerrainResourceDescription v1, RectTerrainResourceDescription v2)
        {
            AreEqual(v1.Landform, v2.Landform);
        }

        private void AreEqual(DescriptionCollection<LandformDescription> v1, DescriptionCollection<LandformDescription> v2)
        {
            Assert.AreEqual(v1.Version, v2.Version);
            Assert.AreEqual(v1.Descriptions[0].DiffuseTex, v2.Descriptions[0].DiffuseTex);
            Assert.AreEqual(v1.Descriptions[0].HeightBlendTex, v2.Descriptions[0].HeightBlendTex);
        }
    }
}
