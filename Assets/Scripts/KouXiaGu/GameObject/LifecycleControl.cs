using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    [DisallowMultipleComponent]
    public class LifecycleControl : MonoBehaviour, IPoolObject
    {
        public uint MaxCountInPool
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Name { get; set; }

        /// <summary>
        /// 启用这个物体;
        /// </summary>
        public void OnActivate()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 停用这个物体;
        /// </summary>
        public void OnSleep()
        {
            throw new NotImplementedException();
        }


        public static implicit operator GameObject(LifecycleControl item)
        {
            return item.gameObject;
        }

        public static implicit operator LifecycleControl(GameObject item)
        {
            return item.GetComponent<LifecycleControl>();
        }

    }

}
