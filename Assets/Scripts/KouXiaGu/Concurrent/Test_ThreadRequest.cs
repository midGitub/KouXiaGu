using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.World2D;
using System.Threading;

namespace KouXiaGu.Concurrent
{

    public class Test_ThreadRequest : MonoBehaviour
    {
        [SerializeField]
        Transform prefab;

        void Start()
        {
            ThreadRequest.GetInstance.InstantiateInQueue(prefab);
        }

        void Update()
        {
            Vector3 position = WorldConvert.MouseToPlane();
            Quaternion rotation = prefab.rotation;
            if (Input.GetKey(KeyCode.Mouse0))
            {
                ThreadPool.QueueUserWorkItem(_ => Ins(position, rotation));
            }
        }

        void Ins(Vector3 position, Quaternion rotation)
        {
            for (int i = 0; i < 8; i++)
            {
                ThreadRequest.GetInstance.InstantiateInQueue(prefab, position, rotation);
            }
        }

    }

}
