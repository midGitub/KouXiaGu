using System;
using System.Collections.Generic;

namespace KouXiaGu.World.Commerce
{

    /// <summary>
    /// 仓库产品腐坏效果;
    /// </summary>
    public class SpoilEffect : National
    {

        public SpoilEffect(Country belongToCountry) : base(belongToCountry)
        {
            rooms = new LinkedList<Unsubscriber>();
        }

        LinkedList<Unsubscriber> rooms;

        /// <summary>
        /// 记录这个库房,并且更新腐坏效果;
        /// </summary>
        public IDisposable OnCreated(IWareroom room)
        {
            var item = new Unsubscriber(this, room);
            var node = rooms.AddLast(item);
            item.Node = node;
            return item;
        }

        /// <summary>
        /// 每天更新;
        /// </summary>
        public void DayUpdate()
        {
            foreach (var room in rooms)
            {
                room.DayUpdate();
            }
        }

        class Unsubscriber : IDisposable
        {
            public Unsubscriber(SpoilEffect spoil, IWareroom room)
            {
                Spoil = spoil;
                Room = room;
                ProductInfo = spoil.ProductInfos[room.Product];
            }

            public LinkedListNode<Unsubscriber> Node { get; set; }
            public SpoilEffect Spoil { get; private set; }
            public IWareroom Room { get; private set; }
            public ProductInfo ProductInfo { get; private set; }

            LinkedList<Unsubscriber> rooms
            {
                get { return Spoil.rooms; }
            }

            public ProductSpoilInfo Info
            {
                get { return ProductInfo.Spoil; }
            }

            public void DayUpdate()
            {
                float spoilPercent = MathI.Clamp01(Info.SpoilPercent);
                int number = (int)(Room.Total * spoilPercent);
                Room.Remove(number);
            }

            void IDisposable.Dispose()
            {
                if (Spoil != null)
                {
                    rooms.Remove(Node);
                    Spoil = null;
                    Room = null;
                }
            }

        }

    }

}
