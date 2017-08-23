﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;

namespace KouXiaGu.Terrain3D.Effects
{

    /// <summary>
    /// 地图区域效果,应该挂载到显示相机的子节点下;
    /// </summary>
    [RequireComponent(typeof(Camera))]
    [DisallowMultipleComponent]
    public sealed class TerrainAreaCameraEffect : MonoBehaviour
    {
        TerrainAreaCameraEffect()
        {
        }

        //[CustomUnityLayer("用于显示选中的区域!")]
        //public const string StencilLayer = "SelectMapNode";
        const string StencilCameraName = "SelectNodeStencilCamera";

        Camera mainCamera;
        Camera stencilCamera;
        public Shader edgePreserveShader;
        public LayerMask cullingMask;
        Material edgePreserveMaterial;

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

        //似乎在Unity2017.1版本在 OnPreRender() 内渲染不到正确的图像;

        //void OnPreRender()
        //{
        //    stencilCamera.fieldOfView = mainCamera.fieldOfView;
        //    stencilCamera.orthographic = mainCamera.orthographic;
        //    stencilCamera.nearClipPlane = mainCamera.nearClipPlane;
        //    stencilCamera.farClipPlane = mainCamera.farClipPlane;

        //    stencilMap = RenderTexture.GetTemporary(Screen.width, Screen.height, 0);
        //    stencilCamera.targetTexture = stencilMap;
        //    stencilCamera.Render();
        //    stencilCamera.targetTexture = null;
        //}

        //void OnPostRender()
        //{
        //    RenderTexture.ReleaseTemporary(stencilMap);
        //}

        void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
        {
            if (TerrainAreaEffect.TerrainAreaEffectItems.Count > 0)
            {
                stencilCamera.fieldOfView = mainCamera.fieldOfView;
                stencilCamera.orthographic = mainCamera.orthographic;
                stencilCamera.nearClipPlane = mainCamera.nearClipPlane;
                stencilCamera.farClipPlane = mainCamera.farClipPlane;

                foreach (var item in TerrainAreaEffect.TerrainAreaEffectItems)
                {
                    item.SetDisplay(true);
                    RenderTexture stencilMap = RenderTexture.GetTemporary(Screen.width, Screen.height, 0);
                    stencilCamera.targetTexture = stencilMap;
                    stencilCamera.Render();
                    stencilCamera.targetTexture = null;

                    edgePreserveMaterial.SetTexture("_StencilMap", stencilMap);
                    edgePreserveMaterial.SetFloat("_OutLineWidth", item.OutLineWidth);
                    edgePreserveMaterial.SetFloat("_Transparency", item.InternalTransparency);
                    Graphics.Blit(sourceTexture, destTexture, edgePreserveMaterial);
                    Graphics.Blit(destTexture, sourceTexture);

                    RenderTexture.ReleaseTemporary(stencilMap);
                    item.SetDisplay(false);
                }
            }
        }
    }
}
