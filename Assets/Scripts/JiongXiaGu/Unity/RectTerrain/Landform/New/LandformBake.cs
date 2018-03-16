using JiongXiaGu.Grids;
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{

    public interface IBakeContent
    {
        RectCoord Target { get; }
        BakeNodeInfo GetInfo(RectCoord pos);
    }

    public struct BakeNodeInfo
    {
        public float Angle { get; internal set; }
        public LandformTextures Textures { get; internal set; }
    }

    public struct BakeResult
    {
        private Texture2D diffuseMap;
        private Texture2D heightMap;

        public Texture2D DiffuseMap
        {
            get { return diffuseMap; }
            set { diffuseMap = value; }
        }

        public Texture2D HeightMap
        {
            get { return heightMap; }
            set { heightMap = value; }
        }
    }

    /// <summary>
    /// 地形烘培;
    /// </summary>
    public sealed class LandformBake
    {

        public BakeResult Bake(IBakeContent content, LandformQuality quality)
        {
            throw new NotImplementedException();
        }

        public Task<BakeResult> BakeAsync(IBakeContent content, LandformQuality quality, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        private IEnumerator BakeInCoroutine(IBakeContent content, LandformQuality quality, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
