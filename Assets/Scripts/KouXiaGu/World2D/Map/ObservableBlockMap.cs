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
    /// <typeparam name="TBlock">地图块类型</typeparam>
    public class ObservableBlockMap<T, TBlock> : IMap<IntVector2, T>
        where T : struct
        where TBlock : IMap<ShortVector2, T>
    {
        protected ObservableBlockMap() { }

        public ObservableBlockMap(ShortVector2 partitionSizes)
        {
            blockMap = new BlockMap<TBlock>(partitionSizes);
        }

        BlockMap<TBlock> blockMap;
        NodeChangingReporter nodeChangingReporter = new NodeChangingReporter();

        public BlockMap<TBlock> BlockMap
        {
            get { return blockMap; }
        }

        public IObservable<KeyValuePair<IntVector2, T>> observeChanges
        {
            get { return nodeChangingReporter; }
        }

        public IEnumerable<KeyValuePair<IntVector2, T>> NodePair
        {
            get
            {
                foreach (var block in blockMap.BlocksPair)
                {
                    foreach (var node in block.Value.NodePair)
                    {
                        IntVector2 position = blockMap.AddressToPosition(block.Key, node.Key);
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

                nodeChangingReporter.NodeDataUpdate(position, value);
            }
        }

        public void Add(IntVector2 position, T item)
        {
            ShortVector2 realPosition;
            TBlock block = TransformToBlock(position, out realPosition);
            block.Add(realPosition, item);

            nodeChangingReporter.NodeDataUpdate(position, item);
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
            ShortVector2 address = blockMap.PlanePointToAddress(position, out realPosition);

            if (blockMap.TryGetValue(address, out block))
            {
                return block;
            }
            throw BlockNotFoundException(address);

        }

        /// <summary>
        /// 实质清除块内容;
        /// </summary>
        void IMap<IntVector2, T>.Clear()
        {
            blockMap.Clear();
        }

        /// <summary>
        /// 返回地图块错误信息;
        /// </summary>
        BlockNotFoundException BlockNotFoundException(ShortVector2 address)
        {
            return new BlockNotFoundException(address.ToString() + "地图块未载入!");
        }

        /// <summary>
        /// 当新加入点,或者点内容发生变化时进行通知;
        /// </summary>
        class NodeChangingReporter : IObservable<KeyValuePair<IntVector2, T>>
        {
            public NodeChangingReporter() { }

            List<IObserver<KeyValuePair<IntVector2, T>>> observers = new List<IObserver<KeyValuePair<IntVector2, T>>>();

            public void NodeDataUpdate(IntVector2 position, T worldNode)
            {
                if (observers.Count != 0)
                {
                    KeyValuePair<IntVector2, T> pari = new KeyValuePair<IntVector2, T>(position, worldNode);
                    foreach (var observer in observers.ToArray())
                    {
                        observer.OnNext(pari);
                    }
                }
            }

            public IDisposable Subscribe(IObserver<KeyValuePair<IntVector2, T>> observer)
            {
                if (observer == null)
                    throw new NullReferenceException();

                if (!observers.Contains(observer))
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
                    observers.Remove(observer);
                }
            }

        }

    }

}
