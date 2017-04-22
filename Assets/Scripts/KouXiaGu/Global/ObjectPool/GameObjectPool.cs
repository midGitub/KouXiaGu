using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// UnityEngine.Component 泛型对象池;
    /// </summary>
    public class GameObjectPool<T> : ObjectPool<T>
        where T : Component
    {

        public GameObjectPool(T prefab)
            : base()
        {
            this.prefab = prefab;
        }

        public GameObjectPool(T prefab, int maxCapacity)
            : base(maxCapacity)
        {
            this.prefab = prefab;
        }

        readonly T prefab;

        public T Prefab
        {
            get { return prefab; }
        }

        public override T Instantiate()
        {
            return GameObject.Instantiate(prefab);
        }

        public override void ResetWhenEnterPool(T item)
        {
            item.gameObject.SetActive(false);
        }

        public override void ResetWhenOutPool(T item)
        {
            item.gameObject.SetActive(true);
        }

        public override void Destroy(T item)
        {
            GameObject.DestroyObject(item.gameObject);
        }
    }

}
