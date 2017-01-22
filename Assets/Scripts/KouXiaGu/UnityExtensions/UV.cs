//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//namespace KouXiaGu
//{

//    /// <summary>
//    /// UV 坐标;
//    /// </summary>
//    [Obsolete]
//    public struct UV
//    {

//        public UV(float x, float y)
//        {
//            this.x = Saturate(x);
//            this.y = Saturate(y);
//        }

//        float x;
//        float y;

//        public float X
//        {
//            get { return x; }
//        }

//        public float Y
//        {
//            get { return y; }
//        }

//        public override string ToString()
//        {
//            return "(" + x + ", " + y + ")";
//        }

//        public static float Saturate(float value)
//        {
//            return Mathf.Clamp01(value);
//        }


//        public static implicit operator Vector2(UV coord)
//        {
//            return new Vector2(coord.x, coord.y);
//        }

//        public static implicit operator UV(Vector2 coord)
//        {
//            return new UV(coord.x, coord.y);
//        }

//    }

//}
