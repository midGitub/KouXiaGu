using System;

namespace KouXiaGu.WorldEvents
{

    /// <summary>
    /// 资源生产;
    /// </summary>
    public class Production
    {

        Production()
        {
            Stock = 0f;
            Capacity = 0f;
        }

        public Production(ResourceType type) : this()
        {
            this.Type = type;
        }


        /// <summary>
        /// 资源类型;
        /// </summary>
        public ResourceType Type { get; private set; }

        /// <summary>
        /// 存量;
        /// </summary>
        public float Stock { get; private set; }

        /// <summary>
        /// 最大容量;
        /// </summary>
        public float Capacity { get; private set; }


        /// <summary>
        /// 改变存量;
        /// </summary>
        public void ChangeStock(float number)
        {
            this.Stock = Math.Min(this.Stock + number, this.Capacity);
        }

        /// <summary>
        /// 改变容量;
        /// </summary>
        public void ChangeCapacity(float number)
        {
            const float minCapacity = 0;
            this.Capacity = Math.Max(this.Capacity + number, minCapacity);
        }

    }

}
