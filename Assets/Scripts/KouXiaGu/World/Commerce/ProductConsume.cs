using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World
{

    public interface IConsumable
    {
        void OnConsume();
    }

    /// <summary>
    /// 产品消耗;
    /// </summary>
    public class ProductConsumption
    {
        public ProductConsumption()
        {
            items = new LinkedList<IConsumable>();
        }

        LinkedList<IConsumable> items;

        /// <summary>
        /// 更新所有消耗项目;
        /// </summary>
        public void Update()
        {
            var itemsArray = items.ToArray();
            foreach (var item in itemsArray)
            {
                item.OnConsume();
            }
        }

        public IDisposable Add(IConsumable item)
        {
            return new Canceler(this, item);
        }

        class Canceler : IDisposable
        {
            public Canceler(ProductConsumption parent, IConsumable item)
            {
                Parent = parent;
                node = Items.AddLast(item);
            }

            public ProductConsumption Parent { get; private set; }
            LinkedListNode<IConsumable> node;

            LinkedList<IConsumable> Items
            {
                get { return Parent.items; }
            }

            public void Dispose()
            {
                if (node != null)
                {
                    Items.Remove(node);
                    node = null;
                }
            }
        }

    }

}
