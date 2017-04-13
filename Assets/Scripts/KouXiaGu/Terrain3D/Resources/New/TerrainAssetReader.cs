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
        public abstract bool TryRead(AssetBundle asset, TInfo info, out T item);

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
        public CoroutineOperation<Dictionary<int, T>> ReadAsync(AssetBundle asset, IEnumerable<TInfo> infos, ISegmented segmented)
        {
            return new AsyncReader(this, asset, infos.GetEnumerator(), segmented);
        }


        /// <summary>
        /// 异步读取;
        /// </summary>
        class AsyncReader : CoroutineOperation<Dictionary<int, T>>
        {
            public AsyncReader(TerrainAssetReader<T, TInfo> reader, AssetBundle asset, IEnumerator<TInfo> infos, ISegmented segmented)
            {
                this.reader = reader;
                this.infos = infos;
                this.segmented = segmented;
                Current = new Dictionary<int, T>();
            }

            TerrainAssetReader<T, TInfo> reader;
            IEnumerator<TInfo> infos;
            ISegmented segmented;
            AssetBundle asset;

            public override bool MoveNext()
            {
                while (infos.MoveNext())
                {
                    TInfo info = infos.Current;
                    T item;

                    if (reader.TryReadAndReport(asset, info, out item))
                    {
                        Current.Add(info.ID, item);
                    }

                    if (segmented.Interrupt())
                        return true;
                }
                return false;
            }
        }

    }

}
