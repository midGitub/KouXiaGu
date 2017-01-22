using System;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形烘焙请求;
    /// </summary>
    public interface IBakeRequest : IRequest
    {

        /// <summary>
        /// 贴图烘焙完成;
        /// </summary>
        void OnComplete(TerrainTexPack tex);

        /// <summary>
        /// 出现错误时调用;
        /// </summary>
        void OnError(Exception ex);

    }

}
