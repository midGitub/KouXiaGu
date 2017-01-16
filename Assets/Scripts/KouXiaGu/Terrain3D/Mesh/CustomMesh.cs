using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 自定义网格结构;
    /// </summary>
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer)), ExecuteInEditMode, DisallowMultipleComponent]
    public abstract class CustomMesh : MonoBehaviour
    {

        /// <summary>
        /// 公共使用的地形块;
        /// </summary>
        protected abstract Mesh PublicMesh { get; set; }

        /// <summary>
        /// 创建一个新的地形块网格结构;
        /// </summary>
        protected abstract Mesh CreateMesh();

        protected virtual void Reset()
        {
            PublicMesh = CreateMesh();
            GetComponent<MeshFilter>().mesh = PublicMesh;
        }

        void Awake()
        {
            GetComponent<MeshFilter>().mesh = PublicMesh;
        }

    }

}
