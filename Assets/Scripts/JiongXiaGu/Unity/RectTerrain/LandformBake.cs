using JiongXiaGu.Grids;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{

    /// <summary>
    /// 地形烘培;
    /// </summary>
    public sealed class LandformBake
    {
        /// <summary>
        /// 相机高度;
        /// </summary>
        public static readonly Vector3 CameraPosition = new Vector3(0, 5, 0);

        /// <summary>
        /// 完整预览整个地图块的摄像机旋转角度;
        /// </summary>
        public static readonly Quaternion CameraRotation = Quaternion.Euler(90, 0, 0);

        internal static readonly float BakeMeshWidth = ChunkInfo.Width * 1.5f;
        internal static readonly float BakeMeshHeight = ChunkInfo.Height * 1.5f;

        /// <summary>
        /// 完整预览整个地图块的摄像机大小;
        /// </summary>
        public static readonly float CameraSize = ChunkInfo.Width;

        /// <summary>
        /// 完整预览整个地图块的摄像机比例(W/H);
        /// </summary>
        public static readonly float CameraAspect = ChunkInfo.Width / ChunkInfo.Height;

        private Transform root;
        private Camera bakeCamera;
        private BakeDrawingBoard bakeDrawingBoardPrefab;
        private List<BakeNode> bakeNodes;

        public LandformBake(Transform root)
        {
            this.root = root;
        }

        public BakeResult Bake(IBakeContent content, Quality quality)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            var coroutine = BakeInCoroutine(content, quality, CancellationToken.None);
            while (coroutine.MoveNext())
            {
            }
            return coroutine.Current;
        }

        public Task<BakeResult> BakeAsync(IBakeContent content, Quality quality, CancellationToken token)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            throw new NotImplementedException();
        }

        private IEnumerator<BakeResult> BakeInCoroutine(IBakeContent content, Quality quality, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            RenderTexture diffuseRT = RenderTexture.GetTemporary(quality.DiffuseMapWidth, quality.DiffuseMapHeight);
            DisplayDiffuse();
            CameraRender(diffuseRT);
            var diffuseMap = CreateTexture(diffuseRT, quality.DiffuseMapWidth, quality.DiffuseMapHeight);
            RenderTexture.ReleaseTemporary(diffuseRT);

            yield return default(BakeResult);
            if (token.IsCancellationRequested)
            {
                GameObject.Destroy(diffuseMap);
                throw new OperationCanceledException();
            }

            RenderTexture heightRT = RenderTexture.GetTemporary(quality.HeightMapWidth, quality.HeightMapHeight);
            DisplayHeight();
            CameraRender(heightRT);
            var heightMap = CreateTexture(heightRT, quality.HeightMapWidth, quality.HeightMapHeight);
            RenderTexture.ReleaseTemporary(heightRT);

            yield return default(BakeResult);
            if (token.IsCancellationRequested)
            {
                GameObject.Destroy(diffuseMap);
                GameObject.Destroy(heightMap);
                throw new OperationCanceledException();
            }

            BakeResult result = new BakeResult()
            {
                DiffuseMap = diffuseMap,
                HeightMap = heightMap,
            };
            yield return result;
        }

        private void PrepareScene(IBakeContent content)
        {
            if (bakeNodes == null)
            {
                bakeNodes = GetBakeNodes();
            }

            foreach (var bakeNode in bakeNodes)
            {
                var coord = content.Target + bakeNode.Offset;

                var info = content.GetInfo(coord);
                bakeNode.DrawingBoard.transform.rotation = Quaternion.Euler(0, info.Angle, 0);
                bakeNode.LandformTextures = info.Textures;
            }
        }

        private List<BakeNode> GetBakeNodes()
        {
            List<BakeNode> list = new List<BakeNode>()
            {
                CreateBakeNodeAt(new RectCoord(-1, 1)),
                CreateBakeNodeAt(new RectCoord(0, 1)),
                CreateBakeNodeAt(new RectCoord(1, 1)),

                CreateBakeNodeAt(new RectCoord(-1, 0)),
                CreateBakeNodeAt(new RectCoord(0, 0)),
                CreateBakeNodeAt(new RectCoord(1, 0)),

                CreateBakeNodeAt(new RectCoord(-1, -1)),
                CreateBakeNodeAt(new RectCoord(0, -1)),
                CreateBakeNodeAt(new RectCoord(1, -1)),
            };
            return list;
        }

        private BakeNode CreateBakeNodeAt(RectCoord offset)
        {
            BakeDrawingBoard drawingBoard = CreateGameObject<BakeDrawingBoard>("LandformBakeDrawingBoard");
            drawingBoard.transform.position = ChunkInfo.ToLandformPixel(offset);
            return new BakeNode(offset, drawingBoard);
        }

        private void DisplayDiffuse()
        {
            foreach (var bakeNode in bakeNodes)
            {
                bakeNode.DrawingBoard.Renderer.SetDiffuseMap(bakeNode.LandformTextures.DiffuseTex);
                bakeNode.DrawingBoard.Renderer.SetBlendMap(bakeNode.LandformTextures.DiffuseBlendTex);
            }
        }

        private void DisplayHeight()
        {
            foreach (var bakeNode in bakeNodes)
            {
                bakeNode.DrawingBoard.Renderer.SetDiffuseMap(bakeNode.LandformTextures.HeightTex);
                bakeNode.DrawingBoard.Renderer.SetBlendMap(bakeNode.LandformTextures.HeightBlendTex);
            }
        }

        private void CameraRender(RenderTexture rt)
        {
            if (bakeCamera == null)
            {
                bakeCamera = CreateGameObject<Camera>("LandformBakeCamera");

                bakeCamera.transform.position = CameraPosition;
                bakeCamera.transform.rotation = CameraRotation;
                bakeCamera.aspect = CameraAspect;
                bakeCamera.orthographicSize = CameraSize;
                bakeCamera.clearFlags = CameraClearFlags.SolidColor;  //必须设置为纯色,否则摄像机渲染贴图会有(残图?);
                bakeCamera.backgroundColor = Color.black;
            }

            bakeCamera.targetTexture = rt;
            bakeCamera.Render();
            bakeCamera.targetTexture = null;
        }

        private Texture2D CreateTexture(RenderTexture rt, int width, int height, TextureFormat format = TextureFormat.RGB24, bool mipmap = false)
        {
            var original = RenderTexture.active;
            RenderTexture.active = rt;
            Texture2D diffuseTex = new Texture2D(width, height, format, mipmap);
            RenderTexture.active = original;
            return diffuseTex;
        }

        private T CreateGameObject<T>(string name)
            where T : Behaviour
        {
            T item = new GameObject(name, typeof(T)).GetComponent<T>();

            if (root != null)
            {
                item.transform.SetParent(root, false);
            }

            return item;
        }

        private class BakeNode
        {
            public RectCoord Offset { get; private set; }
            public BakeDrawingBoard DrawingBoard { get; private set; }
            public LandformTextures LandformTextures { get; set; }

            public BakeNode(RectCoord offset, BakeDrawingBoard drawingBoard)
            {
                if (drawingBoard == null)
                    throw new ArgumentNullException(nameof(drawingBoard));

                Offset = offset;
                DrawingBoard = drawingBoard;
            }
        }
    }
}
