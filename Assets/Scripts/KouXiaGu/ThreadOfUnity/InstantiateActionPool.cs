using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.ThreadOfUnity
{

    public class InstantiateActionPool<T>
        where T : UnityEngine.Component
    {

        private ConcurrentQueue<InstantiateAction<T>> instanceQueue = new ConcurrentQueue<InstantiateAction<T>>();

        private void Awake() { }

        private bool TryDequeue(out InstantiateAction<T> instance)
        {
            Awake();
            if (instanceQueue.TryDequeue(out instance))
            {
                if (instance.OnInitializeQueue)
                {
                    instanceQueue.Enqueue(instance);
                    return false;
                }
                else
                {
                    instance.Clear();
                    return true;
                }
            }
            instance = default(InstantiateAction<T>);
            return false;
        }

        public InstantiateAction<T> Get(T original)
        {
            InstantiateAction<T> instance;
            if (TryDequeue(out instance))
                instance.Set(original);
            else
                instance = InstantiateAction<T>.GetNew(original);
            return instance;
        }

    }

}
