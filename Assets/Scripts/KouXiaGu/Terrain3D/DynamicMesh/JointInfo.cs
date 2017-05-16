﻿using KouXiaGu.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KouXiaGu.Terrain3D.Wall
{

    /// <summary>
    /// 动态墙体节点信息;
    /// </summary>
    [Serializable]
    public class JointInfo
    {
        /// <summary>
        /// 根据定义间隔自动构建;
        /// </summary>
        public JointInfo(Vector3[] vertices, float spacing)
        {
            AutoBuild(vertices, spacing);
        }

        /// <summary>
        /// 深拷贝;
        /// </summary>
        public JointInfo(JointInfo info)
        {
            IEnumerable<JointPoint> sections = info.sectionCollection.Select(item => new JointPoint(item));
            sectionCollection = new List<JointPoint>(sections);
        }

        [SerializeField]
        List<JointPoint> sectionCollection;

        public IList<JointPoint> JointPoints
        {
            get { return sectionCollection; }
        }

        /// <summary>
        /// 构建节点记录;
        /// </summary>
        void AutoBuild(Vector3[] vertices, float spacing)
        {
            sectionCollection = new List<JointPoint>();
            SortedList<Record> verticeSortedList = SortByPosition_Z(vertices);
            float start = verticeSortedList[0].Position.z;
            float end = verticeSortedList[verticeSortedList.Count - 1].Position.z;

            JointPoint currentSection = CreateSection(start, end, start);
            sectionCollection.Add(currentSection);

            for (int index = 0; index < verticeSortedList.Count; index++)
            {
                Record record = verticeSortedList[index];
                int verticeIndex = record.Index;
                if (record.Position.z - currentSection.Position.z > spacing)
                {
                    currentSection = CreateSection(start, end, record.Position.z);
                    sectionCollection.Add(currentSection);
                }
                currentSection.Children.Add(verticeIndex);
            }
        }

        /// <summary>
        /// 获取到根据点的X值从小到达排序的合集;
        /// </summary>
        SortedList<Record> SortByPosition_Z(Vector3[] vertices)
        {
            SortedList<Record> verticeSortedList = new SortedList<Record>(VerticeComparer_Z.instance);
            for (int i = 0; i < vertices.Length; i++)
            {
                Record record = new Record(i, vertices[i]);
                verticeSortedList.Add(record);
            }
            return verticeSortedList;
        }

        JointPoint CreateSection(float start, float end, float current)
        {
            float interpolatedValue = (current - start) / (end - start);
            Vector3 nodePosition = new Vector3(0, 0, current);
            var section = new JointPoint(nodePosition, interpolatedValue);
            return section;
        }

        /// <summary>
        /// 对比坐标的z值;
        /// </summary>
        class VerticeComparer_Z : IComparer<Record>
        {
            public static readonly VerticeComparer_Z instance = new VerticeComparer_Z();

            public int Compare(Record x, Record y)
            {
                return (x.Position.z - y.Position.z) < 0 ? -1 : 1;
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
    }
}