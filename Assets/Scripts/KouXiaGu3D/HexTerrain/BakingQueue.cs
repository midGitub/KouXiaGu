using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using System.Collections;

namespace KouXiaGu.HexTerrain
{

    /// <summary>
    /// 烘焙贴图队列;
    /// 负责将传入的请求渲染出高度图和地貌贴图输出;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class BakingQueue : UnitySingleton<BakingQueue>
    {
        BakingQueue() { }

        /// <summary>
        /// 负责渲染地形的摄像机;
        /// </summary>
        [SerializeField]
        Camera bakingCamera;
        [SerializeField]
        OvenDisplayMeshPool ovenDisplayMeshPool;

        void Awake()
        {
            ovenDisplayMeshPool.Awake();
        }

        void Start()
        {
            
        }



        /// <summary>
        /// 在烘焙时显示在场景内的网格;
        /// </summary>
        [Serializable]
        class OvenDisplayMeshPool
        {
            OvenDisplayMeshPool() { }

            [SerializeField]
            Transform parent;

            [SerializeField]
            MeshRenderer ovenDisplayMesh;

            Queue<MeshRenderer> meshQueue;

            public void Awake()
            {
                meshQueue = new Queue<MeshRenderer>();
            }

            public MeshRenderer Instantiate(Vector3 position, Quaternion rotation)
            {
                if (meshQueue.Count == 0)
                {
                    return GameObject.Instantiate(ovenDisplayMesh, position, rotation, parent) as MeshRenderer;
                }
                else
                {
                    MeshRenderer mesh = meshQueue.Dequeue();
                    mesh.transform.position = position;
                    mesh.transform.rotation = rotation;
                    mesh.gameObject.SetActive(true);
                    return mesh;
                }
            }

            public void Destroy(MeshRenderer mesh)
            {
                mesh.gameObject.SetActive(false);
                meshQueue.Enqueue(mesh);
            }

        }

    }

}
