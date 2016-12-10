using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.HexTerrain
{


    /// <summary>
    /// 对基本地形进行后期的添加和修改;
    /// </summary>
    [DisallowMultipleComponent]
    public class DecorateRenderer : UnitySingleton<DecorateRenderer>
    {
        DecorateRenderer() { }


        /// <summary>
        /// 负责渲染的摄像机;
        /// </summary>
        [SerializeField]
        Camera bakingCamera;

        [SerializeField]
        Shader flatTerrain;


        Material flatTerrainMaterial;


        [SerializeField]
        Texture test_Height;
        [SerializeField]
        MeshRenderer test_mesh;


        void Start()
        {
            InitBakingCamera();
            InitMaterial();
        }

        /// <summary>
        /// 初始化烘焙相机参数;
        /// </summary>
        void InitBakingCamera()
        {
            bakingCamera.aspect = BakingParameter.CameraAspect;
            bakingCamera.orthographicSize = BakingParameter.CameraSize;
            bakingCamera.transform.rotation = BakingParameter.CameraRotation;
        }

        void InitMaterial()
        {
            flatTerrainMaterial = new Material(flatTerrain);
            flatTerrainMaterial.hideFlags = HideFlags.HideAndDontSave;
        }

        [ContextMenu("TTTTT")]
        void Test()
        {
            var rt = FlatTerrain(test_Height);
            rt.SavePNG(@"123");
        }


        #region 烘焙队列;


        IEnumerator Baking()
        {
            yield break;
        }


        /// <summary>
        /// 平整地形;
        /// </summary>
        RenderTexture FlatTerrain(Texture heightTexure)
        {
            //if (test_mesh.material != null)
            //    Destroy(test_mesh.material);

            //test_mesh.material = flatTerrainMaterial;

            //test_mesh.material.SetTexture("_HeightTex", heightTexure);

            RenderTexture heightRT = RenderTexture.GetTemporary(heightTexure.height, heightTexure.width, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1);
            Render(heightRT);
            return heightRT;
        }


        void Render(RenderTexture rt)
        {
            bakingCamera.targetTexture = rt;
            bakingCamera.Render();
            bakingCamera.targetTexture = null;
        }

        #endregion

    }

}
