using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    class BakingRequest : AsyncOperation<ChunkTexture>, IBakingRequest
    {
        public BakingRequest(RectCoord chunkCoord)
        {
            ChunkCoord = chunkCoord;
        }

        public RectCoord ChunkCoord { get; private set; }

        /// <summary>
        /// 标记为完成;
        /// </summary>
        public void Completed(ChunkTexture texture)
        {
            OnCompleted(texture);
        }

        /// <summary>
        /// 标记为被取消;
        /// </summary>
        public void Cancel()
        {
            OnCanceled();
        }

        public override bool Equals(object obj)
        {
            var item = obj as BakingRequest;

            if (item == null)
                return false;

            return ChunkCoord == item.ChunkCoord;
        }

        public override int GetHashCode()
        {
            return ChunkCoord.GetHashCode();
        }
    }

}
