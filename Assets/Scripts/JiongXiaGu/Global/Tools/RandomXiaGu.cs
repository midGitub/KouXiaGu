using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu
{



    [Obsolete]
    public class RandomXiaGu
    {
        static RandomXiaGu()
        {
            Default = new RandomXiaGu();
        }

        public RandomXiaGu()
        {
            random = new Random();
        }

        public static RandomXiaGu Default { get; private set; }

        readonly Random random;

        /// <summary>
        /// 返回一个随机的0~360的角度;
        /// </summary>
        public float Angle()
        {
            return random.Next(0, 360);
        }

        /// <summary>
        /// 返回一个随机的布尔值;
        /// </summary>
        public bool Boolean()
        {
            int i = random.Next(0, 2);
            return i == 0;
        }

        /// <summary>
        /// 从中随机获取到元素;
        /// </summary>
        public T Get<T>(IList<T> array)
        {
            int index = random.Next(0, array.Count);
            return array[index];
        }
    }
}
