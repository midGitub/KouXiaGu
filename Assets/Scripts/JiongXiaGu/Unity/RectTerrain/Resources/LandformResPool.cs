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
                var info = Create(description.Content, description.Value, token);
                return info;
            }
            else
            {
                return Task.FromException<LandformRes>(new KeyNotFoundException(key));
            }
        }

        protected override void Release(LandformRes res)
        {
            res.Destroy();
        }

        public static Task<LandformRes> Create(LoadableContent loadableContent, LandformDescription description, CancellationToken token)
        {
            throw new NotImplementedException();
            //if (loadableContent == null)
            //    throw new ArgumentNullException(nameof(loadableContent));

            //LandformRes info = new LandformRes();
            //List<Task> tasks = new List<Task>();

            //tasks.Add(GetTexture2DAsync(loadableContent, description.HeightTex, token).ContinueWith(task => info.HeightTex = task.Result, token));
            //tasks.Add(GetTexture2DAsync(loadableContent, description.HeightBlendTex, token).ContinueWith(task => info.HeightBlendTex = task.Result, token));
            //tasks.Add(GetTexture2DAsync(loadableContent, description.DiffuseTex, token).ContinueWith(task => info.DiffuseTex = task.Result, token));
            //tasks.Add(GetTexture2DAsync(loadableContent, description.DiffuseBlendTex, token).ContinueWith(task => info.DiffuseBlendTex = task.Result, token));

            //await Task.WhenAll(tasks);
            //return info;
        }

        //private static Task<Texture2D> GetTexture2DAsync(LoadableContent loadableContent, AssetInfo assetInfo, CancellationToken token)
        //{
        //    return loadableContent.ReadAsTexture2D(assetInfo);
        //}
    }
}
