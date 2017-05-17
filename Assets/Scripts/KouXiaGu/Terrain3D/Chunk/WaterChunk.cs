using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityStandardAssets.Water;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace KouXiaGu.Terrain3D
{


    [DisallowMultipleComponent]
    public class WaterChunk : MonoBehaviour
    {
        #region 静态;
        static readonly Type[] ChunkScripts = new Type[]
           {
                typeof(MeshFilter),
                typeof(MeshRenderer),
                typeof(Water),
                typeof(WaterChunk),
           };

        static Transform chunkObjectParent;
        const string defaultChunkName = "WaterChunk";

        public static Transform ChunkObjectParent
        {
            get { return chunkObjectParent ?? (chunkObjectParent = new GameObject("WaterChunks").transform); }
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Create Other/WaterChunk")]
#endif
        static void _CraeteLandformChunk()
        {
            new GameObject(defaultChunkName, ChunkScripts);
        }

        public static Chunk Create()
        {
            return Create(defaultChunkName);
        }

        public static Chunk Create(string name)
        {
            GameObject gameObject = new GameObject(name, ChunkScripts);
            gameObject.transform.SetParent(ChunkObjectParent);
            Chunk chunk = gameObject.GetComponent<Chunk>();
            return chunk;
        }
        #endregion
    }

}
