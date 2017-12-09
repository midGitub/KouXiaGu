using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity
{


    public static class UnityHelper
    {

        /// <summary>
        /// 同方法 UnityEngine.Object.Destroy(obj);
        /// </summary>
        public static void Destroy(this UnityEngine.Object obj)
        {
            UnityEngine.Object.Destroy(obj);
        }

        public static void DestroyAndSetNull<T>(ref T obj)
            where T : UnityEngine.Object
        {
            if (obj != null)
            {
                UnityEngine.Object.Destroy(obj);
                obj = null;
            }
        }
    }
}
