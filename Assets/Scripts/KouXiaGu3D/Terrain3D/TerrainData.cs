using System;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Collections;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形数据管理;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TerrainData : UnitySington<TerrainData>
    {
        [SerializeField]
        Shader terrainShader;
        [SerializeField]
        Shader heightShader;

        public static Shader TerrainShader
        {
            get { return GetInstance.terrainShader; }
        }

        public static Shader HeightShader
        {
            get { return GetInstance.heightShader; }
        }

        [SerializeField, Range(0, 32)]
        float tessellation = 32f;
        [SerializeField, Range(0, 5)]
        float displacement = 2f;
        [SerializeField, Range(0,20)]
        float snowLevel = 0f;

        public static float Tessellation
        {
            get { return GetInstance.tessellation; }
            set { Shader.SetGlobalFloat("_TerrainTess", value); GetInstance.tessellation = value; }
        }

        public static float Displacement
        {
            get { return GetInstance.displacement; }
            set { Shader.SetGlobalFloat("_TerrainDisplacement", value); GetInstance.displacement = value; }
        }

        public static float SnowLevel
        {
            get { return GetInstance.snowLevel; }
            set { Shader.SetGlobalFloat("_TerrainSnow", value); GetInstance.snowLevel = value; }
        }

        void Start()
        {
            Tessellation = tessellation;
            Displacement = displacement;
            SnowLevel = snowLevel;
        }

        void OnValidate()
        {
            Start();
        }

        #region 地形方法(静态)

        /// <summary>
        /// 获取到高度,若超出地图边界则返回0;
        /// </summary>
        public static float GetHeight(Vector3 position)
        {
            TerrainChunk chunk;
            RectCoord coord;
            UV uv = TerrainChunk.ChunkGrid.GetUV(position, out coord);

            if (ActivatedChunks.TryGetValue(coord, out chunk))
            {
                return GetHeight(chunk, uv);
            }
            return 0f;
        }

        /// <summary>
        /// 获取到对应的高度;
        /// </summary>
        public static float GetHeight(TerrainChunk chunk, UV uv)
        {
            Color pixelColor = chunk.HeightTexture.GetPixel(uv);
            return pixelColor.r * Displacement;
        }

        /// <summary>
        /// 是否超出了地形的定义范围;
        /// </summary>
        public static bool IsOutTerrain(Vector3 position)
        {
            RectCoord coord = TerrainChunk.ChunkGrid.GetCoord(position);
            return ActivatedChunks.ContainsKey(coord);
        }

        public void SetSnowLevel(float snow)
        {
            SnowLevel = snow;
        }

        #endregion


        #region 地形池;

        /// <summary>
        /// 地形块挂载的脚本;
        /// </summary>
        static readonly Type[] TERRAIN_CHUNK_SCRIPTS = new Type[]
            {
                typeof(TerrainChunk), //必须的;
                typeof(TerrainTrigger) //地形碰撞器;
            };

        /// <summary>
        /// 在场景中激活的地形块;
        /// </summary>
        static CustomDictionary<RectCoord, TerrainChunk> activatedChunks = new CustomDictionary<RectCoord, TerrainChunk>();

        /// <summary>
        /// 休眠的地形块;
        /// </summary>
        static Queue<TerrainChunk> restingChunks = new Queue<TerrainChunk>();

        /// <summary>
        /// 在场景中激活的地形块;
        /// </summary>
        public static IReadOnlyDictionary<RectCoord, TerrainChunk> ActivatedChunks
        {
            get { return activatedChunks; }
        }

        /// <summary>
        /// 重置的\休眠的地形块数目;
        /// </summary>
        public static int RestingChunkCount
        {
            get { return restingChunks.Count; }
        }

        /// <summary>
        /// 创建地形块到场景;
        /// </summary>
        public static TerrainChunk Create(RectCoord coord, Texture2D diffuse, Texture2D height, Texture2D normal)
        {
            if (activatedChunks.ContainsKey(coord))
                throw new ArgumentException("地形块已经创建到场景;");

            TerrainChunk terrainChunk = GetTerrainChunk(coord.ToString());

            terrainChunk.SetChunk(coord, diffuse, height, normal);

            activatedChunks.Add(coord, terrainChunk);

            return terrainChunk;
        }

        /// <summary>
        /// 创建地形块到场景,若已经存在,则更新其贴图;
        /// </summary>
        public static TerrainChunk CreateOrUpdate(RectCoord coord, Texture2D diffuse, Texture2D height, Texture2D normal)
        {
            if (diffuse == null || height == null)
                throw new NullReferenceException("空的贴图!");

            TerrainChunk chunk;

            if (activatedChunks.TryGetValue(coord, out chunk))
            {
                chunk.SetChunk(coord, diffuse, height, normal);
            }
            else
            {
                chunk = Create(coord, diffuse, height, normal);
            }
            return chunk;
        }

        /// <summary>
        /// 移除这个地形块;
        /// </summary>
        public static bool Destroy(RectCoord coord)
        {
            TerrainChunk terrainChunk;
            if (activatedChunks.TryGetValue(coord, out terrainChunk))
            {
                terrainChunk.DestroyTextures();

                ReleaseTerrainChunk(terrainChunk);
                activatedChunks.Remove(coord);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 移除这个地形块,但是返回其贴图信息;
        /// </summary>
        public static bool Destroy(RectCoord coord, out Texture2D diffuse, out Texture2D height)
        {
            TerrainChunk terrainChunk;
            if (activatedChunks.TryGetValue(coord, out terrainChunk))
            {
                diffuse = terrainChunk.DiffuseTexture;
                height = terrainChunk.HeightTexture;
                terrainChunk.ClearTextures();

                ReleaseTerrainChunk(terrainChunk);
                activatedChunks.Remove(coord);
                return true;
            }
            diffuse = default(Texture2D);
            height = default(Texture2D);
            return false;
        }

        /// <summary>
        /// 移除所有的地图块;
        /// </summary>
        public static void DestroyAll()
        {
            RectCoord[] coords = activatedChunks.Keys.ToArray();
            foreach (var coord in coords)
            {
                Destroy(coord);
            }
        }

        /// <summary>
        /// 从池内获取到或者实例化一个;
        /// </summary>
        static TerrainChunk GetTerrainChunk(string name)
        {
            TerrainChunk terrainChunk;
            if (restingChunks.Count > 0)
            {
                terrainChunk = DequeueTerrainChunk(name);
            }
            else
            {
                terrainChunk = CraeteTerrainChunk(name);
            }
            return terrainChunk;
        }

        /// <summary>
        /// 从对象池获取到地形块;
        /// </summary>
        static TerrainChunk DequeueTerrainChunk(string name)
        {
            var terrainChunk = restingChunks.Dequeue();
            terrainChunk.name = name;
            terrainChunk.gameObject.SetActive(true);
            return terrainChunk;
        }

        /// <summary>
        /// 实例一个地形块;
        /// </summary>
        static TerrainChunk CraeteTerrainChunk(string name)
        {
            GameObject gameObject = new GameObject(name, TERRAIN_CHUNK_SCRIPTS);
            var terrainChunk = gameObject.GetComponent<TerrainChunk>();
#if UNITY_EDITOR
            terrainChunk.transform.SetParent(ChunkParent, false);
#endif
            return terrainChunk;
        }

#if UNITY_EDITOR
        /// <summary>
        /// 放置地形块的父节点;
        /// </summary>
        static Transform chunkParent;
        static Transform ChunkParent
        {
            get { return chunkParent ?? (chunkParent = new GameObject("TerrainChunks").transform); }
        }
#endif

        /// <summary>
        /// 将地形块放回池内,备下次使用;
        /// </summary>
        static void ReleaseTerrainChunk(TerrainChunk terrainChunk)
        {
            terrainChunk.gameObject.SetActive(false);
            restingChunks.Enqueue(terrainChunk);
        }

        #endregion


    }

}
