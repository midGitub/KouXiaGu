using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KouXiaGu
{

    /// <summary>
    /// 对 System.Random 的拓展;
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// 返回一个随机的0~360的角度;
        /// </summary>
        public static float Angle(this Random random)
        {
            return random.Next(0, 360);
        }

        /// <summary>
        /// 返回一个随机的布尔值;
        /// </summary>
        public static bool Boolean(this Random random)
        {
            int i = random.Next(0, 2);
            return i == 0;
        }

        /// <summary>
        /// 从中随机获取到元素;
        /// </summary>
        public static T Get<T>(this Random random, IList<T> array)
        {
            int index = random.Next(0, array.Count);
            return array[index];
        }
    }
}
