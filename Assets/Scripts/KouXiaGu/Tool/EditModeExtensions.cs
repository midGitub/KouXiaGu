using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 编辑模式下的拓展方法;
    /// </summary>
    public static class EditModeExtensions
    {

        /// <summary>
        /// 销毁其,若在 Edit 模式下则为 DestroyImmediate(),否则为 Destroy();
        /// </summary>
        public static void DestroyXia(this UnityEngine.Object mesh)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                GameObject.DestroyImmediate(mesh);
            }
            else
#endif
            {
                GameObject.Destroy(mesh);
            }
        }


    }

}
