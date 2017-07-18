using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;

namespace KouXiaGu.Terrain3D.MapEditor
{

    /// <summary>
    /// 地图边界效果,应该挂载到显示相机的子节点下;
    /// </summary>
    [RequireComponent(typeof(Camera))]
    [DisallowMultipleComponent]
    public class SelectNodeCameraEffect : MonoBehaviour
    {
        SelectNodeCameraEffect()
        {
        }

        [CustomUnityLayer("用于显示选中的区域!")]
        public const string stencilLayer = "SelectMapNode";

        Camera mainCamera;
        public Camera stencilCamera;
        public Shader edgePreserveShader;
        [Range(1, 40)]
        public float outLineWidth = 10;
        [Range(0, 1)]
        public float transparency = 0;
        Material edgePreserveMaterial;
        RenderTexture stencilMap;

        void Awake()
        {
            mainCamera = GetComponent<Camera>();
            stencilCamera.cullingMask = LayerMask.GetMask(stencilLayer);
            edgePreserveMaterial = new Material(edgePreserveShader);
            edgePreserveMaterial.hideFlags = HideFlags.HideAndDontSave;
        }

        void OnPreRender()
        {
            stencilMap = RenderTexture.GetTemporary(Screen.width, Screen.height, 0);
            stencilCamera.fieldOfView = mainCamera.fieldOfView;
            stencilCamera.orthographic = mainCamera.orthographic;
            stencilCamera.nearClipPlane = mainCamera.nearClipPlane;
            stencilCamera.farClipPlane = mainCamera.farClipPlane;
            stencilCamera.targetTexture = stencilMap;
            stencilCamera.Render();
            stencilCamera.targetTexture = null;
        }

        void OnPostRender()
        {
            RenderTexture.ReleaseTemporary(stencilMap);
        }

        void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
        {
            edgePreserveMaterial.SetTexture("_StencilMap", stencilMap);
            edgePreserveMaterial.SetFloat("_OutLineWidth", outLineWidth);
            edgePreserveMaterial.SetFloat("_Transparency", transparency);
            Graphics.Blit(sourceTexture, destTexture, edgePreserveMaterial);
        }
    }
}
