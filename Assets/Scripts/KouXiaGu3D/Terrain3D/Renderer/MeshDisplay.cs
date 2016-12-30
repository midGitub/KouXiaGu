using System;
using System.Collections.Generic;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 场景内的网格控制;
    /// </summary>
    [Serializable]
    public class MeshDisplay
    {
        [SerializeField]
        Transform parent;

        [SerializeField]
        MeshRenderer ovenDisplayPrefab;

        /// <summary>
        /// 中心点,根据传入坐标位置转换到此中心点附近;
        /// </summary>
        [SerializeField]
        CubicHexCoord center;

        Queue<MeshRenderer> sleep;
        Queue<MeshRenderer> active;

        public CubicHexCoord Center
        {
            get { return center; }
        }

        public void Awake()
        {
            sleep = new Queue<MeshRenderer>();
            active = new Queue<MeshRenderer>();
        }

        /// <summary>
        /// 布置到场景;
        /// </summary>
        public MeshRenderer Dequeue(CubicHexCoord coord, CubicHexCoord center, float rotationY)
        {
            MeshRenderer mesh;
            Quaternion rotation = Quaternion.Euler(0, rotationY, 0);
            Vector3 position = PositionConvert(coord, center);
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

        Vector3 PositionConvert(CubicHexCoord coord, CubicHexCoord center)
        {
            CubicHexCoord oCoord = coord - center;
            CubicHexCoord rCoord = oCoord + this.center;
            return GridConvert.Grid.GetPixel(rCoord, -active.Count);
        }

        /// <summary>
        /// 回收所有在场景中的网格;
        /// </summary>
        public void RecoveryActive()
        {
            while (active.Count != 0)
            {
                var item = active.Dequeue();
                Destroy(item);
            }
        }

        void Destroy(MeshRenderer mesh)
        {
            mesh.gameObject.SetActive(false);
            sleep.Enqueue(mesh);
        }

    }

}
