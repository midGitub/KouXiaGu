using System;
using System.Collections.Generic;

namespace KouXiaGu.World.Commerce
{

    /// <summary>
    /// 表示一个比例,并且记录改变这个值的请求者;
    /// </summary>
    public class ProportionItems
    {

        public ProportionItems(float proportion)
        {
            this.Proportion = proportion;
            senderItems = new LinkedList<Item>();
        }

        /// <summary>
        /// 增减条目链表;
        /// </summary>
        LinkedList<Item> senderItems;

        /// <summary>
        /// 百分比 -max ~ max;
        /// </summary>
        public float Proportion { get; private set; }

        /// <summary>
        /// 发起变更的项目总数;
        /// </summary>
        public int ItemCount
        {
            get { return senderItems.Count; }
        }

        /// <summary>
        /// 遍历所有条目;
        /// </summary>
        public IEnumerable<Item> Items
        {
            get { return senderItems; }
        }

        /// <summary>
        /// 增加或减少百分比;
        /// </summary>
        public IDisposable Add(IRequestor requestor, float increment)
        {
            return new CancelableItem(requestor, increment, this);
        }


        public static implicit operator float(ProportionItems v)
        {
            return v.Proportion;
        }


        public class Item
        {
            public Item(IRequestor requestor, float increment)
            {
                this.Requestor = requestor;
                this.Increment = increment;
            }

            public IRequestor Requestor { get; private set; }
            public float Increment { get; private set; }

            public override string ToString()
            {
                return "[Sender:" + Requestor.ToString() + "Increment:" + Increment + "]";
            }
        }

        class CancelableItem : Item, IDisposable
        {
            public CancelableItem(IRequestor requestor, float increment, ProportionItems percentage) :
                base(requestor, increment)
            {
                Percentage = percentage;
                node = senderItems.AddLast(this);
                Proportion += increment;
            }

            public ProportionItems Percentage { get; private set; }
            LinkedListNode<Item> node;

            LinkedList<Item> senderItems
            {
                get { return Percentage.senderItems; }
            }

            float Proportion
            {
                get { return Percentage.Proportion; }
                set { Percentage.Proportion = value; }
            }

            void IDisposable.Dispose()
            {
                if (node != null)
                {
                    senderItems.Remove(node);
                    node = null;
                }
            }

        }

    }

}
