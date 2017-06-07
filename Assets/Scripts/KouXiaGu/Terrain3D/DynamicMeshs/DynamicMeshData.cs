using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ProtoBuf;

namespace KouXiaGu.Terrain3D.DynamicMeshs
{

    /// <summary>
    /// 存储网格原始数据,提供转换方法;
    /// </summary>
    [Serializable, ProtoContract]
    public class DynamicMeshData
    {
        DynamicMeshData()
        {
        }

        public DynamicMeshData(JointInfo joint, Vector3[] vertices)
        {
            Build(joint, vertices);
        }

        [SerializeField, ProtoMember(1)]
        JointInfo joint;
        [SerializeField, ProtoMember(2)]
        WallVertice[] pointCollection;

        /// <summary>
        /// 节点信息;
        /// </summary>
        public JointInfo JointInfo
        {
            get { return joint; }
            private set { joint = value; }
        }

        /// <summary>
        /// 对应节点位置,转换后的顶点坐标;
        /// </summary>
        public WallVertice[] Points
        {
            get { return pointCollection; }
            private set{ pointCollection = value; }
        }

        /// <summary>
        /// 设置新的节点信息,并且根据节点信息生成点合集;
        /// </summary>
        public void Build(JointInfo joint, Vector3[] vertices)
        {
            this.joint = joint;
            pointCollection = new WallVertice[vertices.Length];

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
        }

        /// <summary>
        /// 获取 from 到 to 的角度(忽略Y),单位弧度;原型:Mathf.Atan2();
        /// </summary>
        float AngleY(Vector3 from, Vector3 to)
        {
            return Mathf.Atan2((to.x - from.x), (to.z - from.z));
        }

        /// <summary>
        /// 获取到原本的顶点坐标合集;
        /// </summary>
        public Vector3[] GetOriginalVertices()
        {
            Vector3[] vertices = new Vector3[pointCollection.Length];
            foreach (var section in joint.JointPoints)
            {
                foreach (var childIndex in section.Children)
                {
                    WallVertice point = pointCollection[childIndex];
                    Vector3 vertice = point.LocalPosition + section.Position;
                    vertices[childIndex] = vertice;
                }
            }
            return vertices;
        }

        /// <summary>
        /// 转换到曲线路径,并且指定起点和终点的旋转角度;
        /// </summary>
        public void Transformation(ISpline spline, Vector3 start, Vector3 end, ref Vector3[] vertices)
        {
            if (pointCollection.Length != vertices.Length)
                throw new ArgumentException("预定义网格和模型网格不符合;");

            IList<JointPoint> jointPoints = JointInfo.JointPoints;
            int endIndex = jointPoints.Count - 1;

            JointPoint section = jointPoints[0];
            Vector3 position = spline.InterpolatedPoint(section.InterpolatedValue);
            JointPoint afterSection = jointPoints[1];
            Vector3 afterPosition = spline.InterpolatedPoint(afterSection.InterpolatedValue);
            float angle = AngleY(start, position);
            TransformSection(section, position, angle, ref vertices);
            section = afterSection;
            position = afterPosition;

            for (int i = 1; i < jointPoints.Count; i++)
            {
                if (i < endIndex)
                {
                    afterSection = jointPoints[i + 1];
                    afterPosition = spline.InterpolatedPoint(afterSection.InterpolatedValue);
                    angle = AngleY(position, afterPosition);
                }
                else
                {
                    afterSection = jointPoints[i - 1];
                    afterPosition = spline.InterpolatedPoint(afterSection.InterpolatedValue);
                    angle = AngleY(position, end);
                }

                TransformSection(section, position, angle, ref vertices);
                section = afterSection;
                position = afterPosition;
            }
        }

