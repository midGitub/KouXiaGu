﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D.Wall
{

    /// <summary>
    /// 动态墙体,模型中心轴需要沿着z轴;
    /// </summary>
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

        /// <summary>
        /// 节点信息;
        /// </summary>
        public JointInfo JointInfo
        {
            get { return joint; }
        }

        /// <summary>
        /// 对应节点位置,转换后的顶点坐标;
        /// </summary>
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
        /// 更改顶点坐标到曲线;
        /// </summary>
        public void Transformation(ISpline spline, ref Vector3[] vertices)
        {
            IList<JointPoint> jointPoints = JointInfo.JointPoints;
            int endIndex = jointPoints.Count - 1;

            for (int i = 0; i < jointPoints.Count; i++)
            {
                Vector3 currentJointPoint = spline.InterpolatedPoint(jointPoints[i].InterpolatedValue);
                Vector3 afterJointPoint;
                float angle;

                if (i < endIndex)
                {
                    afterJointPoint = spline.InterpolatedPoint(jointPoints[i + 1].InterpolatedValue);
                    angle = AngleY(currentJointPoint, afterJointPoint);
                }
                else
                {
                    afterJointPoint = spline.InterpolatedPoint(jointPoints[i - 1].InterpolatedValue);
                    angle = AngleY(afterJointPoint, currentJointPoint);
                }
                TransformSection(i, currentJointPoint, angle, ref vertices);
            }
        }

        /// <summary>
        /// 更改单个节点的旋转角度;
        /// </summary>
        /// <param name="sectionIndex">节点坐标下标;</param>
        /// <param name="position">更改后的位置;</param>
        /// <param name="angle">旋转角度,单位为弧度;</param>
        /// <param name="vertices">进行变化的顶点;</param>
        public void TransformSection(int sectionIndex, Vector3 position, float angle, ref Vector3[] vertices)
        {
            JointPoint section = JointInfo.JointPoints[sectionIndex];
            foreach (var childIndex in section.Children)
            {
                WallVertice info = Points[childIndex];
                float localRadius = info.localRadius;
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
                float localRadius = info.localRadius;
                float localAngle = info.LocalAngle + angle;
                Vector3 position = section.Position;
                position.x += Mathf.Sin(localAngle) * localRadius;
                position.y += info.LocalPosition.y;
                position.z += Mathf.Cos(localAngle) * localRadius;
                vertices[childIndex] = position;
            }
        }
    }
}
