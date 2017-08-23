using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{


    public static class RandomXiaGu
    {
        static readonly System.Random random = new System.Random();

        /// <summary>
        /// 返回一个随机的0~360的角度;
        /// </summary>
        public static float Angle()
        {
            return UnityEngine.Random.Range(0, 360);
        }

        /// <summary>
        /// 返回一个随机的布尔值;
        /// </summary>
        public static bool Boolean()
        {
            int i = random.Next(0, 2);
            return i == 0;
        }

        /// <summary>
        /// 从中随机获取到元素;
        /// </summary>
        public static T Get<T>(IList<T> array)
        {
            int index = random.Next(0, array.Count);
            return array[index];
        }
    }
}
