using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形烘培队列;
    /// </summary>
    public class BakeQueue
    {
        public BakeQueue()
        {
            requestQueue = new LinkedList<Baker>();
            readOnleyRequestQueue = requestQueue.AsReadOnlyCollection(item => item.ChunkCoord);
        }

        readonly LinkedList<Baker> requestQueue;
        readonly IReadOnlyCollection<RectCoord> readOnleyRequestQueue;

        public IReadOnlyCollection<RectCoord> RequestQueue
        {
            get { return readOnleyRequestQueue; }
        }

        public IBakingRequest Current
        {
            get { return requestQueue.First.Value; }
        }

        /// <summary>
        /// 请求烘培这个地图块,若已经存在请求,则返回存在的请求;
        /// </summary>
        public IBakingRequest Bake(RectCoord chunkCoord)
        {
            Baker request = requestQueue.FirstOrDefault(item => item.ChunkCoord == chunkCoord);

            if (request == null)
            {
                request = new Baker(chunkCoord);
                requestQueue.AddLast(request);
            }

            return request;
        }

        /// <summary>
        /// 请求取消这个地图块的烘培请求;
        /// </summary>
        public bool Cancel(RectCoord chunkCoord)
        {
            var node = requestQueue.FindNode(request => request.ChunkCoord == chunkCoord);
            if (node != null)
            {
                node.Value.Cancel();
                requestQueue.Remove(node);
                return true;
            }
            return false;
        }

        public class Baker : AsyncOperation<ChunkTexture>, IBakingRequest
        {
            public Baker(RectCoord chunkCoord)
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
                var item = obj as Baker;

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

}
