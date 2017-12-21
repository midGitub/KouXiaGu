using JiongXiaGu.Grids;
using JiongXiaGu.Unity.RectMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{

    /// <summary>
    /// 地形烘培;
    /// </summary>
    [Serializable]
    public sealed class LandformBaker : MonoBehaviour
    {
        [SerializeField, Range(0, 64)]
        float tessellation = 48f;
        [SerializeField, Range(0, 5)]
        float displacement = 1.5f;
        [SerializeField]
        LandformBakeCamera bakeCamera;
        [SerializeField]
        LandformBakeDrawingBoardCollection landformBoardCollection;

        public LandformBakeCamera BakeCamera
        {
            get { return bakeCamera; }
        }

        public LandformQuality Quality
        {
            get { return bakeCamera.Quality; }
        }

        /// <summary>
        /// 地形细分程度;
        /// </summary>
        public float Tessellation
        {
            get { return tessellation; }
        }

        /// <summary>
        /// 地形高度缩放;
        /// </summary>
        public float Displacement
        {
            get { return displacement; }
        }

        private void Awake()
        {
            landformBoardCollection.Initialize();
            OnValidate();
        }

        private void OnValidate()
        {
            LandformChunkRenderer.SetTessellation(tessellation);
            LandformChunkRenderer.SetDisplacement(displacement);
        }

        /// <summary>
        /// 对地图进行烘培,仅在Unity线程调用;
        /// </summary>
        public LandformBakeResult Bake(LandformBakeRequest request)
        {
            BakeTextureInfo landformDiffuseInfo = Quality.LandformDiffuseMap;
            RenderTexture diffuseRT = RenderTexture.GetTemporary(landformDiffuseInfo.BakeWidth, landformDiffuseInfo.BakeHeight);

            BakeTextureInfo landformHeightInfo = Quality.LandformHeightMap;
            RenderTexture heightRT = RenderTexture.GetTemporary(landformHeightInfo.BakeWidth, landformHeightInfo.BakeHeight);

            PrepareBakeScene(request.ChunkCoord, request.BakePoints);
            CameraRender(request.ChunkCoord, diffuseRT);
            BoardsDisplayHeight(request.BakePoints);
            CameraRender(request.ChunkCoord, heightRT);

            LandformBakeResult result = new LandformBakeResult()
            {
                DiffuseMap = GetDiffuseMap(diffuseRT),
                HeightMap = GetHeightMap(heightRT),
            };

            RenderTexture.ReleaseTemporary(diffuseRT);
            RenderTexture.ReleaseTemporary(heightRT);
            return result;
        }

        /// <summary>
        /// 准备烘培场景;
        /// </summary>
        private void PrepareBakeScene(RectCoord chunkCoord, List<LandformBakeNode> bakePoints)
        {
            var boardList = landformBoardCollection.DrawingBoardList;
            for (int i = 0; i < bakePoints.Count; i++)
            {
                var board = boardList[i];
                var bakePoint = bakePoints[i];

                var drawingBoardPoint = landformBoardCollection.GetDrawingBoardPoint(chunkCoord, bakePoint.Position).ChangedY(-i);
                var rotation = Quaternion.Euler(0, bakePoint.Node.Angle, 0);
                board.SetValue(drawingBoardPoint, rotation);
                board.DisplayDiffuse(bakePoint.Res);
            }
        }

        private void BoardsDisplayHeight(List<LandformBakeNode> bakePoints)
        {
            var boardList = landformBoardCollection.DrawingBoardList;
            for (int i = 0; i < bakePoints.Count; i++)
            {
                var board = boardList[i];
                var bakePoint = bakePoints[i];
                board.DisplayHeight(bakePoint.Res);
            }
        }

        private void CameraRender(RectCoord chunkPos, RenderTexture rt)
        {
            Vector3 cameraPos = landformBoardCollection.Center.ChangedY(5);
            bakeCamera.CameraRender(cameraPos, rt);
        }

        private Texture2D GetDiffuseMap(RenderTexture rt, TextureFormat format = TextureFormat.RGB24, bool mipmap = false)
        {
            BakeTextureInfo texInfo = Quality.LandformDiffuseMap;
            RenderTexture.active = rt;
            Texture2D diffuseTex = new Texture2D(texInfo.Width, texInfo.Height, format, mipmap);
            diffuseTex.ReadPixels(texInfo.ClippingRect, 0, 0, false);
            diffuseTex.wrapMode = TextureWrapMode.Clamp;
            diffuseTex.Apply();
            return diffuseTex;
        }

        private Texture2D GetHeightMap(RenderTexture rt, TextureFormat format = TextureFormat.RGB24, bool mipmap = false)
        {
            BakeTextureInfo texInfo = Quality.LandformHeightMap;
            RenderTexture.active = rt;
            Texture2D diffuseTex = new Texture2D(texInfo.Width, texInfo.Height, format, mipmap);
            diffuseTex.ReadPixels(texInfo.ClippingRect, 0, 0, false);
            diffuseTex.wrapMode = TextureWrapMode.Clamp;
            diffuseTex.Apply();
            return diffuseTex;
        }

        private static readonly RectRange landformBakeRange = new RectRange(LandformChunkInfo.ChunkRange.Height + 1, LandformChunkInfo.ChunkRange.Width + 1);

        private static readonly RectCoord[] landformBakePointOffsets = landformBakeRange.Range().ToArray();

        /// <summary>
        /// 获取到烘培的节点;
        /// </summary>
        public static IEnumerable<RectCoord> GetBakePoints(RectCoord chunkPos)
        {
            RectCoord chunkCenter = chunkPos.ToLandformChunkCenter();
            foreach (var offset in landformBakePointOffsets)
            {
                yield return offset + chunkCenter;
            }
        }

        public static LandformBakeRequest CreateRequest(RectCoord chunkCoord, WorldMap map, LandformResCreater resPool)
        {
            throw new NotImplementedException();
        }


        [Serializable]
        private class LandformBakeDrawingBoardCollection
        {
            [SerializeField]
            private LandformBakeDrawingBoardRenderer prefab;
            [SerializeField]
            private Transform objectParent;
            [SerializeField]
            private Vector3 center;

            public List<LandformBakeDrawingBoardRenderer> DrawingBoardList { get; private set; }

            /// <summary>
            /// 将显示的坐标转换到的中心点;
            /// </summary>
            public Vector3 Center
            {
                get { return center; }
            }

            public int BakeDrawingBoardCount
            {
                get { return landformBakeRange.NodeCount; }
            }

            /// <summary>
            /// 初始化合集内容;
            /// </summary>
            public virtual void Initialize()
            {
                int bakeDrawingBoardCount = BakeDrawingBoardCount;
                DrawingBoardList = new List<LandformBakeDrawingBoardRenderer>(bakeDrawingBoardCount);
                for (int i = 0; i < bakeDrawingBoardCount; i++)
                {
                    var board = Instantiate();
                    DrawingBoardList.Add(board);
                }
            }

            /// <summary>
            /// 获取到烘培面板的位置;
            /// </summary>
            public Vector3 GetDrawingBoardPoint(RectCoord chunkPos, RectCoord bakePoint)
            {
                Vector3 chunkCenter = chunkPos.ToLandformChunkPixel();
                Vector3 bakePixel = bakePoint.ToRectTerrainPixel();
                return center + (bakePixel - chunkCenter);
            }

            private LandformBakeDrawingBoardRenderer Instantiate()
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

            private void Destroy(LandformBakeDrawingBoardRenderer item)
            {
                GameObject.Destroy(item.gameObject);
            }
        }
    }
}
