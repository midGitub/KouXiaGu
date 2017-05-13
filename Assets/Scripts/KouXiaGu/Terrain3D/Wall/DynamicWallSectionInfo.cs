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
        /// <summary>
        /// 根据定义间隔自动构建;
        /// </summary>
        public DynamicWallSectionInfo(Vector3[] vertices, float spacing)
        {
            Build(vertices, spacing);
        }

        /// <summary>
        /// 深拷贝;
        /// </summary>
        public DynamicWallSectionInfo(IEnumerable<Section> sections, IEnumerable<Point> points)
        {
            sections = sections.Select(item => new Section(item));
            sectionCollection = new List<Section>(sections);
            pointCollection = points.ToArray();
        }

        [SerializeField]
        List<Section> sectionCollection;
        [SerializeField]
        Point[] pointCollection;

        public IList<Section> Sections
        {
            get { return sectionCollection; }
        }

        public IList<Point> Points
        {
            get { return pointCollection; }
        }

        /// <summary>
        /// 构建节点记录;
        /// </summary>
        void Build(Vector3[] vertices, float spacing)
        {
            sectionCollection = new List<Section>();
            pointCollection = new Point[vertices.Length];
            SortedList<Record> verticeSortedList = SortByPosition_X(vertices);
            float start = verticeSortedList[0].Position.x;
            float end = verticeSortedList[verticeSortedList.Count - 1].Position.x;

            Section currentSection = CreateSection(start, end, start);
            sectionCollection.Add(currentSection);

            for (int index = 0; index < verticeSortedList.Count; index++)
            {
                Record record = verticeSortedList[index];
                int verticeIndex = record.Index;

                if (record.Position.x - currentSection.Position.x > spacing)
                {
                    currentSection = CreateSection(start, end, record.Position.x);
                    sectionCollection.Add(currentSection);
                }
                currentSection.Children.Add(verticeIndex);

                Point currentPoint = CreatePoint(currentSection, record.Position);
                pointCollection[verticeIndex] = currentPoint;
            }
        }

        /// <summary>
        /// 获取到根据点的X值从小到达排序的合集;
        /// </summary>
        SortedList<Record> SortByPosition_X(Vector3[] vertices)
        {
            SortedList<Record> verticeSortedList = new SortedList<Record>(VerticeComparer_x.instance);
            for (int i = 0; i < vertices.Length; i++)
            {
                Record record = new Record(i, vertices[i]);
                verticeSortedList.Add(record);
            }
            return verticeSortedList;
        }

        Section CreateSection(float start, float end, float current)
        {
            float interpolatedValue = (current - start) / (end - start);
            Vector3 nodePosition = new Vector3(current, 0, 0);
            var section = new Section(nodePosition, interpolatedValue);
            return section;
        }

        Point CreatePoint(Section parent, Vector3 point)
        {
            Vector3 localPosition = point - parent.Position;
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
        public IEnumerable<Vector3> GetVertices()
        {
            foreach (var section in sectionCollection)
            {
                foreach (var index in section.Children)
                {
                    Point pointObject = pointCollection[index];
                    yield return pointObject.LocalPosition + section.Position;
                }
            }
        }

        /// <summary>
        /// 对比坐标的x值;
        /// </summary>
        class VerticeComparer_x : IComparer<Record>
        {
            public static readonly VerticeComparer_x instance = new VerticeComparer_x();

            public int Compare(Record x, Record y)
            {
                return (x.Position.x - y.Position.x) < 0 ? -1 : 1;
            }
        }

        struct Record
        {
            public Record(int index, Vector3 position)
            {
                Index = index;
                Position = position;
            }

            public int Index { get; private set; }
            public Vector3 Position { get; private set; }
        }

        [Serializable]
        public class Section
        {
            Section()
            {
            }

            public Section(Section section)
            {
                position = section.position;
                interpolatedValue = section.interpolatedValue;
                children = new List<int>(section.children);
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
