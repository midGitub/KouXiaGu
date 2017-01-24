using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{


    public static class MathI
    {


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
