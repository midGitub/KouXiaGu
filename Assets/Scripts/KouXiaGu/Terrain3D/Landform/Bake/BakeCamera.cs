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
    [Serializable]
    class BakeCamera
    {
        BakeCamera()
        {
        }

        [SerializeField]
        Camera camera;

        public Camera Camera
        {
            get { return camera; }
        }

        public QualitySettings Settings
        {
            get { return LandformSettings.Instance.BakeSettings; }
        }

        public void Initialize()
        {
            InitBakingCamera();
        }

        /// <summary>
        /// 初始化烘焙相机参数;
        /// </summary>
        void InitBakingCamera()
        {
            camera.aspect = QualitySettings.CameraAspect;
            camera.orthographicSize = QualitySettings.CameraSize;
            camera.transform.rotation = QualitySettings.CameraRotation;
            camera.clearFlags = CameraClearFlags.SolidColor;  //必须设置为纯色,否则摄像机渲染贴图会有(残图?);
            camera.backgroundColor = Color.black;
        }

        /// <summary>
        /// 获取到临时烘焙漫反射贴图的 "RenderTexture";
        /// </summary>
        public RenderTexture GetDiffuseTemporaryRender()
        {
            BakeTextureInfo texInfo = Settings.LandformDiffuseMap;
            return RenderTexture.GetTemporary(texInfo.BakeWidth, texInfo.BakeHeight);
        }

        /// <summary>
        /// 获取到临时烘焙高度贴图的 "RenderTexture";
        /// </summary>
        public RenderTexture GetHeightTemporaryRender()
        {
            BakeTextureInfo texInfo = Settings.LandformHeightMap;
            return RenderTexture.GetTemporary(texInfo.BakeWidth, texInfo.BakeHeight);
        }

        /// <summary>
        /// 释放临时的 "RenderTexture";
        /// </summary>
        public void ReleaseTemporary(RenderTexture rt)
        {
            RenderTexture.ReleaseTemporary(rt);
        }


        /// <summary>
        /// 使用摄像机指定背景颜色烘焙;
        /// </summary>
        public void CameraRender(RenderTexture rt, CubicHexCoord cameraPoint, Color backgroundColor)
        {
            Color current = Camera.backgroundColor;
            Camera.backgroundColor = backgroundColor;
            CameraRender(rt, cameraPoint);
            Camera.backgroundColor = current;
        }

        /// <summary>
        /// 使用摄像机烘焙;
        /// </summary>
        public void CameraRender(RenderTexture rt, CubicHexCoord cameraPoint)
        {
            Vector3 cameraPixelPoint = cameraPoint.GetTerrainPixel(5f);
            CameraRender(rt, cameraPixelPoint);
        }

        /// <summary>
        /// 使用摄像机指定背景颜色烘焙;
        /// </summary>
        public void CameraRender(RenderTexture rt, Vector3 cameraPoint, Color backgroundColor)
        {
            Color current = Camera.backgroundColor;
            Camera.backgroundColor = backgroundColor;

            CameraRender(rt, cameraPoint);

            Camera.backgroundColor = current;
        }

        /// <summary>
        /// 使用摄像机烘焙;
        /// </summary>
        public void CameraRender(RenderTexture rt, Vector3 cameraPoint)
        {
            Camera.transform.position = cameraPoint;
            CameraRender(rt);
        }

        /// <summary>
        /// 使用摄像机指定背景颜色烘焙;
        /// </summary>
        public void CameraRender(RenderTexture rt, Color backgroundColor)
        {
            Color current = Camera.backgroundColor;
            Camera.backgroundColor = backgroundColor;

            CameraRender(rt);

            Camera.backgroundColor = current;
        }

        /// <summary>
        /// 使用摄像机烘焙;
        /// </summary>
        public void CameraRender(RenderTexture rt)
        {
            Camera.targetTexture = rt;
            Camera.Render();
            Camera.targetTexture = null;
        }


        public Texture2D GetDiffuseTexture(
            RenderTexture rt,
            TextureFormat format = TextureFormat.RGB24,
            bool mipmap = false)
        {
            BakeTextureInfo texInfo = Settings.LandformDiffuseMap;
            RenderTexture.active = rt;
            Texture2D diffuseTex = new Texture2D(texInfo.Width, texInfo.Height, format, mipmap);
            diffuseTex.ReadPixels(texInfo.ClippingRect, 0, 0, false);
            diffuseTex.wrapMode = TextureWrapMode.Clamp;
            diffuseTex.Apply();
            return diffuseTex;
        }

        public Texture2D GetHeightTexture(
            RenderTexture rt,
            TextureFormat format = TextureFormat.RGB24,
            bool mipmap = false)
        {
            BakeTextureInfo texInfo = Settings.LandformHeightMap;
            RenderTexture.active = rt;
            Texture2D heightMap = new Texture2D(texInfo.Width, texInfo.Height, format, mipmap);
            heightMap.ReadPixels(texInfo.ClippingRect, 0, 0, false);
            heightMap.wrapMode = TextureWrapMode.Clamp;
            heightMap.Apply();
            return heightMap;
        }

    }

}
