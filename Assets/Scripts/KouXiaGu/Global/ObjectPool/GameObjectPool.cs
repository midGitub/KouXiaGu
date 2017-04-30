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
            if (prefab == null)
                throw new ArgumentNullException();
            this.prefab = prefab;
        }

        public GameObjectPool(T prefab, string parentName)
        {
            if (prefab == null)
                throw new ArgumentNullException();
            this.prefab = prefab;
            this.objectParent = new GameObject(parentName).transform;
        }

        public GameObjectPool(T prefab, int maxCapacity)
            : base(maxCapacity)
        {
            if (prefab == null)
                throw new ArgumentNullException();
            this.prefab = prefab;
        }

        readonly T prefab;
        Transform objectParent;

        public T Prefab
        {
            get { return prefab; }
        }

        public Transform ObjectParent
        {
            get { return objectParent; }
            set { objectParent = value; }
        }

        public override T Instantiate()
        {
            var item = GameObject.Instantiate(prefab);
            item.transform.SetParent(objectParent);
            return item;
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
            GameObject.Destroy(item.gameObject);
        }
    }

}
