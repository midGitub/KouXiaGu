﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Collections;

namespace KouXiaGu.Terrain3D.Wall
{

    /// <summary>
    /// 动态墙体网格;
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MeshFilter))]
    public sealed class DynamicWallMesh : MonoBehaviour
    {
        DynamicWallMesh()
        {
        }

        MeshFilter _meshFilter;
        [SerializeField]
        float spacing;
        [SerializeField]
        DynamicWall dynamicWall;

        public DynamicWall WallInfo
        {
            get { return dynamicWall; }
        }

        MeshFilter meshFilter
        {
            get { return _meshFilter ??
                    (_meshFilter = GetComponent<MeshFilter>()); }
        }

        [ContextMenu("Build")]
        void Build()
        {
            Mesh mesh = meshFilter.sharedMesh;
            Build(mesh.vertices, spacing);
        }

        /// <summary>
        /// 构建节点记录;
        /// </summary>
        void Build(Vector3[] vertices, float spacing)
        {
            JointInfo jointInfo = new JointInfo(vertices, spacing);
            dynamicWall = new DynamicWall(jointInfo, vertices);
        }

        /// <summary>
        /// 更改顶点坐标到曲线;
        /// </summary>
        void Transformation(ref Vector3[] vertices, ISpline spline)
        {
            foreach (var section in dynamicWall.JointInfo.JointPoints)
            {
                Vector3 newSection = spline.InterpolatedPoint(section.InterpolatedValue);
                foreach (var childIndex in section.Children)
                {
                    Vector3 newVertice = WallInfo.Points[childIndex].LocalPosition + newSection;
                    vertices[childIndex] = newVertice;
                }
            }
        }


        /// <summary>
        /// 更改节点坐标;
        /// </summary>
        /// <param name="sectionIndex">节点坐标下标;</param>
        /// <param name="position">更改后的位置;</param>
        public void ChangeSection(int sectionIndex, Vector3 position)
        {
            Mesh mesh = meshFilter.sharedMesh;
            Vector3[] vertices = mesh.vertices;
            ChangeSection(sectionIndex, position, ref vertices);
            mesh.vertices = vertices;
        }

        /// <summary>
        /// 更改节点坐标;
        /// </summary>
        /// <param name="sectionIndex">节点坐标下标;</param>
        /// <param name="position">更改后的位置;</param>
        /// <param name="vertices">进行变化的顶点;</param>
        public void ChangeSection(int sectionIndex, Vector3 position, ref Vector3[] vertices)
        {
            JointPoint section = WallInfo.JointInfo.JointPoints[sectionIndex];
            foreach (var childIndex in section.Children)
            {
                Vector3 newVertice = WallInfo.Points[childIndex].LocalPosition + position;
                vertices[childIndex] = newVertice;
            }
        }



        [ContextMenu("Test")]
        void Test()
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            Mesh mesh = meshFilter.sharedMesh;
            Vector3[] vertices = mesh.vertices;
            ISpline spline = new CatmullRomSpline(
                new Vector3(-1, 0, 0),
                new Vector3(0, 0, 1),
                new Vector3(1, 0, 0),
                new Vector3(2, 0, -1)
                );

            ISpline spline2 = new CatmullRomSpline(
                new Vector3(2, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(0, 0, 0),
                new Vector3(-1, 0, 0)
                );
            Transformation(ref vertices, spline);
            mesh.vertices = vertices;
        }

    }

}
