using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{

    public interface IProductConsumer
    {
        /// <summary>
        /// 当满足条件时调用;
        /// </summary>
        void OnEnough();

        /// <summary>
        /// 当不满足条件时调用;
        /// </summary>
        void OnNotEnough();
    }

    /// <summary>
    /// 产品消耗;
    /// </summary>
    public class ProductConsume
    {

        public ProductConsume(ProductWarehouse warehouse)
        {
            Warehouse = warehouse;
            consumersList = new LinkedList<Consume>();
        }

        public ProductWarehouse Warehouse { get; private set; }
        LinkedList<Consume> consumersList;

        public int Count
        {
            get { return consumersList.Count; }
        }

        internal IEnumerable<IProductConsumer> Senders
        {
            get { return consumersList.Select(item => item.Sender); }
        }

        public IDisposable Add(IProductConsumer sender, Container<Product> product)
        {
            return new ConsumeProduct(this, sender, product);
        }

        public IDisposable Add(IProductConsumer sender, Container<ProductCategorie> categorie)
        {
            return new ConsumeCategorie(this, sender, categorie);
        }

        public void Update()
        {
            var consumersArray = consumersList.ToArray();
            foreach (var consumer in consumersArray)
            {
                consumer.Update();
            }
        }

        abstract class Consume : IDisposable
        {
            public Consume(ProductConsume parent, IProductConsumer sender)
            {
                Parent = parent;
                Sender = sender;
                node = ConsumersList.AddLast(this);
            }

            public ProductConsume Parent { get; private set; }
            public IProductConsumer Sender { get; private set; }
            LinkedListNode<Consume> node;

            LinkedList<Consume> ConsumersList
            {
                get { return Parent.consumersList; }
            }

            public ProductWarehouse Warehouse
            {
                get { return Parent.Warehouse; }
            }

            public abstract void Update();

            void IDisposable.Dispose()
            {
                if (node != null)
                {
                    ConsumersList.Remove(node);
                    node = null;
                }
            }
        }

        /// <summary>
        /// 消耗具体产品;
        /// </summary>
        class ConsumeProduct : Consume
        {
            public ConsumeProduct(ProductConsume parent, IProductConsumer sender, Container<Product> product) :
                base(parent, sender)
            {
                Product = product;
                room = Warehouse.FindOrCreate(sender, product.Product);
            }

            public Container<Product> Product { get; private set; }
            IWareroom room;

            public override void Update()
            {
                if (room.Remove(Product.Number))
                    Sender.OnEnough();
                else
                    Sender.OnNotEnough();
            }

        }

        /// <summary>
        /// 消耗产品类型;
        /// </summary>
        class ConsumeCategorie : Consume
        {
            public ConsumeCategorie(ProductConsume parent, IProductConsumer sender, Container<ProductCategorie> product) :
               base(parent, sender)
            {
                Product = product;
                room = Warehouse.FindOrCreate(sender, product.Product);
            }

            public Container<ProductCategorie> Product { get; private set; }
            ICategorieWareroom room;

            public override void Update()
            {
                if (room.Remove(Product.Number))
                    Sender.OnEnough();
                else
                    Sender.OnNotEnough();
            }

        }

    }

}
