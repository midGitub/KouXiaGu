using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    public class GameObjectPool<T> : ObjectPool<T>
        where T : Component
    {

        readonly T prefab;

        public override void Destroy(T item)
        {
            throw new NotImplementedException();
        }

        public override T Instantiate()
        {
            return GameObject.Instantiate(prefab);
        }

        public override void ResetWhenEnterPool(T item)
        {
            throw new NotImplementedException();
        }

        public override void ResetWhenOutPool(T item)
        {
            throw new NotImplementedException();
        }
    }

}
