using System;
using System.Collections.Generic;

namespace KouXiaGu.World.Commerce
{

    /// <summary>
    /// 表示百分比,记录改变这个值的请求者;
    /// </summary>
    public class ProportionItems
    {

        public ProportionItems(float percent)
        {
            this.Proportion = percent;
            senderItems = new List<Item>();
        }

        /// <summary>
        /// 增减条目链表;
        /// </summary>
        List<Item> senderItems;

        /// <summary>
        /// 百分比 -max ~ max;
        /// </summary>
        public float Proportion { get; private set; }

        /// <summary>
        /// 发起变更的项目总数;
        /// </summary>
        public int Count
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
        public IDisposable Add(object sender, float increment)
        {
            var item = new CancelableItem(sender, increment, this);
            Proportion += increment;
            senderItems.Add(item);
            return item;
        }

        /// <summary>
        /// 移除这个条目;
        /// </summary>
        bool Remove(Item item)
        {
            bool isRemove = senderItems.Remove(item);

            if (isRemove)
            {
                Proportion -= item.Increment;
                return true;
            }

            return false;
        }


        public static implicit operator float(ProportionItems v)
        {
            return v.Proportion;
        }


        public class Item
        {
            public Item(object sender, float increment)
            {
                this.Sender = sender;
                this.Increment = increment;
            }

            public object Sender { get; private set; }
            public float Increment { get; private set; }

            public override string ToString()
            {
                return "[Sender:" + Sender.ToString() + "Increment:" + Increment + "]";
            }
        }

        public class CancelableItem : Item, IDisposable
        {
            public CancelableItem(object sender, float increment, ProportionItems percentage) : base(sender, increment)
            {
                this.Percentage = percentage;
            }

            public ProportionItems Percentage { get; private set; }

            public void Dispose()
            {
                if (Percentage != null)
                {
                    Percentage.Remove(this);
                    Percentage = null;
                }
            }
        }

    }

}
