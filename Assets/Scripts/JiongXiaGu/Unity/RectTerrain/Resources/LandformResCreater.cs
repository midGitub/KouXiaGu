using JiongXiaGu.Collections;
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
    public class LandformResCreater
    {
        [SerializeField]
        private LandformRes defaultLandformRes;
        private Dictionary<string, DescriptionInfo> descriptionDictionary;

        public LandformResCreater()
        {
            descriptionDictionary = new Dictionary<string, DescriptionInfo>();
        }

        /// <summary>
        /// 添加描述信息;
        /// </summary>
        public void Add(ModificationContent content, DescriptionCollection<LandformDescription> descriptions)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            foreach (var description in descriptions.EnumerateDescription())
            {
                string key = description.ID;
                DescriptionInfo info;
                if (descriptionDictionary.TryGetValue(key, out info))
                {
                    descriptionDictionary[key] = new DescriptionInfo(content, description);
                }
                else
                {
                    info = new DescriptionInfo(content, description);
                    descriptionDictionary.Add(key, info);
                }
            }
        }

        /// <summary>
        /// 清空描述;
        /// </summary>
        public void Clear()
        {
            descriptionDictionary.Clear();
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
            DescriptionInfo info;
            if (descriptionDictionary.TryGetValue(key, out info))
            {
                return CreateAsync(info.Content, info.Description, token);
            }
            else
            {
                throw new KeyNotFoundException(key);
            }
        }

        public static async Task<LandformRes> CreateAsync(ModificationContent content, LandformDescription description, CancellationToken token)
        {
            var tasks = new Task<Texture2D>[]
            {
                GetTexture2DAsync(content, description.HeightTex, token),
                GetTexture2DAsync(content, description.HeightBlendTex, token),
                GetTexture2DAsync(content, description.DiffuseTex, token),
                GetTexture2DAsync(content, description.DiffuseBlendTex, token),
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

        private static Task<Texture2D> GetTexture2DAsync(ModificationContent loadableContent, AssetInfo assetInfo, CancellationToken token)
        {
            return AssetDictionary.Default.LoadAsync(Texture2DLoader.Default, loadableContent, assetInfo, token);
        }

        private struct DescriptionInfo
        {
            public ModificationContent Content { get; private set; }
            public LandformDescription Description { get; private set; }

            public DescriptionInfo(ModificationContent content, LandformDescription description)
            {
                Content = content;
                Description = description;
            }
        }
    }
}
