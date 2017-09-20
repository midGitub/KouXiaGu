using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Grids;
using KouXiaGu.World;
using UnityEngine;
using KouXiaGu.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using KouXiaGu.RectTerrain.Resources;
using KouXiaGu.World.RectMap;

namespace KouXiaGu.RectTerrain
{

    /// <summary>
    /// 地形烘培器,在Unity线程进行烘培任务;
    /// </summary>
    [Serializable]
    public sealed class LandformBaker : MonoBehaviour, IComponentInitializer
    {
        LandformBaker()
        {
        }

        [SerializeField]
        RectMapDataInitializer mapInitializer;
        [SerializeField]
        LandformDispatcher dispatcher;
        [SerializeField]
        LandformBakeCamera bakeCamera;
        [SerializeField]
        LandformChunkPool landformChunkPool;
        [SerializeField]
        LandformBakeDrawingBoardCollection landformBoardCollection;

        RectTerrainResources resources;
        IDictionary<RectCoord, MapNode> map;

        /// <summary>
        /// 地形请求处置器;
        /// </summary>
        public IRequestDispatcher RequestDispatcher
        {
            get { return dispatcher; }
        }

        public LandformBakeCamera BakeCamera
        {
            get { return bakeCamera; }
        }

        public LandformQuality Quality
        {
            get { return bakeCamera.Quality; }
        }

        public IObjectPool<LandformChunkRenderer> LandformChunkPool
        {
            get { return landformChunkPool; }
        }

        void Awake()
        {
            landformBoardCollection.Initialize();
        }

        Task IComponentInitializer.StartInitialize(CancellationToken token)
        {
            resources = RectTerrainResourcesInitializer.RectTerrainResources;
            map = mapInitializer.WorldMap.Map;
            return null;
        }

        public void Bake(RectCoord chunkPos, LandformChunkRenderer landformChunk)
        {
            BakeTextureInfo landformDiffuseInfo = Quality.LandformDiffuseMap;
            RenderTexture diffuseRT = RenderTexture.GetTemporary(landformDiffuseInfo.BakeWidth, landformDiffuseInfo.BakeHeight);

            BakeTextureInfo landformHeightInfo = Quality.LandformHeightMap;
            RenderTexture heightRT = RenderTexture.GetTemporary(landformHeightInfo.BakeWidth, landformHeightInfo.BakeHeight);

            PrepareLandformBakeScene(chunkPos);
            BakeLandformDiffuse(chunkPos, diffuseRT);
            BakeLandformHeight(chunkPos, heightRT);

            Texture2D diffuseMap = GetDiffuseMap(diffuseRT);
            Texture2D heightMap = GetHeightMap(heightRT);

            landformChunk.SetDiffuseMap(diffuseMap);
            landformChunk.SetHeightMap(heightMap);

            RenderTexture.ReleaseTemporary(diffuseRT);
            RenderTexture.ReleaseTemporary(heightRT);
        }

        void PrepareLandformBakeScene(RectCoord chunkPos)
        {
            var bakePoints = landformBoardCollection.GetBakePoints(chunkPos);
            var boardList = landformBoardCollection.DrawingBoardList;
            int i = 0;
            foreach (var bakePoint in bakePoints)
            {
                var drawingBoardPoint = landformBoardCollection.GetDrawingBoardPoint(chunkPos, bakePoint).ChangedY(-i);
                var landformInfo = GetLandformNodeInfo(bakePoint);
                var landformResource = GetLandformResource(landformInfo.TypeID);
                Quaternion rotation = Quaternion.Euler(0, landformInfo.Angle, 0);
                LandformBakeDrawingBoardRenderer board = boardList[i++];
                board.Initialize(drawingBoardPoint, rotation, landformResource);
            }
        }

        NodeLandformInfo GetLandformNodeInfo(RectCoord position)
        {
            MapNode node;
            map.TryGetValue(position, out node);
            return node.Landform;
        }

        LandformResource GetLandformResource(int typeID)
        {
            LandformResource res;
            resources.Landform.TryGetValue(typeID, out res);
            return res;
        }

        void BakeLandformDiffuse(RectCoord chunkPos, RenderTexture rt)
        {
            foreach (var drawingBoard in landformBoardCollection.DrawingBoardList)
            {
                drawingBoard.DisplayDiffuse();
            }
            Vector3 cameraPos = landformBoardCollection.Center.ChangedY(5);
            bakeCamera.CameraRender(cameraPos, rt);
        }

