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
        public TerrainAssetReader(AssetBundle assetBundle)
        {
            this.assetBundle = assetBundle;
        }

        AssetBundle assetBundle;

        public abstract T Read(TInfo info);

        public Texture ReadTexture(string name)
        {
            return assetBundle.LoadAsset<Texture>(name);
        }

        public CoroutineOperation<Dictionary<int, T>> ReadAsync(IEnumerable<TInfo> infos)
        {
            throw new NotImplementedException();
        }

        class AsyncReader : CoroutineOperation<Dictionary<int, T>>
        {
            public AsyncReader(TerrainAssetReader<T, TInfo> reader, IEnumerator<TInfo> infos)
            {
                this.reader = reader;
                this.infos = infos;
                Current = new Dictionary<int, T>();
            }

            TerrainAssetReader<T, TInfo> reader;
            IEnumerator<TInfo> infos;

            public override bool MoveNext()
            {
                while (infos.MoveNext())
                {
                    TInfo info = infos.Current;
                    T item = reader.Read(info);
                    Current.Add(info.ID, item);

                    if (NeedPause())
                        return true;
                }
                return false;
            }

            bool NeedPause()
            {
                return false;
            }

        }

    }

}
