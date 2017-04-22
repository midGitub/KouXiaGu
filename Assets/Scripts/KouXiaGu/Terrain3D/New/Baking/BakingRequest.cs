using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    class BakingRequest : AsyncOperation<ChunkTexture>, IBakingRequest, IEnumerator
    {
        public BakingRequest(RectCoord chunkCoord)
        {
            this.chunkCoord = chunkCoord;
            bakeCoroutine = BakeCoroutine();
        }

        readonly RectCoord chunkCoord;
        readonly IEnumerator bakeCoroutine;

        public RectCoord ChunkCoord
        {
            get { return chunkCoord; }
        }

        object IEnumerator.Current
        {
            get { return null; }
        }

        void IEnumerator.Reset()
        {
            return;
        }

        public bool MoveNext()
        {
            return bakeCoroutine.MoveNext();
        }

        /// <summary>
        /// 标记为被取消;
        /// </summary>
        public void Cancel()
        {
            OnCanceled();
        }

        IEnumerator BakeCoroutine()
        {
            throw new NotImplementedException();
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
