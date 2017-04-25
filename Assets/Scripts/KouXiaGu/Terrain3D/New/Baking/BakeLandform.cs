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
    public class BakeLandform
    {
        public MeshRenderer prefab;
        public int maxCapacity = 100;
        public Shader diffuseShader;
        public Shader heightShader;

        Material diffuseMaterial;
        Material heightMaterial;
        List<Pack> sceneObjects;
        GameObjectPool<MeshRenderer> objectPool;

        public IWorldData WorldData { get; private set; }
        public CubicHexCoord ChunkCenter { get; private set; }
        public IEnumerable<CubicHexCoord> Displays { get; private set; }
        public RenderTexture DiffuseRT { get; private set; }
        public RenderTexture HeightRT { get; private set; }

        public void Initialise()
        {
            diffuseMaterial = new Material(diffuseShader);
            heightMaterial = new Material(heightShader);
            sceneObjects = new List<Pack>();
            objectPool = new GameObjectPool<MeshRenderer>(prefab, maxCapacity);
        }

        /// <summary>
        /// 获取到烘培协程;
        /// </summary>
        /// <param name="worldData">世界数据</param>
        /// <param name="chunkCenter">地形块中心坐标;</param>
        /// <param name="displays">地形块烘焙时,需要显示到场景的块坐标;</param>
        public IEnumerator GetBakeCoroutine(IWorldData worldData, CubicHexCoord chunkCenter, IEnumerable<CubicHexCoord> displays)
        {
            WorldData = worldData;
            ChunkCenter = chunkCenter;
            Displays = displays;
            return Bake();
        }

        IEnumerator Bake()
        {
            PrepareScene();
            BakeDiffuse();
            yield return null;
            BakeHeight();
            yield return null;
            ClearScene();
            yield break;
        }

        /// <summary>
        /// 重置参数,备下次重复使用,仅在协程中途取消时调用;
        /// </summary>
        public void Reset()
        {
            if (sceneObjects.Count != 0)
                ClearScene();

            ReleaseAll();
        }

        /// <summary>
        /// 清空场景;
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
        /// 释放所有该实例创建的 RenderTexture 类型的资源;
        /// </summary>
        public void ReleaseAll()
        {
            BakeCamera.ReleaseTemporary(DiffuseRT);
            DiffuseRT = null;

            BakeCamera.ReleaseTemporary(HeightRT);
            HeightRT = null;
        }

        /// <summary>
        /// 在场景内创建烘培使用的网格;
        /// </summary>
        void PrepareScene()
        {
            foreach (var display in Displays)
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
                renderer.Rednerer.material = diffuseMaterial;
            }

            var material = renderer.Rednerer.material;
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
