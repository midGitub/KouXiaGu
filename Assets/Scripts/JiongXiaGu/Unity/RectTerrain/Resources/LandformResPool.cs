using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{

    /// <summary>
    /// 负责提供地形资源;
    /// </summary>
    [Serializable]
    public class LandformResPool : ResPool<LandformRes>
    {
        [SerializeField]
        private LandformRes defaultLandformRes;
        private LandformDescrDictionary descrDictionary;

        public LandformResPool(LandformDescrDictionary descrDictionary)
        {
            this.descrDictionary = descrDictionary;
        }

        public override LandformRes Default()
        {
            return defaultLandformRes;
        }

        protected override Task<LandformRes> Create(string key, CancellationToken token)
        {
            Description<LandformDescription> description;
            if (descrDictionary.Descriptions.TryGetValue(key, out description))
            {
                var task = CreateAsync(description.Content, description.Value, token);
                return task;
            }
            else
            {
                return Task.FromException<LandformRes>(new KeyNotFoundException(key));
            }
        }

        protected override Task Release(LandformRes res)
        {
            return TaskHelper.Run(delegate ()
            {
                res.Destroy();
            }, UnityTaskScheduler.TaskScheduler);
        }

        public static async Task<LandformRes> CreateAsync(LoadableContent loadableContent, LandformDescription description, CancellationToken token)
        {
            var tasks = new Task<Texture2D>[]
            {
                GetTexture2DAsync(loadableContent, description.HeightTex, token),
                GetTexture2DAsync(loadableContent, description.HeightBlendTex, token),
                GetTexture2DAsync(loadableContent, description.DiffuseTex, token),
                GetTexture2DAsync(loadableContent, description.DiffuseBlendTex, token),
            };

            await Task.WhenAll(tasks);

            LandformRes info = new LandformRes();
            info.HeightTex = tasks[0].Result;
            info.HeightBlendTex = tasks[1].Result;
            info.DiffuseTex = tasks[2].Result;
            info.DiffuseBlendTex = tasks[3].Result;
            return info;
        }

        private static Task<Texture2D> GetTexture2DAsync(LoadableContent loadableContent, AssetInfo assetInfo, CancellationToken token)
        {
            return AssetPool.Default.ReadAsTexture2D(loadableContent, assetInfo, token);
        }
    }
}
