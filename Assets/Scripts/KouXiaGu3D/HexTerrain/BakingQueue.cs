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
        OvenDisplayMeshQueue ovenDisplayMeshPool;
        [SerializeField]
        BakingParameter parameter;

        Queue<BakingRequest> bakingQueue;

        void Awake()
        {
            ovenDisplayMeshPool.Awake();
            bakingQueue = new Queue<BakingRequest>();
        }

        void Start()
        {
            bakingCamera.aspect = TerrainBlock.CameraAspect;
            bakingCamera.orthographicSize = TerrainBlock.CameraSize;
            bakingCamera.transform.position = Vector3.zero;
            bakingCamera.transform.rotation = TerrainBlock.CameraRotation;

            StartCoroutine(Baking());
        }

        /// <summary>
        /// 加入到烘焙队列;
        /// </summary>
        public void Enqueue(BakingRequest request)
        {
            bakingQueue.Enqueue(request);
        }

        IEnumerator Baking()
        {
            CustomYieldInstruction bakingYieldInstruction = new WaitWhile(() => bakingQueue.Count == 0);

            while (true)
            {
                yield return bakingYieldInstruction;

                BakingRequest request = bakingQueue.Dequeue();

                IEnumerable<KeyValuePair<MeshRenderer, BakingNode>> bakingNodes = PrepareBaking(request);



            }
        }

        /// <summary>
        /// 烘焙前的准备,返回烘焙对应的网格;
        /// </summary>
        List<KeyValuePair<MeshRenderer, BakingNode>> PrepareBaking(BakingRequest request)
        {
            ovenDisplayMeshPool.RecoveryActive();

            IEnumerable<BakingNode> bakingNodes = request.GetBakingNodes();
            List<KeyValuePair<MeshRenderer, BakingNode>> list = new List<KeyValuePair<MeshRenderer, BakingNode>>();

            foreach (var node in bakingNodes)
            {
                if (node.NotBoundary)
                {
                    Quaternion rotation = Quaternion.Euler(0, node.RotationY, 0);
                    var mesh = ovenDisplayMeshPool.Dequeue(node.Position, rotation);

                    list.Add(new KeyValuePair<MeshRenderer, BakingNode>(mesh, node));
                }
            }

            return list;
        }


        /// <summary>
        /// 在烘焙时显示在场景内的网格;
        /// </summary>
        [Serializable]
        class OvenDisplayMeshQueue
        {
            OvenDisplayMeshQueue() { }

            [SerializeField]
            Transform parent;

            [SerializeField]
            MeshRenderer ovenDisplayMesh;

            Queue<MeshRenderer> sleep;
            Queue<MeshRenderer> active;

            /// <summary>
            /// 激活在场景的物体;
            /// </summary>
            public IEnumerable<MeshRenderer> Active
            {
                get { return active; }
            }

            public void Awake()
            {
                sleep = new Queue<MeshRenderer>();
                active = new Queue<MeshRenderer>();
            }

            /// <summary>
            /// 回收所有激活的物体(将所有激活的物体设为睡眠模式);
            /// </summary>
            public void RecoveryActive()
            {
                while (active.Count != 0)
                {
                    var item = active.Dequeue();
                    Destroy(item);
                }
            }

            /// <summary>
            /// 获取到一个网格物体;
            /// </summary>
            public MeshRenderer Dequeue(Vector3 position, Quaternion rotation)
            {
                if (sleep.Count == 0)
                {
                    return GameObject.Instantiate(ovenDisplayMesh, position, rotation, parent) as MeshRenderer;
                }
                else
                {
                    MeshRenderer mesh = sleep.Dequeue();
                    mesh.transform.position = position;
                    mesh.transform.rotation = rotation;
                    mesh.gameObject.SetActive(true);
                    return mesh;
                }
            }

            void Destroy(MeshRenderer mesh)
            {
                mesh.gameObject.SetActive(false);
                sleep.Enqueue(mesh);
            }

        }

    }


    /// <summary>
    /// 烘焙的参数;
    /// </summary>
    [SerializeField]
    public class BakingParameter
    {

        ////烘焙参数;
        public int DiffuseMapWidth;
        public int DiffuseMapHeight;
        public int DiffuseMapAntiAliasing;

        public int HeightMapWidth;
        public int HeightMapHeight;
        public int HeightMapAntiAliasing;

    }

}
