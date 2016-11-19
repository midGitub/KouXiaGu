using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    [DisallowMultipleComponent]
    public class XiaGuObject : MonoBehaviour, IPoolObject
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


        public static implicit operator GameObject(XiaGuObject item)
        {
            return item.gameObject;
        }

        public static explicit operator XiaGuObject(GameObject item)
        {
            XiaGuObject instance = item.GetComponent<XiaGuObject>();

            if (instance == null)
                Debug.LogWarning("GameObject 不存在初始化组件!");

            return instance;
        }

    }

}
