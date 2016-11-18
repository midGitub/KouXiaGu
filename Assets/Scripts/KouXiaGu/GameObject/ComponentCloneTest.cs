using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

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
                //ThreadPool.QueueUserWorkItem(_ => ComponentClone.InstantiateAsync());
            }
        }

    }

}
