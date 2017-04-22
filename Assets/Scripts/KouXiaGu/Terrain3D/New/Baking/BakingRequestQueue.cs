using System;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形烘培队列;
    /// </summary>
    class BakingRequestQueue
    {
        public BakingRequestQueue()
        {
            requestQueue = new LinkedList<BakingRequest>();
            readOnleyRequestQueue = requestQueue.AsReadOnlyCollection(item => item.ChunkCoord);
        }

        readonly LinkedList<BakingRequest> requestQueue;
        readonly IReadOnlyCollection<RectCoord> readOnleyRequestQueue;
        public BakingRequest First { get; private set; }

        public IReadOnlyCollection<RectCoord> Requests
        {
            get { return readOnleyRequestQueue; }
        }

        public bool IsEmpty
        {
            get { return First == null; }
        }

        /// <summary>
        /// 创建到队尾,若该坐标已经存在队列中,则返回队列中的实例;
        /// </summary>
        public IBakingRequest Enqueue(RectCoord chunkCoord)
        {
            var request = FirstOrDefault(chunkCoord);

            if (request == null)
            {
                request = new BakingRequest(chunkCoord);

                if (First == null)
                    First = request;
                else
                    requestQueue.AddLast(request);
            }

            return request;
        }

        BakingRequest FirstOrDefault(RectCoord chunkCoord)
        {
            if (IsFirst(chunkCoord) && !First.IsFaulted && !First.IsCanceled)
                return First;

            return requestQueue.FirstOrDefault(request => request.ChunkCoord == chunkCoord);
        }

        /// <summary>
        /// 坐标是否和 First 相符;
        /// </summary>
        bool IsFirst(RectCoord chunkCoord)
        {
            return First != null && First.ChunkCoord == chunkCoord;
        }

        /// <summary>
        /// 设置新的请求到 BakeQueue.First;
        /// </summary>
        public BakingRequest Dequeue()
        {
            if (requestQueue.First == null)
            {
                First = null;
            }
            else
            {
                First = requestQueue.First.Value;
                requestQueue.RemoveFirst();
            }
            return First;
        }

        /// <summary>
        /// 请求取消这个请求;
        /// </summary>
        public bool Cancel(RectCoord chunkCoord)
        {
            if (IsFirst(chunkCoord))
            {
                First.Cancel();
                return true;
            }
            else
            {
                var node = requestQueue.FirstOrDefaultNode(request => request.ChunkCoord == chunkCoord);
                if (node != null)
                {
                    node.Value.Cancel();
                    requestQueue.Remove(node);
                    return true;
                }
                return false;
            }
        }

    }

}
