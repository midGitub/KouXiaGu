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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Vector2 position = WorldConvert.MouseToPlane();
                Quaternion rotation = prefab.rotation;
                ThreadPool.QueueUserWorkItem(_ => ObjectClone.InstantiateInThread(prefab, position, rotation));
            }
        }

    }

}
