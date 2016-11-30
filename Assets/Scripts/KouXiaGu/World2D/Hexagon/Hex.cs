using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World2D
{


    public struct Hex
    {
        public Hex(int q, int r, int s)
        {
            this.q = q;
            this.r = r;
            this.s = s;
        }
        public int q;
        public int r;
        public int s;

    }

}
