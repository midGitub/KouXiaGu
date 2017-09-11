using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace KouXiaGu.RectTerrain.Resources
{


    public static class RectTerrainResourcesTemplateSerializer
    {

        static readonly RectTerrainResources rectTerrainResources = new RectTerrainResources()
        {
            Landform = new Dictionary<string, LandformResource>()
            {
                {
                    "Unknow1",
                    new LandformResource()
                    {
                        Name = "Unknow1",
                        DiffuseTex = "DiffuseTex1",
                        DiffuseBlendTex = "DiffuseBlendTex1",
                        HeightTex = "HeightTex1",
                        HeightBlendTex = "HeightBlendTex1",
                    }
                },
                {
                    "Unknow2",
                    new LandformResource()
                    {
                        Name = "Unknow2",
                        DiffuseTex = "DiffuseTex2",
                        DiffuseBlendTex = "DiffuseBlendTex2",
                        HeightTex = "HeightTex2",
                        HeightBlendTex = "HeightBlendTex2",
                    }
                }
            },
        };

        static readonly TemplateSerializer templateSerializer = new TemplateSerializer();

        [MenuItem("Templates/RectTerrainResources")]
        public static void Serialize()
        {
            templateSerializer.Serialize(rectTerrainResources);
        }

        class TemplateSerializer : RectTerrainResourcesSerializer
        {
            public TemplateSerializer()
            {
                LandformSerializer.ResourceSearcher = LandformSerializer.ResourceSearcher.AsTemplateResourceSearcher();
            }
        }
    }
}
