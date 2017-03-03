using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{

    /// <summary>
    /// 生产线;
    /// </summary>
    public class Workshop : IEquatable<Workshop>
    {

        const int DefaultLevel = 1;
        const int DefaultProducerCount = 0;


        public Workshop(Product type)
        {
            if (type == null)
                throw new ArgumentNullException();

            this.Type = type;
            SetLevel(DefaultLevel);
            SetProducer(DefaultProducerCount);
        }


        /// <summary>
        /// 生产资源类型;
        /// </summary>
        public Product Type { get; set; }


        /// <summary>
        /// 车间级别,改变最大生产者数目;
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// 最大生产者数目;
        /// </summary>
        public int MaxProducerCount { get; private set; }

        /// <summary>
        /// 生产者数目; 小于或等于 最大生产者数目
        /// </summary>
        public int ProducerCount { get; private set; }

        /// <summary>
        /// 产出;
        /// </summary>
        public int Produce
        {
            get { return ProducerCount * Type.Productivity; }
        }


        /// <summary>
        /// 设置 车间级别;
        /// </summary>
        public void SetLevel(int level)
        {
            this.Level = level;
            this.MaxProducerCount = (ushort)(Level * Type.MaxProducerLevel);
            this.ProducerCount = Math.Min(ProducerCount, MaxProducerCount);
        }

        /// <summary>
        /// 设置 生产者数目;
        /// </summary>
        public void SetProducer(ushort number)
        {
            if (number > MaxProducerCount || number < 0)
                throw new ArgumentOutOfRangeException();

            this.ProducerCount = number;
        }


        public bool Equals(Workshop other)
        {
            return other.Type == this.Type;
        }

        public override bool Equals(object obj)
        {
            var item = obj as Workshop;

            if (item == null)
                return false;

            return Equals(item);
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }

    }

}
