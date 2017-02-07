using System;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 点坐标记录;
    /// </summary>
    public struct CheckPoint : IEquatable<CheckPoint>
    {

        public CheckPoint(int id, Vector3 point)
        {
            this.ID = id;
            this.Point = point;
        }


        public int ID { get; private set; }
        public Vector3 Point { get; private set; }


        public bool Equals(CheckPoint other)
        {
            return 
                other.ID == ID &&
                other.Point == Point;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is CheckPoint))
                return false;
            return Equals((CheckPoint)obj);
        }

        public override int GetHashCode()
        {
            return ID ^ Point.GetHashCode();
        }

    }

}
