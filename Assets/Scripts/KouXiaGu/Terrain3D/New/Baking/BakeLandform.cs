using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using KouXiaGu.World;
using KouXiaGu.World.Map;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形烘焙;
    /// </summary>
    [Serializable]
    public class BakeLandform
    {
        BakeLandform()
        {
        }

        public MeshRenderer _prefab;
        public int _maxCapacity = 100;
        GameObjectPool<MeshRenderer> objectPool;

        public Shader _diffuseShader;
        public Shader _heightShader;
        Material diffuseMaterial;
        Material heightMaterial;

        List<Pack> sceneObjects;

        public IWorldData WorldData { get; private set; }
        public RectCoord ChunkCoord { get; private set; }
        public RenderTexture DiffuseRT { get; private set; }
        public RenderTexture HeightRT { get; private set; }

        public void Initialise()
        {
            objectPool = new GameObjectPool<MeshRenderer>(_prefab, _maxCapacity);
            diffuseMaterial = new Material(_diffuseShader);
            heightMaterial = new Material(_heightShader);
            sceneObjects = new List<Pack>();
        }

        /// <summary>
        /// 开始烘培任务;
        /// </summary>
        public IEnumerator Bake(IWorldData worldData, RectCoord chunkCoord, IEnumerable<CubicHexCoord> displays)
        {
            WorldData = worldData;
            ChunkCoord = chunkCoord;

            PrepareScene(displays);
            yield return null;


            ClearScene();
        }

        /// <summary>
        /// 清空场景
        /// </summary>
        void ClearScene()
        {
            foreach (var roadMesh in sceneObjects)
            {
                Release(roadMesh.Rednerer);
            }
            sceneObjects.Clear();
        }

        /// <summary>
        /// 在场景内创建烘培使用的网格;
        /// </summary>
        void PrepareScene(IEnumerable<CubicHexCoord> displays)
        {
            foreach (var display in displays)
            {
                float angle;
                TerrainLandform info = GetLandformInfo(display, out angle);

                if (info != null)
                {
                    var mesh = Get(display, angle, -sceneObjects.Count);
                    sceneObjects.Add(new Pack(info, mesh));
                }
            }
        }

        /// <summary>
        /// 获取到该点的地形贴图信息;
        /// </summary>
        TerrainLandform GetLandformInfo(CubicHexCoord pos, out float angle)
        {
            IDictionary<CubicHexCoord, MapNode> map = WorldData.Map.Data;
            MapNode node;
            if (map.TryGetValue(pos, out node))
            {
                angle = node.Landform.Angle;
                //TerrainLandform info = WorldData
            }
            throw new NotImplementedException();
        }

        public MeshRenderer Get(CubicHexCoord coord, float angle, float y)
        {
            MeshRenderer item = objectPool.Get();
            item.transform.position = coord.GetTerrainPixel(y);
            item.transform.rotation = Quaternion.Euler(0, angle, 0);
            return item;
        }

        public void Release(MeshRenderer mesh)
        {
            objectPool.Release(mesh);
        }

        /// <summary>
        /// 释放所有该实例创建的 RenderTexture 类型的资源;
        /// </summary>
        public void ReleaseAll()
        {
            BakeCamera.ReleaseTemporary(DiffuseRT);
            DiffuseRT = null;

            BakeCamera.ReleaseTemporary(HeightRT);
            HeightRT = null;
        }

        struct Pack
        {
            public Pack(TerrainLandform res, MeshRenderer rednerer)
            {
                this.Res = res;
                this.Rednerer = rednerer;
            }

            public TerrainLandform Res { get; private set; }
            public MeshRenderer Rednerer { get; private set; }
        }

    }

}
