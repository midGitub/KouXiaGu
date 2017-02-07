using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 墙体顶点编辑;
    /// </summary>
    [RequireComponent(typeof(MeshFilter))]
    [DisallowMultipleComponent, ExecuteInEditMode]
    public class WallMesh : MonoBehaviour
    {
        WallMesh()
        {
        }


        [SerializeField]
        WallVertice wallVertice;

        MeshFilter meshFilter;

        void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
        }

        void Reset()
        {
            wallVertice.Vertices = meshFilter.sharedMesh.vertices;
        }

    }

}
