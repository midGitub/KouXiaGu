using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 用于烘焙场景和品质控制;
    /// </summary>
    [RequireComponent(typeof(Camera)), DisallowMultipleComponent]
    public class BakeCamera : SceneSington<BakeCamera>
    {

        BakeCamera()
        {
        }

        /// <summary>
        /// 烘焙时的边框比例(需要裁剪的部分比例);
        /// </summary>
        static readonly float OutlineScale = 1f / 12f;

        /// <summary>
        /// 完整预览整个地图块的摄像机旋转角度;
        /// </summary>
        static readonly Quaternion CameraRotation = Quaternion.Euler(90, 0, 0);

        /// <summary>
        /// 完整预览整个地图块的摄像机大小;
        /// </summary>
        static readonly float CameraSize =
            ((TerrainChunk.CHUNK_HEIGHT + (TerrainChunk.CHUNK_HEIGHT * OutlineScale)) / 2);

        /// <summary>
        /// 完整预览整个地图块的摄像机比例(W/H);
        /// </summary>
        static readonly float CameraAspect =
            (TerrainChunk.CHUNK_WIDTH + TerrainChunk.CHUNK_WIDTH * OutlineScale) /
            (TerrainChunk.CHUNK_HEIGHT + TerrainChunk.CHUNK_HEIGHT * OutlineScale);

        /// <summary>
        /// 烘培使用的相机;
        /// </summary>
        static Camera Camera
        {
            get { return GetInstance._camera; }
        }


        /// <summary>
        /// 获取到临时烘焙漫反射贴图的 "RenderTexture";
        /// </summary>
        public static RenderTexture GetDiffuseTemporaryRender()
        {
            return RenderTexture.GetTemporary(rDiffuseTexWidth, rDiffuseTexHeight);
        }

        /// <summary>
        /// 获取到临时烘焙高度贴图的 "RenderTexture";
        /// </summary>
        public static RenderTexture GetHeightTemporaryRender()
        {
            return RenderTexture.GetTemporary(rHeightMapWidth, rHeightMapHeight);
        }

        /// <summary>
        /// 释放临时的 "RenderTexture";
        /// </summary>
        public static void ReleaseTemporary(RenderTexture rt)
        {
            RenderTexture.ReleaseTemporary(rt);
        }


        /// <summary>
        /// 使用摄像机指定背景颜色烘焙;
        /// </summary>
        public static void CameraRender(RenderTexture rt, CubicHexCoord cameraPoint, Color backgroundColor)
        {
            Color current = Camera.backgroundColor;
            Camera.backgroundColor = backgroundColor;

            CameraRender(rt, cameraPoint);

            Camera.backgroundColor = current;
        }

        /// <summary>
        /// 使用摄像机烘焙;
        /// </summary>
        public static void CameraRender(RenderTexture rt, CubicHexCoord cameraPoint)
        {
            Vector3 cameraPixelPoint = cameraPoint.GetTerrainPixel(5f);
            CameraRender(rt, cameraPixelPoint);
        }

        /// <summary>
        /// 使用摄像机指定背景颜色烘焙;
        /// </summary>
        public static void CameraRender(RenderTexture rt, Vector3 cameraPoint, Color backgroundColor)
        {
            Color current = Camera.backgroundColor;
            Camera.backgroundColor = backgroundColor;

            CameraRender(rt, cameraPoint);

            Camera.backgroundColor = current;
        }

        /// <summary>
        /// 使用摄像机烘焙;
        /// </summary>
        public static void CameraRender(RenderTexture rt, Vector3 cameraPoint)
        {
            GetInstance.transform.position = cameraPoint;
            CameraRender(rt);
        }

        /// <summary>
        /// 使用摄像机指定背景颜色烘焙;
        /// </summary>
        public static void CameraRender(RenderTexture rt, Color backgroundColor)
        {
            Color current = Camera.backgroundColor;
            Camera.backgroundColor = backgroundColor;

            CameraRender(rt);

            Camera.backgroundColor = current;
        }

        /// <summary>
        /// 使用摄像机烘焙;
        /// </summary>
        public static void CameraRender(RenderTexture rt)
        {
            Camera.targetTexture = rt;
            Camera.Render();
            Camera.targetTexture = null;
        }


        public static Texture2D GetDiffuseTexture(RenderTexture rt)
        {
            RenderTexture.active = rt;
            Texture2D diffuseTex = new Texture2D((int)BakeCamera.DiffuseTexWidth, (int)BakeCamera.DiffuseTexHeight, TextureFormat.RGB24, false);
            diffuseTex.ReadPixels(BakeCamera.DiffuseReadPixel, 0, 0, false);
            diffuseTex.wrapMode = TextureWrapMode.Clamp;
            diffuseTex.Apply();
            return diffuseTex;
        }

        public static Texture2D GetHeightTexture(RenderTexture rt)
        {
            RenderTexture.active = rt;
            Texture2D heightMap = new Texture2D((int)BakeCamera.HeightMapWidth, (int)BakeCamera.HeightMapHeight, TextureFormat.RGB24, false);
            heightMap.ReadPixels(BakeCamera.HeightReadPixel, 0, 0, false);
            heightMap.wrapMode = TextureWrapMode.Clamp;
            heightMap.Apply();
            return heightMap;
        }



        [SerializeField, Range(80, 500)]
        float textureSize = 120;

        [SerializeField, Range(0.1f, 1)]
        float diffuseMapRatios = 1;

        [SerializeField, Range(0.1f, 1)]
        float heightMapRatios = 1;

        /// <summary>
        /// 图片裁剪后的尺寸;
        /// </summary>
        public static int DiffuseTexWidth { get; private set; }
        public static int DiffuseTexHeight { get; private set; }
        public static int HeightMapWidth { get; private set; }
        public static int HeightMapHeight { get; private set; }

        /// <summary>
        /// 烘焙时的尺寸;
        /// </summary>
        public static int rDiffuseTexWidth { get; private set; }
        public static int rDiffuseTexHeight { get; private set; }
        public static int rHeightMapWidth { get; private set; }
        public static int rHeightMapHeight { get; private set; }

        /// <summary>
        /// 裁剪区域;
        /// </summary>
        public static Rect DiffuseReadPixel { get; private set; }
        public static Rect HeightReadPixel { get; private set; }

        /// <summary>
        /// 烘培摄像机;
        /// </summary>
        Camera _camera;

        /// <summary>
        /// 贴图大小 推荐 80 ~ 500;
        /// </summary>
        public float TextureSize
        {
            get { return textureSize; }
            set { textureSize = value; UpdataTextureSize(); }
        }

        /// <summary>
        /// 贴图分辨率百分比 0.1~1
        /// </summary>
        public float DiffuseMapRatios
        {
            get { return diffuseMapRatios; }
            set { diffuseMapRatios = value; UpdataTextureSize(); }
        }

        /// <summary>
        /// 贴图分辨率百分比 0.1~1
        /// </summary>
        public float HeightMapRatios
        {
            get { return heightMapRatios; }
            set { heightMapRatios = value; UpdataTextureSize(); }
        }


        protected override void Awake()
        {
            base.Awake();
            _camera = GetComponent<Camera>();
            InitBakingCamera();
        }

        void OnValidate()
        {
            UpdataTextureSize();
        }


        /// <summary>
        /// 初始化烘焙相机参数;
        /// </summary>
        [ContextMenu("初始化相机")]
        void InitBakingCamera()
        {
            _camera.aspect = CameraAspect;
            _camera.orthographicSize = CameraSize;
            _camera.transform.rotation = CameraRotation;
            _camera.clearFlags = CameraClearFlags.SolidColor;  //必须设置为纯色,否则摄像机渲染贴图会有(残图?);
            _camera.backgroundColor = Color.black;
        }

        void UpdataTextureSize()
        {
            float chunkWidth = TerrainChunk.CHUNK_WIDTH * textureSize;
            float chunkHeight = TerrainChunk.CHUNK_HEIGHT * textureSize;

            DiffuseTexWidth = (int)Math.Round(chunkWidth * diffuseMapRatios);
            DiffuseTexHeight = (int)Math.Round(chunkHeight * diffuseMapRatios);
            HeightMapWidth = (int)Math.Round(chunkWidth * heightMapRatios);
            HeightMapHeight = (int)Math.Round(chunkHeight * heightMapRatios);

            rDiffuseTexWidth = (int)Math.Round(DiffuseTexWidth + DiffuseTexWidth * OutlineScale);
            rDiffuseTexHeight = (int)Math.Round(DiffuseTexHeight + DiffuseTexHeight * OutlineScale);
            rHeightMapWidth = (int)Math.Round(HeightMapWidth + HeightMapWidth * OutlineScale);
            rHeightMapHeight = (int)Math.Round(HeightMapHeight + HeightMapHeight * OutlineScale);

            DiffuseReadPixel =
                new Rect(
                    DiffuseTexWidth * OutlineScale / 2,
                    DiffuseTexHeight * OutlineScale / 2,
                    DiffuseTexWidth,
                    DiffuseTexHeight);

            HeightReadPixel =
                new Rect(
                    HeightMapWidth * OutlineScale / 2,
                    HeightMapHeight * OutlineScale / 2,
                    HeightMapWidth,
                    HeightMapHeight);
        }


    }

}
