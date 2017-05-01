using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    class ChunkBakeRequest : IBakeRequest
    {
        public ChunkBakeRequest(RectCoord chunkCoord, Chunk chunk, BakeTargets targets)
        {
            ChunkCoord = chunkCoord;
            Chunk = chunk;
            Targets = targets;
            IsBaking = false;
            IsInBakeQueue = false;
            IsBakeCompleted = false;
            IsCanceled = false;
        }

        public RectCoord ChunkCoord { get; private set; }
        public BakeTargets Targets { get; internal set; }
        public Chunk Chunk { get; private set; }
        public bool IsInBakeQueue { get; private set; }
        public bool IsBaking { get; private set; }
        public bool IsBakeCompleted { get; private set; }
        public bool IsCanceled { get; private set; }

        ChunkTexture IBakeRequest.Textures
        {
            get { return Chunk.Renderer; }
        }

        void IBakeRequest.AddBakeQueue()
        {
            if (IsInBakeQueue)
                UnityEngine.Debug.LogError("重复加入烘培队列?");

            IsInBakeQueue = true;
        }

        void IBakeRequest.StartBake()
        {
            if (IsBaking)
                UnityEngine.Debug.LogError("重复烘焙?");

            IsBaking = true;
        }

        void IBakeRequest.BakeCompleted()
        {
            try
            {
                Chunk.Renderer.Apply();
            }
            finally
            {
                IsBakeCompleted = true;
                IsBaking = false;
                IsInBakeQueue = false;
            }
        }

        /// <summary>
        /// 重置状态;
        /// </summary>
        internal void ResetState()
        {
            IsBakeCompleted = false;
            IsCanceled = false;
        }

        internal void Cancel()
        {
            IsCanceled = true;
        }
    }


}
