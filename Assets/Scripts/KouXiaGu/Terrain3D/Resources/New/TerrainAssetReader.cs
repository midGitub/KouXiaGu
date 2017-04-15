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

}
