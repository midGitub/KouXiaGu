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
        public const string StencilLayer = "SelectMapNode";
        const string StencilCameraName = "SelectNodeStencilCamera";

        Camera mainCamera;
        Camera stencilCamera;
        public Shader edgePreserveShader;
        [Range(1, 40)]
        public float outLineWidth = 10;
        [Range(0, 1)]
        public float transparency = 0;
        public LayerMask cullingMask;
        Material edgePreserveMaterial;
        RenderTexture stencilMap;

        void Awake()
        {
            mainCamera = GetComponent<Camera>();
            stencilCamera = CreateStencilCamera();
            edgePreserveMaterial = new Material(edgePreserveShader);
            edgePreserveMaterial.hideFlags = HideFlags.HideAndDontSave;
        }

        void OnDestroy()
        {
            Destroy(stencilCamera.gameObject);
        }

        Camera CreateStencilCamera()
        {
            var node = new GameObject(StencilCameraName, typeof(Camera)).transform;
            node.parent = transform;
            node.localPosition = Vector3.zero;
            node.localRotation = Quaternion.identity;
            node.localScale = Vector3.one;

            Camera stencilCamera = node.GetComponent<Camera>();
            stencilCamera.enabled = false;
            stencilCamera.clearFlags = CameraClearFlags.SolidColor;
            stencilCamera.backgroundColor = new Color(0, 0, 0, 0);
            stencilCamera.cullingMask = cullingMask;
            stencilCamera.renderingPath = RenderingPath.VertexLit;
            stencilCamera.allowHDR = false;
            stencilCamera.allowMSAA = false;
            stencilCamera.useOcclusionCulling = false;
            return stencilCamera;
        }

        void OnPreRender()
        {
            stencilCamera.fieldOfView = mainCamera.fieldOfView;
            stencilCamera.orthographic = mainCamera.orthographic;
            stencilCamera.nearClipPlane = mainCamera.nearClipPlane;
            stencilCamera.farClipPlane = mainCamera.farClipPlane;

            stencilMap = RenderTexture.GetTemporary(Screen.width, Screen.height, 0);
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
