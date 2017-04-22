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
    class BakeQueue
    {
        public BakeQueue()
        {
            requestQueue = new LinkedList<BakingRequest>();
            readOnleyRequestQueue = requestQueue.AsReadOnlyCollection(item => item.ChunkCoord);
        }

        readonly LinkedList<BakingRequest> requestQueue;
        readonly IReadOnlyCollection<RectCoord> readOnleyRequestQueue;
        public BakingRequest Current { get; private set; }

        public IReadOnlyCollection<RectCoord> RequestQueue
        {
            get { return readOnleyRequestQueue; }
        }

        public bool IsEmpty
        {
            get { return requestQueue.Count == 0; }
        }

        /// <summary>
        /// 创建到队尾,若该坐标已经存在队列中,则返回队列中的实例;
        /// </summary>
        public BakingRequest Enqueue(RectCoord chunkCoord)
        {
            var request = FirstOrDefault(chunkCoord);

            if (request == null)
            {
                request = new BakingRequest(chunkCoord);
                requestQueue.AddLast(request);
            }

            return request;
        }

        public BakingRequest FirstOrDefault(RectCoord chunkCoord)
        {
            if (Current.ChunkCoord == chunkCoord)
                return Current;

            return requestQueue.FirstOrDefault(request => request.ChunkCoord == chunkCoord);
        }

        /// <summary>
        /// 设置新的请求到 BakeQueue.Current;
        /// </summary>
        public BakingRequest Dequeue()
        {
            Current = requestQueue.First.Value;
            requestQueue.RemoveFirst();
            return Current;
        }

        /// <summary>
        /// 请求取消这个请求;
        /// </summary>
        public bool Cancel(RectCoord chunkCoord)
        {
            if (Current.ChunkCoord == chunkCoord)
            {
                Current.Cancel();
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
