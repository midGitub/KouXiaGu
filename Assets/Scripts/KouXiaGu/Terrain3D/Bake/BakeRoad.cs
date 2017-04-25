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
    /// 地形道路烘培;
    /// </summary>
    [Serializable]
    class BakeRoad
    {
        [SerializeField]
        RoadMesh prefab = null;
        [SerializeField]
        int maxCapacity = 100;
        [SerializeField, Range(4, 60)]
        public int segmentPoints = 16;
        [SerializeField, Range(0.01f, 2)]
        public float roadWidth = 0.07f;
        [SerializeField]
        Shader diffuseShader = null;
        [SerializeField]
        Shader heightShader = null;

        List<Pack> sceneObjects;
        GameObjectPool<RoadMesh> objectPool;

        BakeCamera bakeCamera;
        IWorldData worldData;
        CubicHexCoord chunkCenter;
        IEnumerable<CubicHexCoord> displays;
        public RenderTexture DiffuseRT { get; private set; }
        public RenderTexture HeightRT { get; private set; }

        IDictionary<CubicHexCoord, MapNode> worldMap
        {
            get { return worldData.Map.Data; }
        }

        IDictionary<int, TerrainRoad> roadResources
        {
            get { return worldData.GameData.Terrain.RoadInfos; }
        }

        public void Initialise()
        {
            sceneObjects = new List<Pack>();
            objectPool = new GameObjectPool<RoadMesh>(prefab, maxCapacity);
        }


        public IEnumerator BakeCoroutine(BakeCamera bakeCamera, IWorldData worldData, CubicHexCoord chunkCenter)
        {
            this.bakeCamera = bakeCamera;
            this.worldData = worldData;
            this.chunkCenter = chunkCenter;
            this.displays = ChunkPartitioner.GetRoad(chunkCenter);

            PrepareScene();
            yield return null;
            BakeDiffuse();
            yield return null;
            BakeHeight();
            yield return null;
            ClearScene();
            yield return null;
        }

        void PrepareScene()
        {
            foreach (var display in displays)
            {
                MapNode node;
                if (worldMap.TryGetValue(display, out node))
                {
                    var meshs = GetRoadInfo(display, node);
                    sceneObjects.Add(meshs);
                }
            }
        }

        void ClearScene()
        {
            foreach (var pack in sceneObjects)
            {
                foreach (var mesh in pack.Rednerers)
                {
                    objectPool.Release(mesh);
                }
            }
            sceneObjects.Clear();
        }


        Pack GetRoadInfo(CubicHexCoord pos, MapNode node)
        {
            TerrainRoad res = GetRoadResource(node.Road.Type);
            IEnumerable<RoadMesh> meshs = CreateMesh(pos);
            return new Pack(res, meshs);
        }

        TerrainRoad GetRoadResource(int roadID)
        {
            TerrainRoad res;
            if (!roadResources.TryGetValue(roadID, out res))
            {
                Debug.LogWarning("未找到对应道路贴图,ID:[" + roadID + "];");
            }
            return res;
        }

        IEnumerable<RoadMesh> CreateMesh(CubicHexCoord target)
        {
            var paths = worldMap.FindPaths(target);

            foreach (var path in paths)
            {
                Vector3[] pixelPath = ConvertPixel(path);

                RoadMesh roadMesh = objectPool.Get();
                roadMesh.SetPath(pixelPath, segmentPoints, roadWidth);

                yield return roadMesh;
            }
        }

        /// <summary>
        /// 转换目标点到相应位置;
        /// </summary>
        Vector3[] ConvertPixel(CubicHexCoord[] path)
        {
            Vector3[] newPath = new Vector3[path.Length];

            for (int i = 0; i < path.Length; i++)
            {
                newPath[i] = path[i].GetTerrainPixel(-sceneObjects.Count);
            }

            return newPath;
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

            DiffuseRT = bakeCamera.GetDiffuseTemporaryRender();
            bakeCamera.CameraRender(DiffuseRT, chunkCenter, Transparent);
        }

        void SetDiffuserMaterial(Pack pack)
        {
            TerrainRoad res = pack.Res;
            Material material = new Material(diffuseShader);
            material.SetTexture("_MainTex", res.DiffuseTex);
            material.SetTexture("_BlendTex", res.DiffuseBlendTex);

            foreach (var renderer in pack.Rednerers)
            {
                if (renderer.MeshRenderer.sharedMaterial != null)
                    GameObject.Destroy(renderer.MeshRenderer.sharedMaterial);

                renderer.MeshRenderer.sharedMaterial = material;
            }
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

        void SetHeightMaterial(Pack pack)
        {
            TerrainRoad res = pack.Res;
            Material material = new Material(heightShader);
            material.SetTexture("_MainTex", res.HeightAdjustTex);

            foreach (var renderer in pack.Rednerers)
            {
                if (renderer.MeshRenderer.sharedMaterial != null)
                    GameObject.Destroy(renderer.MeshRenderer.sharedMaterial);

                renderer.MeshRenderer.sharedMaterial = material;
            }
        }

        struct Pack
        {
            public Pack(TerrainRoad res, IEnumerable<RoadMesh> rednerer)
            {
                this.Res = res;
                this.Rednerers = rednerer;
            }

            public TerrainRoad Res { get; private set; }
            public IEnumerable<RoadMesh> Rednerers { get; private set; }
        }

    }

}
