using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using KouXiaGu.World;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形烘焙;
    /// </summary>
    [Serializable]
    class BakeLandform
    {
        BakeLandform()
        {
        }

        public Shader _diffuseShader;
        public Shader _heightShader;
        Material diffuseMaterial;
        Material heightMaterial;

        List<Pack> sceneObjects;
        public _BakeMeshPool bakeMeshPool;

        public RenderTexture DiffuseRT { get; private set; }
        public RenderTexture HeightRT { get; private set; }

        public void Initialise()
        {
            diffuseMaterial = new Material(_diffuseShader);
            heightMaterial = new Material(_heightShader);
            sceneObjects = new List<Pack>();
            bakeMeshPool.Initialise();
        }

        /// <summary>
        /// 开始烘培任务;
        /// </summary>
        public IEnumerator Bake(IWorldData worldData, RectCoord chunkCoord, IEnumerable<CubicHexCoord> displays)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 在场景内创建
        /// </summary>
        void PrepareScene(IWorldData worldData, RectCoord chunkCoord, IEnumerable<CubicHexCoord> displays)
        {
            bakeMeshPool.SetTarget(chunkCoord);
            ClearScene();

            foreach (var display in displays)
            {

            }
        }

        void ClearScene()
        {
            foreach (var roadMesh in sceneObjects)
            {
                bakeMeshPool.Release(roadMesh.Rednerer);
            }
            sceneObjects.Clear();
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

        [Serializable]
        public class _BakeMeshPool : BakeCoordTransform , IObjectPool<MeshRenderer>
        {
            _BakeMeshPool()
            {
            }

            public MeshRenderer _prefab;
            public int _maxCapacity = 100;
            GameObjectPool<MeshRenderer> objectPool;

            public int Count
            {
                get { return objectPool.Count; }
            }

            public void Initialise()
            {
                objectPool = new GameObjectPool<MeshRenderer>(_prefab, _maxCapacity);
            }

            public MeshRenderer Get()
            {
                return objectPool.Get();
            }

            public MeshRenderer Get(CubicHexCoord coord, float angle, float y)
            {
                MeshRenderer item = objectPool.Get();
                item.transform.position = PositionConvert(coord, y);
                item.transform.rotation = Quaternion.Euler(0, angle, 0);
                return item;
            }

            public void Release(MeshRenderer mesh)
            {
                objectPool.Release(mesh);
            }

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
