using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XGame
{


    public static class Random
    {
        private static System.Random m_Random = new System.Random();


        public static IntVector2 IntVector2()
        {
            short x = (short)m_Random.Next();
            short y = (short)m_Random.Next();
            return new XGame.IntVector2(x, y);
        }

        public static IntVector2 IntVector2(IntVector2 southWesternPoint, IntVector2 northEastPoint)
        {
            short x = (short)m_Random.Next(southWesternPoint.x, northEastPoint.x);
            short y = (short)m_Random.Next(southWesternPoint.y, northEastPoint.y);
            return new XGame.IntVector2(x, y);
        }

        public static T Enum<T>()
            where T: struct
        {
            int randomIndex;
            T aspect;

            Array aspects = System.Enum.GetValues(typeof(T));
            randomIndex = m_Random.Next(0, aspects.Length);
            aspect = (T)aspects.GetValue(randomIndex);

            return aspect;
        }

    }

}