        void BakeLandformHeight(RectCoord chunkPos, RenderTexture rt)
        {
            foreach (var drawingBoard in landformBoardCollection.DrawingBoardList)
            {
                drawingBoard.DisplayHeight();
            }
            Vector3 cameraPos = landformBoardCollection.Center.ChangedY(5);
            bakeCamera.CameraRender(cameraPos, rt);
        }

        Texture2D GetDiffuseMap(RenderTexture rt, TextureFormat format = TextureFormat.RGB24, bool mipmap = false)
        {
            BakeTextureInfo texInfo = Quality.LandformDiffuseMap;
            RenderTexture.active = rt;
            Texture2D diffuseTex = new Texture2D(texInfo.Width, texInfo.Height, format, mipmap);
            diffuseTex.ReadPixels(texInfo.ClippingRect, 0, 0, false);
            diffuseTex.wrapMode = TextureWrapMode.Clamp;
            diffuseTex.Apply();
            return diffuseTex;
        }

        Texture2D GetHeightMap(RenderTexture rt, TextureFormat format = TextureFormat.RGB24, bool mipmap = false)
        {
            BakeTextureInfo texInfo = Quality.LandformHeightMap;
            RenderTexture.active = rt;
            Texture2D diffuseTex = new Texture2D(texInfo.Width, texInfo.Height, format, mipmap);
            diffuseTex.ReadPixels(texInfo.ClippingRect, 0, 0, false);
            diffuseTex.wrapMode = TextureWrapMode.Clamp;
            diffuseTex.Apply();
            return diffuseTex;
        }


        abstract class BakeDrawingBoardCollection<T>
            where T : Component
        {

            [SerializeField]
            T prefab;
            [SerializeField]
            Transform objectParent;
            [SerializeField]
            Vector3 center;

            public List<T> DrawingBoardList { get; private set; }
            public abstract int BakeDrawingBoardCount { get; }

            /// <summary>
            /// 将显示的坐标转换到的中心点;
            /// </summary>
            public Vector3 Center
            {
                get { return center; }
            }

            /// <summary>
            /// 初始化合集内容;
            /// </summary>
            public virtual void Initialize()
            {
                int bakeDrawingBoardCount = BakeDrawingBoardCount;
                DrawingBoardList = new List<T>(bakeDrawingBoardCount);
                for (int i = 0; i < bakeDrawingBoardCount; i++)
                {
                    var board = Instantiate();
                    DrawingBoardList.Add(board);
                }
            }

            /// <summary>
            /// 获取到在烘培时显示的坐标;
            /// </summary>
            public abstract IEnumerable<RectCoord> GetBakePoints(RectCoord chunkPos);

            /// <summary>
            /// 获取到烘培面板的位置;
            /// </summary>
            public Vector3 GetDrawingBoardPoint(RectCoord chunkPos, RectCoord bakePoint)
            {
                Vector3 chunkCenter = chunkPos.ToLandformChunkPixel();
                Vector3 bakePixel = bakePoint.ToRectTerrainPixel();
                return center + (bakePixel - chunkCenter);
            }

            T Instantiate()
            {
                if (objectParent == null)
                {
                    return GameObject.Instantiate(prefab);
                }
                else
                {
                    return GameObject.Instantiate(prefab, objectParent);
                }
            }

            void Destroy(T item)
            {
                GameObject.Destroy(item.gameObject);
            }
        }

        [Serializable]
        class LandformBakeDrawingBoardCollection : BakeDrawingBoardCollection<LandformBakeDrawingBoardRenderer>
        {
            static readonly RectRange landformBakeRange = new RectRange(LandformInfo.ChunkRange.Height + 1 , LandformInfo.ChunkRange.Width + 1);

            static readonly RectCoord[] landformBakePointOffsets = landformBakeRange.Range().ToArray();

            public override int BakeDrawingBoardCount
            {
                get { return landformBakeRange.NodeCount; }
            }

            public override IEnumerable<RectCoord> GetBakePoints(RectCoord chunkPos)
            {
                RectCoord chunkCenter = chunkPos.ToLandformChunkCenter();
                foreach (var offset in landformBakePointOffsets)
                {
                    yield return offset + chunkCenter;
                }
            }
        }
    }
}
