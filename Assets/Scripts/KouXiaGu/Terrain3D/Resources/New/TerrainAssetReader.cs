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

        static readonly ISegmented DefaultSegmented = new SegmentedBlock();

        public TerrainAssetReader(AssetBundle assetBundle)
        {
            this.assetBundle = assetBundle;
        }

        protected AssetBundle assetBundle { get; private set; }
        public abstract bool TryRead(TInfo info, out T item);

        public Texture ReadTexture(string name)
        {
            return assetBundle.LoadAsset<Texture>(name);
        }

        /// <summary>
        /// 尝试读取到,若无法读取则输出错误信息,并返回false;
        /// </summary>
        bool TryReadAndReport(TInfo info, out T item)
        {
            if (TryRead(info, out item))
            {
                return true;
            }
            Debug.LogWarning("无法读取[" + typeof(T).Name + "],Info:" + info.ToString());
            return false;
        }

        /// <summary>
        /// 异步读取到信息合集;
        /// </summary>
        public CoroutineOperation<Dictionary<int, T>> ReadAsync(IEnumerable<TInfo> infos)
        {
            return ReadAsync(infos, DefaultSegmented);
        }

        /// <summary>
        /// 异步读取到信息合集;
        /// </summary>
        public CoroutineOperation<Dictionary<int, T>> ReadAsync(IEnumerable<TInfo> infos, ISegmented segmented)
        {
            return new AsyncReader(this, infos.GetEnumerator(), segmented);
        }


        /// <summary>
        /// 异步读取;
        /// </summary>
        class AsyncReader : CoroutineOperation<Dictionary<int, T>>
        {
            public AsyncReader(TerrainAssetReader<T, TInfo> reader, IEnumerator<TInfo> infos, ISegmented segmented)
            {
                this.reader = reader;
                this.infos = infos;
                this.segmented = segmented;
                Current = new Dictionary<int, T>();
            }

            TerrainAssetReader<T, TInfo> reader;
            IEnumerator<TInfo> infos;
            ISegmented segmented;

            public override bool MoveNext()
            {
                while (infos.MoveNext())
                {
                    TInfo info = infos.Current;
                    T item;

                    if (reader.TryReadAndReport(info, out item))
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
