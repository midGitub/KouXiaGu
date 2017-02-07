using System;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 点坐标记录;
    /// </summary>
    [Serializable]
    public struct CheckPoint : IEquatable<CheckPoint>
    {

        public CheckPoint(int id, Vector3 point)
        {
            this.id = id;
            this.point = point;
        }

        [SerializeField]
        int id;

        [SerializeField]
        Vector3 point;


        public int ID
        {
            get { return id; }
        }

        public Vector3 Point
        {
            get { return point; }
        }


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
