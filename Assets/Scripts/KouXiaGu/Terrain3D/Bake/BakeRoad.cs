using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField, Range(4, 60)]
        public int segmentPoints = 16;
        [SerializeField, Range(0.01f, 2)]
        public float roadWidth = 0.07f;
        [SerializeField]
        Shader diffuseShader = null;
        [SerializeField]
        Shader heightShader = null;

        List<Pack> sceneObjects;
        RoadMeshPool objectPool;

        BakeCamera bakeCamera;
        IWorldData worldData;
        CubicHexCoord chunkCenter;
        IEnumerable<CubicHexCoord> displays;
        public RenderTexture DiffuseRT { get; private set; }
        public RenderTexture HeightRT { get; private set; }

        IReadOnlyDictionary<CubicHexCoord, MapNode> map
        {
            get { return worldData.MapData.ReadOnlyMap; }
        }

        IDictionary<int, RoadResource> roadResources
        {
            get { return worldData.GameData.Terrain.RoadInfos; }
        }

        public void Initialise()
        {
            sceneObjects = new List<Pack>();
            objectPool = new RoadMeshPool(prefab, "BakeRoadMesh");
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

        public IEnumerator BakeCoroutine(BakeCamera bakeCamera, IWorldData worldData, CubicHexCoord chunkCenter, ICoroutineState state)
        {
            this.bakeCamera = bakeCamera;
            this.worldData = worldData;
            this.chunkCenter = chunkCenter;
            this.displays = ChunkPartitioner.GetRoad(chunkCenter);

            PrepareScene();
            if (state.IsCanceled)
            {
                goto _End_;
            }
            if (state.Await())
            {
                yield return null;
                state.Restart();
            }


            BakeDiffuse();
            if (state.IsCanceled)
            {
                goto _End_;
            }
            if (state.Await())
            {
                yield return null;
                state.Restart();
            }


            BakeHeight();
            if (state.Await())
            {
                yield return null;
                state.Restart();
            }

        _End_:
            ClearScene();
        }

        void PrepareScene()
        {
            foreach (var display in displays)
            {
                MapNode node;
                if (map.TryGetValue(display, out node))
                {
                    var meshs = CreateMesh(display).ToArray();

                    if (meshs.Length > 0)
                    {
                        RoadResource res = GetRoadResource(node.Road.RoadType);
                        var pack = new Pack(res, meshs);
                        sceneObjects.Add(pack);
                    }
                }
            }
        }

        IEnumerable<Pack<RoadMesh, MeshRenderer>> CreateMesh(CubicHexCoord target)
        {
            var paths = GetPeripheralRoutes(target);

            foreach (var path in paths)
            {
                Vector3[] pixelPath = ConvertPixel(path);

                var roadMesh = objectPool.Get();
                roadMesh.Value1.SetPath(pixelPath, segmentPoints, roadWidth);

                yield return roadMesh;
            }
        }

        /// <summary>
        /// 迭代获取到这个点通向周围的路径点,若不存在节点则不进行迭代;
        /// </summary>
        public IEnumerable<CubicHexCoord[]> GetPeripheralRoutes(CubicHexCoord target)
        {
            PeripheralRoute.TryGetPeripheralValue tryGetValue = delegate (CubicHexCoord position, out uint value)
            {
                MapNode node;
                if (map.TryGetValue(position, out node))
                {
                    if (node.Road.Exist())
                    {
                        value = node.Road.ID;
                        return true;
                    }
                }
                value = default(uint);
                return false;
            };
            return PeripheralRoute.GetRoadRoutes(target, tryGetValue);
        }

        Vector3[] ConvertPixel(CubicHexCoord[] path)
        {
            Vector3[] newPath = new Vector3[path.Length];

            for (int i = 0; i < path.Length; i++)
            {
                newPath[i] = path[i].GetTerrainPixel();
            }

            return newPath;
        }

        RoadResource GetRoadResource(int roadID)
        {
            RoadResource res;
            if (!roadResources.TryGetValue(roadID, out res))
            {
                Debug.LogWarning("未找到对应道路贴图,ID:[" + roadID + "];");
            }
            return res;
        }


        void ClearScene()
        {
            foreach (var pack in sceneObjects)
            {
                foreach (var mesh in pack.Packs)
                {
                    objectPool.Release(mesh);
                }
            }
            sceneObjects.Clear();
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

        void SetDiffuserMaterial(Pack pack)
        {
            RoadResource res = pack.Res;
            Material material = new Material(diffuseShader);
            material.SetTexture("_MainTex", res.DiffuseTex);
            material.SetTexture("_BlendTex", res.DiffuseBlendTex);

            foreach (var item in pack.Packs)
            {
                if (item.Value2.sharedMaterial != null)
                    GameObject.Destroy(item.Value2.sharedMaterial);

                item.Value2.sharedMaterial = material;
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
            RoadResource res = pack.Res;
            Material material = new Material(heightShader);
            material.SetTexture("_MainTex", res.HeightAdjustTex);

            foreach (var item in pack.Packs)
            {
                if (item.Value2.sharedMaterial != null)
                    GameObject.Destroy(item.Value2.sharedMaterial);

                item.Value2.sharedMaterial = material;
            }
        }

        struct Pack
        {
            public Pack(RoadResource res, IEnumerable<Pack<RoadMesh, MeshRenderer>> rednerer)
            {
                this.Res = res;
                this.Packs = rednerer;
            }

            public RoadResource Res { get; private set; }
            public IEnumerable<Pack<RoadMesh, MeshRenderer>> Packs { get; private set; }
        }


        class RoadMeshPool : ObjectPool<Pack<RoadMesh, MeshRenderer>>
        {
            public RoadMeshPool(RoadMesh prefab, string parentName)
            {
                this.prefab = prefab;
                this.objectParent = new GameObject(parentName).transform;
            }

            readonly RoadMesh prefab;
            readonly Transform objectParent;

            public override Pack<RoadMesh, MeshRenderer> Instantiate()
            {
                var mesh = GameObject.Instantiate(prefab);
                mesh.transform.SetParent(objectParent);
                MeshRenderer renderer = mesh.GetComponent<MeshRenderer>();
                return new Pack<RoadMesh, MeshRenderer>(mesh, renderer);
            }

            public override void Destroy(Pack<RoadMesh, MeshRenderer> item)
            {
                var gameObject = item.Value1.gameObject;
                GameObject.Destroy(gameObject);
            }

            public override void ResetWhenEnterPool(Pack<RoadMesh, MeshRenderer> item)
            {
                item.Value1.Reset();
                item.Value1.gameObject.SetActive(false);
            }

            public override void ResetWhenOutPool(Pack<RoadMesh, MeshRenderer> item)
            {
                item.Value1.gameObject.SetActive(true);
            }
        }

    }

}
