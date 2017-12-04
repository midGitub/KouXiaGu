using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{

    /// <summary>
    /// 地形资源;
    /// </summary>
    public class LandformRes
    {
        public LoadableContent LoadableContent { get; private set; }
        public LandformDescription Description { get; private set; }
        public Texture2D HeightTex { get; private set; }
        public Texture2D HeightBlendTex { get; private set; }
        public Texture2D DiffuseTex { get; private set; }
        public Texture2D DiffuseBlendTex { get; private set; }

        private LandformRes(LoadableContent loadableContent, LandformDescription description)
        {
            LoadableContent = loadableContent;
            Description = description;
        }

        public void Destroy()
        {
            UnityEngine.Object.Destroy(HeightTex);
            UnityEngine.Object.Destroy(HeightBlendTex);
            UnityEngine.Object.Destroy(DiffuseTex);
            UnityEngine.Object.Destroy(DiffuseBlendTex);
        }

        public static async Task<LandformRes> CreateAsync(LoadableContent loadableContent, LandformDescription description)
        {
            if (loadableContent == null)
                throw new ArgumentNullException(nameof(loadableContent));

            LandformRes info = new LandformRes(loadableContent, description);
            List<Task> tasks = new List<Task>();

            tasks.Add(GetTexture2DAsync(loadableContent, description.HeightTex).ContinueWith(task => info.HeightTex = task.Result));
            tasks.Add(GetTexture2DAsync(loadableContent, description.HeightBlendTex).ContinueWith(task => info.HeightBlendTex = task.Result));
            tasks.Add(GetTexture2DAsync(loadableContent, description.DiffuseTex).ContinueWith(task => info.DiffuseTex = task.Result));
            tasks.Add(GetTexture2DAsync(loadableContent, description.DiffuseBlendTex).ContinueWith(task => info.DiffuseBlendTex = task.Result));

            await Task.WhenAll(tasks);
            return info;
        }

        private static Task<Texture2D> GetTexture2DAsync(LoadableContent loadableContent, AssetInfo assetInfo)
        {
            return loadableContent.ReadAsTexture2D(assetInfo);
        }
    }
}
