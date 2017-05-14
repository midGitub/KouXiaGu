using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D.Wall
{

    /// <summary>
    /// 关节点,存储对应节点;
    /// </summary>
    [Serializable]
    public class JointPoint
    {
        JointPoint()
        {
        }

        public JointPoint(JointPoint section)
        {
            position = section.position;
            interpolatedValue = section.interpolatedValue;
            children = new List<int>(section.children);
        }

        public JointPoint(Vector3 position, float interpolatedValue)
        {
            this.position = position;
            this.interpolatedValue = interpolatedValue;
            children = new List<int>();
        }

        [SerializeField]
        Vector3 position;
        [SerializeField]
        float interpolatedValue;
        [SerializeField]
        List<int> children;

        public Vector3 Position
        {
            get { return position; }
        }

        public float InterpolatedValue
        {
            get { return interpolatedValue; }
        }

        public List<int> Children
        {
            get { return children; }
        }
    }

    /// <summary>
    /// 转换为本地坐标的顶点;
    /// </summary>
    [Serializable]
    public struct WallVertice
    {
        public WallVertice(Vector3 localPosition, float localAngle)
        {
            this.localPosition = localPosition;
            this.localAngle = localAngle;
        }

        [SerializeField]
        Vector3 localPosition;
        [SerializeField]
        float localAngle;

        public float localRadius
        {
            get { return Mathf.Sqrt(localPosition.x * localPosition.x + localPosition.z * localPosition.z); }
        }

        /// <summary>
        /// 相对于父节点的位置;
        /// </summary>
        public Vector3 LocalPosition
        {
            get { return localPosition; }
        }

        /// <summary>
        /// 相对于父节点的角度;
        /// </summary>
        public float LocalAngle
        {
            get { return localAngle; }
        }

        public override string ToString()
        {
            return "[Position:" + localPosition + ",Angle:" + localAngle + "]";
        }
    }

}
