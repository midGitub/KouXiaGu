using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    /// <summary>
    /// 像素坐标;
    /// </summary>
    public struct Pixel
    {
        public Pixel(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        int x;
        int y;

        public int X
        {
            get { return x; }
        }

        public int Y
        {
            get { return y; }
        }

        public override string ToString()
        {
            return "(" + x + ", " + y + ")";
        }

    }

}
