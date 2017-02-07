using UnityEngine;

namespace KouXiaGu
{

    public static class RandomColor
    {

        /// <summary>
        /// 返回一个随机的颜色;
        /// </summary>
        public static Color Next()
        {
            return new Color(RandomF(), RandomF(), RandomF());
        }

        static float RandomF()
        {
            return UnityEngine.Random.Range(0f, 1f);
        }

    }

}
