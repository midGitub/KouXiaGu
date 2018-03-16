using JiongXiaGu.Grids;
using JiongXiaGu.Unity.Maps;
using System;
using System.Collections;
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
        [SerializeField]
        private Camera bakeCamera;
        [SerializeField]
        private LandformQuality quality;
        [SerializeField]
        private LandformBakeDrawingBoardCollection landformBoardCollection;

        public LandformQuality Quality => quality;

        private void Awake()
        {
            InitBakingCamera();
        }

        /// <summary>
        /// 初始化烘焙相机参数;
        /// </summary>
        [ContextMenu("初始化")]
        private void InitBakingCamera()
        {
            bakeCamera.aspect = LandformQuality.CameraAspect;
            bakeCamera.orthographicSize = LandformQuality.CameraSize;
            bakeCamera.transform.rotation = LandformQuality.CameraRotation;
            bakeCamera.clearFlags = CameraClearFlags.SolidColor;  //必须设置为纯色,否则摄像机渲染贴图会有(残图?);
            bakeCamera.backgroundColor = Color.black;
        }


        public Task<LandformBakeResult> BakeAsync(LandformBakeRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 烘培,仅在Unity线程调用;
        /// </summary>
        public IEnumerator BakeCoroutine()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 烘培,仅在Unity线程调用;
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
        /// 获取到临时烘焙漫反射贴图的 "RenderTexture";
        /// </summary>
        public RenderTexture GetDiffuseTemporaryRender()
        {
            BakeTextureInfo texInfo = quality.LandformDiffuseMap;
            return RenderTexture.GetTemporary(texInfo.BakeWidth, texInfo.BakeHeight);
        }

        /// <summary>
        /// 获取到临时烘焙高度贴图的 "RenderTexture";
        /// </summary>
        public RenderTexture GetHeightTemporaryRender()
        {
            BakeTextureInfo texInfo = quality.LandformHeightMap;
            return RenderTexture.GetTemporary(texInfo.BakeWidth, texInfo.BakeHeight);
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
            bakeCamera.transform.position = cameraPos;
            bakeCamera.targetTexture = rt;
            bakeCamera.Render();
            bakeCamera.targetTexture = null;
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

        public static LandformBakeRequest CreateRequest(RectCoord chunkCoord, Map<RectCoord> map)
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

            private Lazy<List<LandformBakeDrawingBoardRenderer>> drawingBoardList;
            public List<LandformBakeDrawingBoardRenderer> DrawingBoardList => drawingBoardList.Value;

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

            public LandformBakeDrawingBoardCollection()
            {
                drawingBoardList = new Lazy<List<LandformBakeDrawingBoardRenderer>>(delegate ()
                {
                    int bakeDrawingBoardCount = BakeDrawingBoardCount;
                    var list = new List<LandformBakeDrawingBoardRenderer>(bakeDrawingBoardCount);
                    for (int i = 0; i < bakeDrawingBoardCount; i++)
                    {
                        var board = Instantiate();
                        list.Add(board);
                    }
                    return list;
                });
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
