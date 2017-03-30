using System;
using System.Collections.Generic;
using System.Linq;

namespace KouXiaGu.World
{

    public interface IProducible
    {
        /// <summary>
        /// 进行生产更新;
        /// </summary>
        void OnProduce();
    }

    /// <summary>
    /// 负责生产项目更新;
    /// </summary>
    public class ProductProduction
    {
        public ProductProduction()
        {
            items = new LinkedList<IProducible>();
        }

        LinkedList<IProducible> items;

        /// <summary>
        /// 更新所有生产项目;
        /// </summary>
        public void Update()
        {
            var itemsArray = items.ToArray();
            foreach (var item in itemsArray)
            {
                item.OnProduce();
            }
        }

        public IDisposable Add(IProducible item)
        {
            return new Canceler(this, item);
        }

        class Canceler : IDisposable
        {
            public Canceler(ProductProduction parent, IProducible item)
            {
                Parent = parent;
                node = Items.AddLast(item);
            }

            public ProductProduction Parent { get; private set; }
            LinkedListNode<IProducible> node;

            LinkedList<IProducible> Items
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
