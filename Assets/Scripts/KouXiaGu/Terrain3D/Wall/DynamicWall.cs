using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D.Wall
{

    [Serializable]
    public class DynamicWall
    {
        DynamicWall()
        {
        }

        public DynamicWall(JointInfo joint, Vector3[] vertices)
        {
            this.joint = joint;
            pointCollection = Build(joint, vertices);
        }

        [SerializeField]
        JointInfo joint;
        [SerializeField]
        WallVertice[] pointCollection;

        public JointInfo JointInfo
        {
            get { return joint; }
        }

        public IList<WallVertice> Points
        {
            get { return pointCollection; }
        }

        /// <summary>
        /// 根据节点信息生成点合集;
        /// </summary>
        WallVertice[] Build(JointInfo joint, Vector3[] vertices)
        {
            WallVertice[] pointCollection = new WallVertice[vertices.Length];
            foreach (var section in joint.JointPoints)
            {
                foreach (var childIndex in section.Children)
                {
                    Vector3 childPosition = vertices[childIndex];
                    Vector3 localPosition = childPosition - section.Position;
                    float localAngle = AngleY(section.Position, childPosition);
                    WallVertice pointInfo = new WallVertice(localPosition, localAngle);
                    pointCollection[childIndex] = pointInfo;
                }
            }
            return pointCollection;
        }

        /// <summary>
        /// 获取 from 到 to 的角度(忽略Y),单位弧度;原型:Mathf.Atan2();
        /// </summary>
        float AngleY(Vector3 from, Vector3 to)
        {
            return Mathf.Atan2((to.x - from.x), (to.z - from.z));
        }

        /// <summary>
        /// 更改顶点坐标到曲线;
        /// </summary>
        void Transformation(ref Vector3[] vertices, ISpline spline)
        {
            foreach (var section in JointInfo.JointPoints)
            {
                Vector3 newSection = spline.InterpolatedPoint(section.InterpolatedValue);
                foreach (var childIndex in section.Children)
                {
                    Vector3 newVertice = Points[childIndex].LocalPosition + newSection;
                    vertices[childIndex] = newVertice;
                }
            }
        }

        /// <summary>
        /// 更改单个节点坐标;
        /// </summary>
        /// <param name="sectionIndex">节点坐标下标;</param>
        /// <param name="position">更改后的位置;</param>
        /// <param name="vertices">进行变化的顶点;</param>
        public void ChangeSection(int sectionIndex, Vector3 position, ref Vector3[] vertices)
        {
            JointPoint section = JointInfo.JointPoints[sectionIndex];
            foreach (var childIndex in section.Children)
            {
                Vector3 newVertice = Points[childIndex].LocalPosition + position;
                vertices[childIndex] = newVertice;
            }
        }

        /// <summary>
        /// 更改单个节点的旋转角度;
        /// </summary>
        /// <param name="sectionIndex">节点坐标下标;</param>
        /// <param name="angle">旋转角度,单位为弧度;</param>
        /// <param name="vertices">进行变化的顶点;</param>
        public void ChangeSection(int sectionIndex, float angle, ref Vector3[] vertices)
        {
            JointPoint section = JointInfo.JointPoints[sectionIndex];
            foreach (var childIndex in section.Children)
            {
                WallVertice info = Points[childIndex];
                float localRadius = info.localRadius;
                float localAngle = info.LocalAngle + angle;
                Vector3 position = section.Position;
                position.x += Mathf.Sin(localAngle) * localRadius;
                position.y += info.LocalPosition.y;
                position.z += Mathf.Cos(localAngle) * localRadius;
                vertices[childIndex] = position;
            }
        }

        /// <summary>
        /// 获取到半径圆上任何一点,忽略Y轴;
        /// </summary>
        Vector3 Circle(Vector3 position, WallVertice info, float angle)
        {
            float radius = info.localRadius;
            angle = info.LocalAngle + angle;
            position.x += Mathf.Sin(angle) * radius;
            position.y += info.LocalPosition.y;
            position.z += Mathf.Cos(angle) * radius;
            return position;
        }
    }
}
