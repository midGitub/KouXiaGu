using JiongXiaGu.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JiongXiaGu.ImageEffects
{

    /// <summary>
    /// 图片特效静态方法类;
    /// </summary>
    [ExecuteInEditMode, DisallowMultipleComponent]
    public sealed partial class ImageEffect : MonoBehaviour
    {
        ImageEffect()
        {
        }

        [SerializeField]
        BlurOptimized blurOptimized;

        [SerializeField]
        GaussianBlur gaussianBlur;

        //public static BlurOptimized BlurOptimized
        //{
        //    get { return Instance.blurOptimized; }
        //}

        //public static GaussianBlur GaussianBlur
        //{
        //    get { return Instance.gaussianBlur; }
        //}

    }

}
