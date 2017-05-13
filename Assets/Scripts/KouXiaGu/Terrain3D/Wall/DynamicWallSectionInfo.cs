using KouXiaGu.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 动态墙体节点信息;
    /// </summary>
    [Serializable]
    public class DynamicWallSectionInfo
    {
        DynamicWallSectionInfo()
        {
            sectionList = new List<Section>();
            pointList = new List<Point>();
        }

        public DynamicWallSectionInfo(Vector3[] vertices, float spacing) : this()
        {
            Build(vertices, spacing);
        }

        [SerializeField]
        List<Section> sectionList;
        [SerializeField]
        List<Point> pointList;

        public List<Section> SectionList
        {
            get { return sectionList; }
        }

        public List<Point> PointList
        {
            get { return pointList; }
        }

        public int VerticeCount
        {
            get { return pointList.Count; }
        }

        /// <summary>
        /// 构建节点记录;
        /// </summary>
        public void Build(Vector3[] vertices, float spacing)
        {
            SortedList<Vector3> verticeSortedList = new SortedList<Vector3>(vertices, VerticeComparer_x.instance);
            Section currentSection = CreateSection(verticeSortedList, 0);
            sectionList.Add(currentSection);

            for (int index = 0; index < verticeSortedList.Count; index++)
            {
                Vector3 point = verticeSortedList[index];
                if (point.x - currentSection.Position.x > spacing)
                {
                    currentSection = CreateSection(verticeSortedList, index);
                    sectionList.Add(currentSection);
                }
                currentSection.Children.Add(index);

                Point currentPoint = CreatePoint(currentSection, point);
                pointList.Add(currentPoint);
            }
        }

        Section CreateSection(SortedList<Vector3> verticeSortedList, int index)
        {
            Vector3 point = verticeSortedList[index];
            float start = verticeSortedList[0].x;
            float end = verticeSortedList[verticeSortedList.Count - 1].x;
            float interpolatedValue = (point.x - start) / (end - start);
            Vector3 nodePosition = new Vector3(point.x, 0, 0);
            var item = new Section(nodePosition, interpolatedValue);
            return item;
        }

        Point CreatePoint(Section parent, Vector3 point)
        {
            Vector3 localPosition = parent.Position - point;
            float localAngle = AngleY(parent.Position, point);
            var item = new Point(localPosition, localAngle);
            return item;
        }

        /// <summary>
        /// 返回弧度;
        /// </summary>
        float AngleY(Vector3 from, Vector3 to)
        {
            return Mathf.Atan2((to.x - from.x), (to.z - from.z));
        }

        /// <summary>
        /// 获取到所有原始的顶点坐标;
        /// </summary>
        public IEnumerable<Vector3> GetOriginalVertices()
        {
            foreach (var section in SectionList)
            {
                foreach (var index in section.Children)
                {
                    Point pointObject = pointList[index];
                    yield return pointObject.LocalPosition + section.Position;
                }
            }
        }

        /// <summary>
        /// 对比坐标的x值;
        /// </summary>
        class VerticeComparer_x : IComparer<Vector3>
        {
            public static readonly VerticeComparer_x instance = new VerticeComparer_x();

            /// <summary>
            /// 小于零  x 小于 y。
            /// 零      x 等于 y。
            /// 大于零  x 大于 y。
            /// </summary>
            public int Compare(Vector3 x, Vector3 y)
            {
                return (x.x - y.x) < 0 ? -1 : 1;
            }
        }

        [Serializable]
        public class Section
        {
            Section()
            {
            }

            public Section(Vector3 position, float interpolatedValue)
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

        [Serializable]
        public struct Point
        {
            public Point(Vector3 localPosition, float localAngle)
            {
                this.localPosition = localPosition;
                this.localAngle = localAngle;
            }

            [SerializeField]
            Vector3 localPosition;
            [SerializeField]
            float localAngle;

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
}
