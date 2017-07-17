using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地图边界效果,应该挂载到显示相机的子节点下;
    /// </summary>
    [RequireComponent(typeof(Camera))]
    [DisallowMultipleComponent]
    public class BoundaryEffect : MonoBehaviour
    {
        BoundaryEffect()
        {
        }

        [SerializeField]
        Camera boundaryEffectCamera;

        Camera mainCamera;
        RenderTexture stencilMap;
        public LayerMask cullingMask;

        void Awake()
        {
            
        }

        void OnPreRender()
        {
            mainCamera = mainCamera ?? GetComponent<Camera>();
            stencilMap = RenderTexture.GetTemporary(Screen.width, Screen.height, 0);

            boundaryEffectCamera.fieldOfView = mainCamera.fieldOfView;
            boundaryEffectCamera.orthographic = mainCamera.orthographic;
            boundaryEffectCamera.nearClipPlane = mainCamera.nearClipPlane;
            boundaryEffectCamera.farClipPlane = mainCamera.farClipPlane;
            boundaryEffectCamera.cullingMask = cullingMask;
            boundaryEffectCamera.targetTexture = stencilMap;
            boundaryEffectCamera.Render();
        }
    }
}
