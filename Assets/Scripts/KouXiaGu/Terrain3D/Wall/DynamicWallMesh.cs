using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Collections;

namespace KouXiaGu.Terrain3D
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

        [SerializeField]
        float spacing;
        [SerializeField, HideInInspector]
        DynamicWallSectionInfo dynamicWall;

        public DynamicWallSectionInfo DynamicWall
        {
            get { return dynamicWall; }
        }

        [ContextMenu("Build")]
        void Build()
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            Mesh mesh = meshFilter.sharedMesh;
            Build(mesh.vertices);
        }

        /// <summary>
        /// 构建节点记录;
        /// </summary>
        void Build(Vector3[] vertices)
        {
            dynamicWall = new DynamicWallSectionInfo(vertices, spacing);
        }
    }

}
