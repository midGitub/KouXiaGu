﻿using System;
using System.Collections;
using System.Collections.Generic;
using KouXiaGu.Grids;
using KouXiaGu.World;
using KouXiaGu.World.Map;
using UnityEngine;
using KouXiaGu.World.Resources;

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
        Shader diffuseShader = null;
        [SerializeField]
        Shader heightShader = null;

        List<Pack> sceneObjects;
        GameObjectPool<MeshRenderer> objectPool;

        BakeCamera bakeCamera;
        IWorldData worldData;
        CubicHexCoord chunkCenter;
        IEnumerable<CubicHexCoord> displays;
        public RenderTexture DiffuseRT { get; private set; }
        public RenderTexture HeightRT { get; private set; }

        IReadOnlyDictionary<CubicHexCoord, MapNode> worldMap
        {
            get { return worldData.MapData.ReadOnlyMap; }
        }

        IDictionary<int, OLandformInfo> landformInfos
        {
            get { throw new NotImplementedException(); }
            //get { return worldData.BasicData.BasicResource.Terrain.Landform; }
        }

        public void Initialize()
        {
            sceneObjects = new List<Pack>();
            objectPool = new GameObjectPool<MeshRenderer>(prefab, "BakeLandformMesh");
        }

        /// <summary>
        /// 获取到烘培协程;
        /// </summary>
        /// <param name="world">世界数据</param>
        /// <param name="chunkCenter">地形块中心坐标;</param>
        /// <param name="displays">地形块烘焙时,需要显示到场景的块坐标;</param>
        public void BakeCoroutine(BakeCamera bakeCamera, IWorldData world, CubicHexCoord chunkCenter, LandformRenderer textures)
        {
            this.bakeCamera = bakeCamera;
            this.worldData = world;
            this.chunkCenter = chunkCenter;
            this.displays = ChunkPartitioner.GetLandform(chunkCenter);

            PrepareScene();
            BakeDiffuse();
            BakeHeight();

            var diffuseMap = bakeCamera.GetDiffuseTexture(DiffuseRT);
            var heightMap = bakeCamera.GetHeightTexture(HeightRT);
            textures.SetDiffuseMap(diffuseMap);
            textures.SetHeightMap(heightMap);

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
                MapNode node;
                if (worldMap.TryGetValue(display, out node))
                {
                    float angle = node.Landform.Angle;
                    OLandformInfo info;
                    int landformType = node.Landform.LandformType;
                    if (landformInfos.TryGetValue(landformType, out info))
                    {
                        LandformResource resource = info.Terrain;
                        var mesh = CreateMeshRenderer(display, angle, -sceneObjects.Count);
                        sceneObjects.Add(new Pack(resource, mesh));
                    }
                    else
                    {
                        Debug.LogWarning("找不到对应的地形资源!ID:" + landformType);
                    }
                }
            }
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
            bakeCamera.CameraRender(DiffuseRT, chunkCenter, LandformSettings.BlackTransparent);
        }

        void SetDiffuserMaterial(Pack renderer)
        {
            LandformResource res = renderer.Res;

            var material = renderer.Rednerer.sharedMaterial;
            GameObject.Destroy(material);
            material = renderer.Rednerer.sharedMaterial = new Material(diffuseShader);

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
            LandformResource res = renderer.Res;

            var material = renderer.Rednerer.sharedMaterial;
            GameObject.Destroy(material);
            material = renderer.Rednerer.sharedMaterial = new Material(heightShader);

            material.SetTexture("_MainTex", res.HeightTex);
            material.SetTexture("_BlendTex", res.HeightBlendTex);
        }

        struct Pack
        {
            public Pack(LandformResource res, MeshRenderer rednerer)
            {
                this.Res = res;
                this.Rednerer = rednerer;
            }

            public LandformResource Res { get; private set; }
            public MeshRenderer Rednerer { get; private set; }
        }
    }
}
