using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    public abstract class AssetReadRequest<TResult> : CoroutineOperation<TResult>
    {
        public AssetReadRequest(AssetBundle assetBundle, ISegmented segmented) 
            : base(segmented)
        {
            this.assetBundle = assetBundle;
        }

        protected AssetBundle assetBundle { get; private set; }

        protected Texture ReadTexture(string name)
        {
            return assetBundle.LoadAsset<Texture>(name);
        }
    }



    //public abstract class AssetBuilder<TSource, TResult> : CoroutineOperation<TResult>
    //{
    //    public AssetBuilder(AssetBundle assetBundle, ISegmented segmented, IEnumerable<TSource> sources) 
    //        : base(segmented)
    //    {
    //        this.assetBundle = assetBundle;
    //        this.sources = sources;
    //    }

    //    protected AssetBundle assetBundle { get; private set; }
    //    protected IEnumerable<TSource> sources { get; private set; }

    //    public abstract TResult Build(TSource source);

    //    protected Texture ReadTexture(string name)
    //    {
    //        return assetBundle.LoadAsset<Texture>(name);
    //    }

    //    protected override IEnumerator Operate()
    //    {
    //        foreach (var source in sources)
    //        {
    //            TResult result;
    //            if (TryReadAndReport(source, out result))
    //                dictionary.AddOrUpdate(info.ID, item);
    //            yield return null;
    //        }
    //        OnCompleted(dictionary);
    //    }

    //    private bool TryReadAndReport(object info, out TResult item)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

}
