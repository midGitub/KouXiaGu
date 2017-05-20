using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{


    public class WaterManager
    {

        static Transform chunkObjectParent;

        public static Transform ChunkObjectParent
        {
            get { return chunkObjectParent ?? (chunkObjectParent = new GameObject("WaterChunks").transform); }
        }

        public WaterManager()
        {
            sceneChunks = new List<WaterChunk>();
        }

        readonly List<WaterChunk> sceneChunks;


        //WaterChunk CreateChunk()
        //{

        //}

        class Watcher
        {
            public Watcher()
            {
            }

            readonly WaterChunk chunkObject;

        }
    }

}
