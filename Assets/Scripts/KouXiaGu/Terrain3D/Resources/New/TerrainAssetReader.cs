using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    public abstract class TerrainAssetReader<T, TInfo>
        where TInfo : ElementInfo
    {

        public TerrainAssetReader(ISegmented segmented)
        {
            Segmented = segmented;
        }

        public ISegmented Segmented { get; set; }
        public abstract bool TryRead(AssetBundle asset, TInfo info, out T item);

        /// <summary>
        /// 从资源包内读取贴图;
        /// </summary>
        protected Texture ReadTexture(AssetBundle asset, string name)
        {
            return asset.LoadAsset<Texture>(name);
        }

        /// <summary>
        /// 尝试读取到,若无法读取则输出错误信息,并返回false;
        /// </summary>
        bool TryReadAndReport(AssetBundle asset, TInfo info, out T item)
        {
            if (TryRead(asset, info, out item))
            {
                return true;
            }
            Debug.LogWarning("无法读取[" + typeof(T).Name + "],Info:" + info.ToString());
            return false;
        }

        /// <summary>
        /// 同步读取到;
        /// </summary>
        public Dictionary<int, T> Read(AssetBundle asset, IEnumerable<TInfo> infos)
        {
            Dictionary<int, T> dictionary = new Dictionary<int, T>();

            foreach (var info in infos)
            {
                T item;
                if (TryReadAndReport(asset, info, out item))
                {
                    dictionary.Add(info.ID, item);
                }
            }

            return dictionary;
        }

        /// <summary>
        /// 异步读取到信息合集;
        /// </summary>
        public IEnumerator Read(AssetBundle asset, Dictionary<int, T> dictionary, IDictionary<int, TInfo> infoDictionary)
        {
            IEnumerable<TInfo> infos = infoDictionary.Values;

            foreach (var info in infos)
            {
                T item;

                if (TryReadAndReport(asset, info, out item))
                {
                    dictionary.Add(info.ID, item);
                }

                if (Segmented.Interrupt())
                    yield return null;
            }
        }
    }
}
