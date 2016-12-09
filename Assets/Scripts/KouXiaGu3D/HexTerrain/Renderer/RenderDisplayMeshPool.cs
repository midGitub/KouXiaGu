using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.HexTerrain
{

    /// <summary>
    /// 在烘焙时显示在场景内的网格;
    /// </summary>
    [Serializable]
    public class RenderDisplayMeshPool
    {
        RenderDisplayMeshPool() { }

        [SerializeField]
        Transform parent;

        [SerializeField]
        MeshRenderer ovenDisplayPrefab;

        Queue<MeshRenderer> sleep;
        Queue<MeshRenderer> active;

        float rotationX;

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
            rotationX = ovenDisplayPrefab.transform.rotation.eulerAngles.x;
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
        public MeshRenderer Dequeue(Vector3 position, float rotationY)
        {
            MeshRenderer mesh;
            Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
            if (sleep.Count == 0)
            {
                mesh = GameObject.Instantiate(ovenDisplayPrefab, position, rotation, parent) as MeshRenderer;
                mesh.gameObject.SetActive(true);
            }
            else
            {
                mesh = sleep.Dequeue();
                mesh.transform.position = position;
                mesh.transform.rotation = rotation;
                mesh.gameObject.SetActive(true);
            }

            active.Enqueue(mesh);
            return mesh;
        }

        void Destroy(MeshRenderer mesh)
        {
            mesh.gameObject.SetActive(false);
            sleep.Enqueue(mesh);
        }

    }


}
