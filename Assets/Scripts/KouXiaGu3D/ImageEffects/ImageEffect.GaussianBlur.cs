﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.ImageEffects
{

    public sealed partial class ImageEffect : GlobalSington<ImageEffect>
    {

        [SerializeField]
        Shader gaussianBlur;

        /// <summary>
        /// 获取到高斯模糊后的效果;
        /// </summary>
        /// <param name="radius">[模糊半径]此值越大相邻像素间隔越远，图像越模糊。但过大的值会导致失真</param>
        /// <param name="downSample">[降分辨率]此值越大,则采样间隔越大,需要处理的像素点越少,运行速度越快</param>
        /// <param name="iteration">[迭代次数]此值越大,则模糊操作的迭代次数越多，模糊效果越好，但消耗越大</param>
        public static RenderTexture GaussianBlur(Texture source, float radius, int downSample, int iteration)
        {
            RenderTexture rt = null;
            try
            {
                rt = RenderTexture.GetTemporary(source.width >> downSample, source.height >> downSample, 0);
                GaussianBlur(source, rt, radius, downSample, iteration);
                return rt;
            }
            catch(Exception ex)
            {
                if (rt != null)
                    RenderTexture.ReleaseTemporary(rt);
                throw ex;
            }
        }

        /// <summary>
        /// 获取到高斯模糊后的效果;
        /// </summary>
        /// <param name="radius">[模糊半径]此值越大相邻像素间隔越远，图像越模糊。但过大的值会导致失真</param>
        /// <param name="downSample">[降分辨率]此值越大,则采样间隔越大,需要处理的像素点越少,运行速度越快</param>
        /// <param name="iteration">[迭代次数]此值越大,则模糊操作的迭代次数越多，模糊效果越好，但消耗越大</param>
        public static void GaussianBlur(Texture source, RenderTexture destination, float radius, int downSample, int iteration)
        {
            Material material = null;
            RenderTexture rt = null;
            try
            {
                material = new Material(GetInstance.gaussianBlur);
                rt = RenderTexture.GetTemporary(source.width >> downSample, source.height >> downSample, 0);

                //进行迭代高斯模糊  
                for (int i = 0; i < iteration; i++)
                {
                    //第一次高斯模糊，设置offsets，竖向模糊  
                    material.SetVector("_offsets", new Vector4(0, radius, 0, 0));
                    Graphics.Blit(source, rt, material);
                    //第二次高斯模糊，设置offsets，横向模糊  
                    material.SetVector("_offsets", new Vector4(radius, 0, 0, 0));
                    Graphics.Blit(rt, destination, material);
                }
            }
            finally
            {
                if (rt != null)
                    RenderTexture.ReleaseTemporary(rt);
                if(material != null)
                    DestroyImmediate(material);
            }
        }



    }

}
