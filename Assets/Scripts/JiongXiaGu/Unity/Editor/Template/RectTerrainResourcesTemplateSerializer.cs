﻿//using JiongXiaGu.Unity.RectTerrain;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEditor;

//namespace JiongXiaGu.Unity.Template
//{


//    public static class RectTerrainResourcesTemplateSerializer
//    {

//        static readonly RectTerrainResources rectTerrainResources = new RectTerrainResources()
//        {
//            Landform = new Dictionary<int, LandformRes>()
//            {
//                {
//                    1,
//                    new LandformRes()
//                    {
//                        ID = 1,
//                        Name = "Unknow1",
//                        DiffuseTex = "DiffuseTex1",
//                        DiffuseBlendTex = "DiffuseBlendTex1",
//                        HeightTex = "HeightTex1",
//                        HeightBlendTex = "HeightBlendTex1",
//                    }
//                },
//                {
//                    2,
//                    new LandformRes()
//                    {
//                        ID = 2,
//                        Name = "Unknow2",
//                        DiffuseTex = "DiffuseTex2",
//                        DiffuseBlendTex = "DiffuseBlendTex2",
//                        HeightTex = "HeightTex2",
//                        HeightBlendTex = "HeightBlendTex2",
//                    }
//                }
//            },
//        };

//        static readonly TemplateSerializer templateSerializer = new TemplateSerializer();

//        [MenuItem("Templates/RectTerrainResources")]
//        public static void Serialize()
//        {
//            templateSerializer.Serialize(rectTerrainResources);
//        }

//        class TemplateSerializer : RectTerrainResourcesSerializer
//        {
//            public TemplateSerializer()
//            {
//                LandformSerializer.ResourceSearcher = LandformSerializer.ResourceSearcher.AsTemplateResourceSearcher();
//            }
//        }
//    }
//}
