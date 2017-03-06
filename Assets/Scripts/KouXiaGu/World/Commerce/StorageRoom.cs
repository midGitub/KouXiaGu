using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{



    public abstract class StorageRoom
    {

        public const int EmptyObserverCount = 0;


        public StorageRoom()
        {
            this.ObserverCount = EmptyObserverCount;
        }


        /// <summary>
        /// 登记数目;
        /// </summary>
        public int ObserverCount { get; private set; }

        /// <summary>
        /// 房间是被引用?
        /// </summary>
        public virtual bool ExistsReference
        {
            get { return EmptyObserverCount == ObserverCount; }
        }


        /// <summary>
        /// 登记占用;
        /// </summary>
        public IDisposable Subscribe()
        {
            ObserverCount++;
            return new Unsubscriber(this);
        }
        
        class Unsubscriber : IDisposable
        {
            public Unsubscriber(StorageRoom storageRoom)
            {
                this.StorageRoom = storageRoom;
                IsRomve = false;
            }


            public StorageRoom StorageRoom { get; private set; }
            public bool IsRomve { get; private set; }


            public void Dispose()
            {
                if (!IsRomve)
                {
                    StorageRoom.ObserverCount--;
                    IsRomve = true;
                }
            }

        }

    }

}
