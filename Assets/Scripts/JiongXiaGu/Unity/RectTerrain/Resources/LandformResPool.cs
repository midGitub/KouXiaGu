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
    public class LandformResPool
    {
        [SerializeField]
        private LandformRes defaultLandformRes;
        private LandformDescrDictionary descrDictionary;

        public LandformResPool(LandformDescrDictionary descrDictionary)
        {
            this.descrDictionary = descrDictionary;
        }

        public LandformRes Default()
        {
            return defaultLandformRes;
        }

        /// <summary>
        /// 获取到对应资源,若未加载,则开始加载并返回;
        /// </summary>
        public Task<LandformRes> Get(string key, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
            //Task<T> info;
            //if (infos.TryGetValue(key, out info))
            //{
            //    return info.ContinueWith(task => task.Result, token);
            //}
            //else
            //{
            //    info = Create(key, token);
            //    if (!info.IsFaulted)
            //    {
            //        infos.Add(key, info);
            //    }
            //    return info;
            //}
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

            LandformRes info = new LandformRes()
            {
                HeightTex = tasks[0].Result,
                HeightBlendTex = tasks[1].Result,
                DiffuseTex = tasks[2].Result,
                DiffuseBlendTex = tasks[3].Result,
            };
            return info;
        }

        private static Task<Texture2D> GetTexture2DAsync(LoadableContent loadableContent, AssetInfo assetInfo, CancellationToken token)
        {
            return AssetDictionary.Default.LoadAsync(Texture2DLoader.Default, loadableContent, assetInfo, token);
        }
    }
}
