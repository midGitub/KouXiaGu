using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using KouXiaGu.World2D;

namespace KouXiaGu
{

    [DisallowMultipleComponent]
    public class ComponentCloneTest : MonoBehaviour
    {
        [SerializeField]
        private Transform prefab;
        [SerializeField]
        private int count;

        private void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Vector2 position = WorldConvert.MouseToPlane();
                Quaternion rotation = prefab.rotation;

                for (int i = 0; i < 6; i++)
                {
                    ThreadPool.QueueUserWorkItem(_ => Op(prefab, position, rotation));
                }
            }
        }

        private void Op(Transform prefab, Vector2 position, Quaternion rotation)
        {
            for (int i = 0; i < 6; i++)
            {
                UnityObjectClone.InstantiateInThread(prefab, position, rotation);
                count++;
            }
        }

    }

}
