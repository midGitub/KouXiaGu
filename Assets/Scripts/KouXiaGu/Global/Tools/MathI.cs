using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{


    public static class MathI
    {


        public static float Clamp01(float value)
        {
            value = value < 0 ? 0 : value;
            value = value > 1 ? 1 : value;
            return value;
        }

        public static float Clamp(float value, float min, float max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException();

            value = value < min ? min : value;
            value = value > max ? max : value;
            return value;
        }

        public static int Clamp(int value, int min, int max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException();

            value = value < min ? min : value;
            value = value > max ? max : value;
            return value;
        }

        public static short Clamp(short value, short min, short max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException();

            value = value < min ? min : value;
            value = value > max ? max : value;
            return value;
        }


    }

}
