using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形之上的建筑物;
    /// </summary>
    public class Building : UnitySingleton<Building>
    {

        public Shader shader;
        public Texture heightMap;
        public Texture buildMap;

        public Material material;

        [ContextMenu("输出")]
        void Test()
        {
            material = material != null ? material : new Material(shader);

            var rt = RenderTexture.GetTemporary(heightMap.width, heightMap.height);

            material.SetTexture("_MainTex", heightMap);
            material.SetTexture("_HeightMap", buildMap);
            Graphics.Blit(heightMap, rt, material);

            rt.SavePNG("F:\\My_Code\\Unity5\\KouXiaGu\\Assets\\Textrue\\Test");
            RenderTexture.ReleaseTemporary(rt);
        }


    }

}
