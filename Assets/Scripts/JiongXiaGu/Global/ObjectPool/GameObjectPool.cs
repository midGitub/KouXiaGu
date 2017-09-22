using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JiongXiaGu
{

    /// <summary>
    /// UnityEngine.Component 泛型对象池;
    /// </summary>
    [Serializable]
    public class GameObjectPool<T> : ObjectPool<T>
        where T : Component
    {
        protected GameObjectPool()
        {
        }

        public GameObjectPool(T prefab, Transform objectParent)
        {
            this.prefab = prefab;
            this.objectParent = objectParent;
        }


        [SerializeField]
        T prefab;
        [SerializeField]
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
            if (ObjectParent == null)
            {
                return GameObject.Instantiate(prefab);
            }
            else
            {
                return GameObject.Instantiate(prefab, objectParent);
            }
        }

        public override void Destroy(T item)
        {
            GameObject.Destroy(item.gameObject);
        }

        public override void ResetWhenEnterPool(T item)
        {
            item.gameObject.SetActive(false);
            item.gameObject.hideFlags = HideFlags.HideAndDontSave;
        }

        public override void ResetWhenOutPool(T item)
        {
            item.gameObject.SetActive(true);
            item.gameObject.hideFlags = HideFlags.None;
        }
    }
}
