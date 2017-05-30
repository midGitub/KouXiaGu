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
            sceneChunks = new List<SceneWatcher>();
        }

        static readonly List<WaterWatcher> watchers = new List<WaterWatcher>();
        readonly List<SceneWatcher> sceneChunks;

        WaterSettings settings
        {
            get { return LandformSettings.Instance.WaterSettings; }
        }

        public static ICollection<WaterWatcher> Watchers
        {
            get { return watchers; }
        }

        class SceneWatcher
        {
            public SceneWatcher(WaterChunk chunk, WaterWatcher watcher)
            {
                this.chunk = chunk;
                this.watcher = watcher;
            }

            readonly WaterChunk chunk;
            readonly WaterWatcher watcher;
            
            public WaterChunk Chunk
            {
                get { return chunk; }
            }

            public WaterWatcher Watcher
            {
                get { return watcher; }
            }
        }
    }

}
