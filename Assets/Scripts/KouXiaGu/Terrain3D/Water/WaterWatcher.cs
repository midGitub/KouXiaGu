using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 挂载到需要看见水资源的物体上;
    /// </summary>
    [DisallowMultipleComponent]
    public class WaterWatcher : MonoBehaviour
    {
        static WaterWatcher()
        {
            watchers = new List<WaterWatcher>();
        }

        static List<WaterWatcher> watchers;
        static Transform chunkParent;
        WaterChunk chunk;

        WaterSettings settings
        {
            get { return LandformSettings.Instance.WaterSettings; }
        }

        public static List<WaterWatcher> Watchers
        {
            get { return watchers; }
        }

        public static Transform ChunkParent
        {
            get { return chunkParent ?? (chunkParent = new GameObject("WaterChunks").transform); }
        }

        void Awake()
        {
            chunk = CreateWaterChunk();
            watchers.Add(this);
        }

        void OnEnable()
        {
            chunk.gameObject.SetActive(true);
        }

        void Update()
        {
            Vector3 position = transform.position;
            position.y = settings.SeaLevel;
            chunk.transform.position = position;
        }

        void OnDisable()
        {
            if (chunk != null)
            {
                chunk.gameObject.SetActive(false);
            }
        }

        void OnDestroy()
        {
            watchers.Remove(this);
            if (chunk != null)
            {
                GameObject.Destroy(chunk.gameObject);
            }
        }

        WaterChunk CreateWaterChunk()
        {
            var chunk = Instantiate(settings.PrefabChunk, ChunkParent);
            chunk.name = name;
            return chunk;
        }
    }
}
