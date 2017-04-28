using System;
using System.Collections;
using System.Collections.Generic;
using KouXiaGu.Grids;
using KouXiaGu.World;
using KouXiaGu.World.Map;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形贴图烘焙;
    /// </summary>
    [Serializable]
    class BakeLandform
    {
        [SerializeField]
        MeshRenderer prefab = null;
        [SerializeField]
        int maxCapacity = 100;
        [SerializeField]
        Shader diffuseShader = null;
        [SerializeField]
        Shader heightShader = null;

        List<Pack> sceneObjects;
        GameObjectPool<MeshRenderer> objectPool;

        BakeCamera bakeCamera;
        IWorldData worldData;
        CubicHexCoord chunkCenter;
        IEnumerable<CubicHexCoord> displays;
        Material diffuseMaterial;
        Material heightMaterial;
        public RenderTexture DiffuseRT { get; private set; }
        public RenderTexture HeightRT { get; private set; }

        IDictionary<CubicHexCoord, MapNode> worldMap
        {
            get { return worldData.Map.Data; }
        }

        IDictionary<int, TerrainLandform> landformResources
        {
            get { return worldData.GameData.Terrain.LandformInfos; }
        }

        public void Initialize()
        {
            diffuseMaterial = new Material(diffuseShader);
            heightMaterial = new Material(heightShader);
            prefab.material = diffuseMaterial;
            sceneObjects = new List<Pack>();
            objectPool = new GameObjectPool<MeshRenderer>(prefab, maxCapacity);
        }

        /// <summary>
        /// 获取到烘培协程;
        /// </summary>
        /// <param name="worldData">世界数据</param>
        /// <param name="chunkCenter">地形块中心坐标;</param>
        /// <param name="displays">地形块烘焙时,需要显示到场景的块坐标;</param>
        public IEnumerator BakeCoroutine(BakeCamera bakeCamera, IWorldData worldData, CubicHexCoord chunkCenter)
        {
            this.bakeCamera = bakeCamera;
            this.worldData = worldData;
            this.chunkCenter = chunkCenter;
            this.displays = ChunkPartitioner.GetLandform(chunkCenter);

            PrepareScene();
            yield return null;
            BakeDiffuse();
            yield return null;
            BakeHeight();
            yield return null;
            ClearScene();
            yield return null;
        }

        /// <summary>
        /// 重置所有参数,在协程运行失败时候调用;
        /// </summary>
        public void ResetAll()
        {
            if (sceneObjects.Count != 0)
                ClearScene();

            Reset();
        }

        /// <summary>
        /// 释放所有该实例创建的 RenderTexture 类型的资源;
        /// </summary>
        public void Reset()
        {
            bakeCamera.ReleaseTemporary(DiffuseRT);
            DiffuseRT = null;

            bakeCamera.ReleaseTemporary(HeightRT);
            HeightRT = null;
        }

        /// <summary>
        /// 清空场景;
        /// </summary>
        void ClearScene()
        {
            foreach (var roadMesh in sceneObjects)
            {
                objectPool.Release(roadMesh.Rednerer);
            }
            sceneObjects.Clear();
        }

        /// <summary>
        /// 在场景内创建烘培使用的网格;
        /// </summary>
        void PrepareScene()
        {
            foreach (var display in displays)
            {
                float angle;
                TerrainLandform info = GetLandformInfo(display, out angle);

                if (info != null)
                {
                    var mesh = CreateMeshRenderer(display, angle, -sceneObjects.Count);
                    sceneObjects.Add(new Pack(info, mesh));
                }
            }
        }

        /// <summary>
        /// 获取到该点的地形贴图信息;
        /// </summary>
        TerrainLandform GetLandformInfo(CubicHexCoord pos, out float angle)
        {
            TerrainLandform info;
            MapNode node;
            if (worldMap.TryGetValue(pos, out node))
            {
                info = GetLandformResource(node);
                angle = GetLandformAngle(node);
            }
            else
            {
                info = default(TerrainLandform);
                angle = default(float);
            }
            return info;
        }

        float GetLandformAngle(MapNode node)
        {
            float angle = node.Landform.Angle;
            return angle;
        }

        TerrainLandform GetLandformResource(MapNode node)
        {
            int landformID = node.Landform.LandformID;
            TerrainLandform info = GetLandformResource(landformID);
            return info;
        }

        TerrainLandform GetLandformResource(int landformID)
        {
            TerrainLandform info;
            if (!landformResources.TryGetValue(landformID, out info))
            {
                Debug.LogWarning("未找到对应地形贴图,ID:[" + landformID + "];");
            }
            return info;
        }

        public MeshRenderer CreateMeshRenderer(CubicHexCoord coord, float angle, float y)
        {
            MeshRenderer item = objectPool.Get();
            item.transform.position = coord.GetTerrainPixel(y);
            item.transform.rotation = Quaternion.Euler(0, angle, 0);
            return item;
        }


        void BakeDiffuse()
        {
            foreach (var meshRenderer in sceneObjects)
            {
                SetDiffuserMaterial(meshRenderer);
            }

            DiffuseRT = bakeCamera.GetDiffuseTemporaryRender();
            bakeCamera.CameraRender(DiffuseRT, chunkCenter, LandformBaker.BlackTransparent);
        }

        void SetDiffuserMaterial(Pack renderer)
        {
            TerrainLandform res = renderer.Res;

            var material = renderer.Rednerer.material;
            //GameObject.Destroy(material);
            //material = renderer.Rednerer.material = diffuseMaterial;

            material.SetTexture("_MainTex", res.DiffuseTex);
            material.SetTexture("_BlendTex", res.DiffuseBlendTex);
        }

        void BakeHeight()
        {
            foreach (var meshRenderer in sceneObjects)
            {
                SetHeightMaterial(meshRenderer);
            }

            HeightRT = bakeCamera.GetHeightTemporaryRender();
            bakeCamera.CameraRender(HeightRT, chunkCenter);
        }

        void SetHeightMaterial(Pack renderer)
        {
            TerrainLandform res = renderer.Res;

            var material = renderer.Rednerer.material;
            //GameObject.Destroy(material);
            //material = renderer.Rednerer.material = heightMaterial;

            material.SetTexture("_MainTex", res.HeightTex);
            material.SetTexture("_BlendTex", res.HeightBlendTex);
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
