using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    public static class Texture2DAssetExtensions
    {
        public static Task<Texture2D> ReadAsTexture2D(this AssetDictionary assetPool, ModificationContent content, AssetInfo assetInfo, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}