using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.RectTerrain
{

    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    public class LandformBakeCamera : MonoBehaviour
    {
        LandformBakeCamera()
        {
        }

        [SerializeField]
        LandformQuality quality;
        Camera _camera;

        /// <summary>
        /// 烘培质量,在运行时切勿修改参数;
        /// </summary>
        public LandformQuality Quality
        {
            get { return quality; }
        }

        void Awake()
        {
            _camera = GetComponent<Camera>();
            quality.Updata();
            InitBakingCamera();
        }

        /// <summary>
        /// 初始化烘焙相机参数;
        /// </summary>
        [ContextMenu("初始化")]
        void InitBakingCamera()
        {
            _camera.aspect = LandformQuality.CameraAspect;
            _camera.orthographicSize = LandformQuality.CameraSize;
            _camera.transform.rotation = LandformQuality.CameraRotation;
            _camera.clearFlags = CameraClearFlags.SolidColor;  //必须设置为纯色,否则摄像机渲染贴图会有(残图?);
            _camera.backgroundColor = Color.black;
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
        /// 使用摄像机烘焙;
        /// </summary>
        public void CameraRender(Vector3 cameraPoint, RenderTexture rt)
        {
            _camera.transform.position = cameraPoint;
            CameraRender(rt);
        }

        /// <summary>
        /// 使用摄像机烘焙;
        /// </summary>
        void CameraRender(RenderTexture rt)
        {
            _camera.targetTexture = rt;
            _camera.Render();
            _camera.targetTexture = null;
        }
    }
}
