﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UniRx;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 烘培请求;
    /// </summary>
    public interface IBakingRequest : IAsyncOperation<ChunkTexture>, IObservable<ChunkTexture>, IDisposable
    {
        RectCoord ChunkCoord { get; }
    }

}