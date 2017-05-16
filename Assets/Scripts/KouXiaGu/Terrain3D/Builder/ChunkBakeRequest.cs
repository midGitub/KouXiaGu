//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using KouXiaGu.Grids;

//namespace KouXiaGu.Terrain3D
//{

//    class ChunkBakeRequest : IBakeRequest
//    {
//        public ChunkBakeRequest(RectCoord chunkCoord, Chunk chunk, BakeTargets targets)
//        {
//            ChunkCoord = chunkCoord;
//            Chunk = chunk;
//            Targets = targets;
//            inBakeQueueTime = 0;
//            IsBaking = false;
//            IsCanceled = false;
//        }

//        public RectCoord ChunkCoord { get; private set; }
//        public BakeTargets Targets { get; internal set; }
//        public Chunk Chunk { get; private set; }
//        int inBakeQueueTime;
//        public bool IsBaking { get; private set; }
//        public bool IsCanceled { get; private set; }

//        public bool IsInBakeQueue
//        {
//            get { return inBakeQueueTime > 0; }
//        }

//        ChunkTexture IBakeRequest.Textures
//        {
//            get { return Chunk.Renderer; }
//        }

//        void IBakeRequest.AddBakeQueue()
//        {
//            inBakeQueueTime++;
//        }

//        void IBakeRequest.StartBake()
//        {
//            if (IsBaking)
//                UnityEngine.Debug.LogError("重复烘焙?");

//            IsBaking = true;
//        }

//        void IBakeRequest.BakeCompleted()
//        {
//            try
//            {
//                Chunk.Renderer.Apply();
//            }
//            finally
//            {
//                IsBaking = false;
//                inBakeQueueTime--;
//            }
//        }

//        /// <summary>
//        /// 重置状态;
//        /// </summary>
//        internal void ResetState()
//        {
//            IsCanceled = false;
//        }

//        internal void Cancel()
//        {
//            IsCanceled = true;
//        }
//    }


//}
