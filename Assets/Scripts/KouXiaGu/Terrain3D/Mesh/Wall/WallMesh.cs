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



        /// <summary>
        /// 对顶点进行自动分段;
        /// </summary>
        /// <param name="vertices">原本的顶点数据</param>
        /// <param name="anchorPoints">锚点顺序</param>
        static WallVertice AutoSegmentation(Vector3[] vertices, IList<Vector3> anchorPoints)
        {
            HashSet<int> closeSet = new HashSet<int>();

            for (int i = 0; i < vertices.Length; i++)
            {
                if (closeSet.Contains(i))
                    continue;


            }

            throw new NotImplementedException();
        }

    }

}
