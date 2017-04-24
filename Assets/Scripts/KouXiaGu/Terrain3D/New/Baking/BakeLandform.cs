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
    /// 地形烘焙;
    /// </summary>
    [Serializable]
    public class BakeLandform
    {
        BakeLandform()
        {
        }

        public MeshRenderer prefab;
        public int maxCapacity = 100;
        GameObjectPool<MeshRenderer> objectPool;

        public Shader diffuseShader;
        public Shader heightShader;
        Material diffuseMaterial;
        Material heightMaterial;

        List<Pack> sceneObjects;

        public IWorldData WorldData { get; private set; }
        public CubicHexCoord ChunkCenter { get; private set; }
        public RenderTexture DiffuseRT { get; private set; }
        public RenderTexture HeightRT { get; private set; }

        public void Initialise()
        {
            objectPool = new GameObjectPool<MeshRenderer>(prefab, maxCapacity);
            diffuseMaterial = new Material(diffuseShader);
            heightMaterial = new Material(heightShader);
            sceneObjects = new List<Pack>();
        }

        /// <summary>
        /// 开始烘培任务;
        /// </summary>
        public IEnumerator Bake(IWorldData worldData, CubicHexCoord chunkCenter, IEnumerable<CubicHexCoord> displays)
        {
            WorldData = worldData;
            ChunkCenter = chunkCenter;

            PrepareScene(displays);
            BakeDiffuse();
            ClearScene();
            yield break;
        }

        /// <summary>
        /// 清空场景
        /// </summary>
        void ClearScene()
        {
            foreach (var roadMesh in sceneObjects)
            {
                GameObject.Destroy(roadMesh.Rednerer);
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
            IDictionary<CubicHexCoord, MapNode> map = WorldData.Map.Data;
            TerrainLandform info;
            MapNode node;
            if (map.TryGetValue(pos, out node))
            {
                info = GetLandformInfo(node);
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

        TerrainLandform GetLandformInfo(MapNode node)
        {
            int landformID = node.Landform.LandformID;
            TerrainLandform info = GetLandformInfo(WorldData.GameData.Terrain, landformID);
            return info;
        }

        TerrainLandform GetLandformInfo(TerrainResource data, int landformID)
        {
            TerrainLandform info;
            if (!data.LandformInfos.TryGetValue(landformID, out info))
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


        /// <summary>
        /// 完全透明颜色;
        /// </summary>
        static readonly Color Transparent = new Color(0, 0, 0, 0);

        void BakeDiffuse()
        {
            foreach (var meshRenderer in sceneObjects)
            {
                SetDiffuserMaterial(meshRenderer);
            }

            DiffuseRT = BakeCamera.GetDiffuseTemporaryRender();
            BakeCamera.CameraRender(DiffuseRT, ChunkCenter, Transparent);
        }

        void SetDiffuserMaterial(Pack renderer)
        {
            TerrainLandform res = renderer.Res;

            if (renderer.Rednerer.material != null)
            {
                GameObject.Destroy(renderer.Rednerer.material);
            }

            var material = renderer.Rednerer.material = new Material(diffuseMaterial);
            material.SetTexture("_MainTex", res.DiffuseTex);
            material.SetTexture("_BlendTex", res.DiffuseBlendTex);
        }

        void BakeHeight()
        {
            foreach (var meshRenderer in sceneObjects)
            {
                SetHeightMaterial(meshRenderer);
            }

            HeightRT = BakeCamera.GetHeightTemporaryRender();
            BakeCamera.CameraRender(HeightRT, ChunkCenter);
        }

        void SetHeightMaterial(Pack renderer)
        {
            TerrainLandform res = renderer.Res;

            if (renderer.Rednerer.material != null)
            {
                GameObject.Destroy(renderer.Rednerer.material);
                renderer.Rednerer.material = heightMaterial;
            }

            var material = renderer.Rednerer.material;
            material.SetTexture("_MainTex", res.HeightTex);
            material.SetTexture("_BlendTex", res.HeightBlendTex);
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