        /// <summary>
        /// 更改顶点坐标到曲线;
        /// </summary>
        public void Transformation(ISpline spline, ref Vector3[] vertices)
        {
            if (pointCollection.Length != vertices.Length)
                throw new ArgumentException("预定义网格和模型网格不符合;");

            IList<JointPoint> jointPoints = JointInfo.JointPoints;
            int endIndex = jointPoints.Count - 1;

            JointPoint section = jointPoints[0];
            Vector3 position = spline.InterpolatedPoint(section.InterpolatedValue);

            for (int i = 0; i < jointPoints.Count; i++)
            {
                JointPoint afterSection;
                Vector3 afterPosition;
                float angle;

                if (i < endIndex)
                {
                    afterSection = jointPoints[i + 1];
                    afterPosition = spline.InterpolatedPoint(afterSection.InterpolatedValue);
                    angle = AngleY(position, afterPosition);
                }
                else
                {
                    afterSection = jointPoints[i - 1];
                    afterPosition = spline.InterpolatedPoint(afterSection.InterpolatedValue);
                    angle = AngleY(afterPosition, position);
                }

                TransformSection(section, position, angle, ref vertices);
                section = afterSection;
                position = afterPosition;
            }
        }

        /// <summary>
        /// 更改单个节点的旋转角度;
        /// </summary>
        /// <param name="sectionIndex">节点坐标下标;</param>
        /// <param name="position">更改后的位置;</param>
        /// <param name="angle">旋转角度,单位为弧度;</param>
        /// <param name="vertices">进行变化的顶点;</param>
        void TransformSection(JointPoint section, Vector3 position, float angle, ref Vector3[] vertices)
        {
            foreach (var childIndex in section.Children)
            {
                WallVertice info = Points[childIndex];
                float localRadius = info.LocalRadius;
                float localAngle = info.LocalAngle + angle;
                Vector3 newPosition = position;
                newPosition.x += Mathf.Sin(localAngle) * localRadius;
                newPosition.y += info.LocalPosition.y;
                newPosition.z += Mathf.Cos(localAngle) * localRadius;
                vertices[childIndex] = newPosition;
            }
        }


        /// <summary>
        /// 更改单个节点坐标;
        /// </summary>
        /// <param name="sectionIndex">节点坐标下标;</param>
        /// <param name="position">更改后的位置;</param>
        /// <param name="vertices">进行变化的顶点;</param>
        public void TransformSection(int sectionIndex, Vector3 position, ref Vector3[] vertices)
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
        public void TransformSection(int sectionIndex, float angle, ref Vector3[] vertices)
        {
            JointPoint section = JointInfo.JointPoints[sectionIndex];
            foreach (var childIndex in section.Children)
            {
                WallVertice info = Points[childIndex];
                float localRadius = info.LocalRadius;
                float localAngle = info.LocalAngle + angle;
                Vector3 position = section.Position;
                position.x += Mathf.Sin(localAngle) * localRadius;
                position.y += info.LocalPosition.y;
                position.z += Mathf.Cos(localAngle) * localRadius;
                vertices[childIndex] = position;
            }
        }


        /// <summary>
        /// 转换为本地坐标的顶点;
        /// </summary>
        [Serializable, ProtoContract]
        public struct WallVertice
        {
            public WallVertice(Vector3 localPosition, float localAngle)
            {
                this.localPosition = localPosition;
                this.localAngle = localAngle;
            }

            [SerializeField, ProtoMember(1)]
            Vector3 localPosition;
            [SerializeField, ProtoMember(2)]
            float localAngle;

            public float LocalRadius
            {
                get { return Mathf.Sqrt(localPosition.x * localPosition.x + localPosition.z * localPosition.z); }
            }

            /// <summary>
            /// 相对于父节点的位置;
            /// </summary>
            public Vector3 LocalPosition
            {
                get { return localPosition; }
                private set { localPosition = value; }
            }

            /// <summary>
            /// 相对于父节点的角度;
            /// </summary>
            public float LocalAngle
            {
                get { return localAngle; }
                private set { localAngle = value; }
            }

            public override string ToString()
            {
                return "[Position:" + localPosition + ",Angle:" + localAngle + "]";
            }
        }
    }
}
