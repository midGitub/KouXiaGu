using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UniRx;

namespace KouXiaGu.World2D.Map
{

    /// <summary>
    /// 根据块加载的地图;
    /// </summary>
    /// <typeparam name="T">节点</typeparam>
    /// <typeparam name="TBlock">地图块</typeparam>
    [Serializable]
    public class NodeMap<T, TBlock> : BlockMap<TBlock>, IMap<IntVector2, T>, IBlockMap<ShortVector2, TBlock>,
        IObservable<KeyValuePair<IntVector2, T>>
        where T : struct
        where TBlock : IMap<ShortVector2, T>
    {

        IBlockMap<ShortVector2, TBlock> mapCollection
        {
            get { return (this as IBlockMap<ShortVector2, TBlock>); }
        }

        public IEnumerable<KeyValuePair<IntVector2, T>> NodePair
        {
            get
            {
                foreach (var block in base.BlocksPair)
                {
                    foreach (var node in block.Value.NodePair)
                    {
                        IntVector2 position = AddressToPosition(block.Key, node.Key);
                        yield return new KeyValuePair<IntVector2, T>(position, node.Value);
                    }
                }
            }
        }

        public T this[IntVector2 position]
        {
            get
            {
                ShortVector2 realPosition;
                TBlock block = TransformToBlock(position, out realPosition);
                return block[realPosition];
            }
            set
            {
                ShortVector2 realPosition;
                TBlock block = TransformToBlock(position, out realPosition);
                block[realPosition] = value;

                OnValueUpdate(position, value);
            }
        }

        public void Add(IntVector2 position, T item)
        {
            ShortVector2 realPosition;
            TBlock block = TransformToBlock(position, out realPosition);
            block.Add(realPosition, item);

            OnValueUpdate(position, item);
        }

        public bool Remove(IntVector2 position)
        {
            ShortVector2 realPosition;
            TBlock block = TransformToBlock(position, out realPosition);
            return block.Remove(realPosition);
        }

        public bool Contains(IntVector2 position)
        {
            ShortVector2 realPosition;
            TBlock block = TransformToBlock(position, out realPosition);
            return block.Contains(realPosition);
        }

        public bool TryGetValue(IntVector2 position, out T item)
        {
            ShortVector2 realPosition;
            TBlock block = TransformToBlock(position, out realPosition);
            return block.TryGetValue(realPosition, out item);
        }

        /// <summary>
        /// 转换成块的信息;
        /// </summary>
        TBlock TransformToBlock(IntVector2 position, out ShortVector2 realPosition)
        {
            TBlock block;
            ShortVector2 address = GetAddress(position, out realPosition);

            if (mapCollection.TryGetValue(address, out block))
            {
                return block;
            }
            throw BlockNotFoundException(address);

        }

        void IMap<IntVector2, T>.Clear()
        {
            mapCollection.Clear();
        }

        /// <summary>
        /// 返回地图块错误信息;
        /// </summary>
        BlockNotFoundException BlockNotFoundException(ShortVector2 address)
        {
            return new BlockNotFoundException(address.ToString() + "地图块未载入!");
        }


        List<IObserver<KeyValuePair<IntVector2, T>>> observers = new List<IObserver<KeyValuePair<IntVector2, T>>>();

        public override void Add(ShortVector2 position, TBlock item)
        {
            base.Add(position, item);
            foreach (var node in item.NodePair)
            {
                OnValueUpdate(node.Key, node.Value);
            }
        }

        public void OnValueUpdate(IntVector2 position, T worldNode)
        {
            KeyValuePair<IntVector2, T> pari = new KeyValuePair<IntVector2, T>(position, worldNode);
            foreach (var observer in observers.ToArray())
            {
                observer.OnNext(pari);
            }
        }

        /// <summary>
        /// 当这个点的内容发生变化时调用;
        /// </summary>
        public IDisposable Subscribe(IObserver<KeyValuePair<IntVector2, T>> observer)
        {
            if (observer != null && !observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);

        }

        private class Unsubscriber : IDisposable
        {
            public Unsubscriber(List<IObserver<KeyValuePair<IntVector2, T>>> observers, IObserver<KeyValuePair<IntVector2, T>> observer)
            {
                this.observers = observers;
                this.observer = observer;
            }

            List<IObserver<KeyValuePair<IntVector2, T>>> observers;
            IObserver<KeyValuePair<IntVector2, T>> observer;

            public void Dispose()
            {
                if(observer != null)
                    observers.Remove(observer);
            }
        }

    }

}
