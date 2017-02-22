using System;

namespace KouXiaGu.WorldEvents
{

    /// <summary>
    /// 资源;
    /// </summary>
    [Obsolete]
    public class Resource
    {

        public Resource()
        {
            Stock = 0f;
            Capacity = 0f;
            Throughput = 0f;
            ThroughputPercentage = 1f;
            Consume = 0f;
            ConsumePercentage = 1f;
        }


        /// <summary>
        /// 存量;
        /// </summary>
        public float Stock { get; private set; }

        /// <summary>
        /// 容量;
        /// </summary>
        public float Capacity { get; private set; }

        /// <summary>
        /// 总产量,每次更新增加的数量;
        /// </summary>
        public float Throughput { get; private set; }

        /// <summary>
        /// 生产效率;
        /// </summary>
        public float ThroughputPercentage { get; private set; }

        /// <summary>
        /// 消耗量;
        /// </summary>
        public float Consume { get; private set; }

        /// <summary>
        /// 消耗效率;
        /// </summary>
        public float ConsumePercentage { get; private set; }


        /// <summary>
        /// 真实的产量;
        /// </summary>
        public float RealThroughput
        {
            get { return Throughput * ThroughputPercentage; }
        }

        /// <summary>
        /// 真实的消耗量;
        /// </summary>
        public float RealConsume
        {
            get { return Consume * ConsumePercentage; }
        }

        /// <summary>
        /// 盈余;
        /// </summary>
        public float Surplus
        {
            get { return RealThroughput - RealConsume; }
        }


        /// <summary>
        /// 增加或减少存量;
        /// </summary>
        public virtual void AddStock(object sender, float number)
        {
            this.Stock = Math.Min(this.Stock + number, this.Capacity);
        }

        /// <summary>
        /// 增加或减少容量;
        /// </summary>
        public virtual void AddCapacity(object sender, float number)
        {
            const float minCapacity = 0;
            this.Capacity = Math.Max(this.Capacity + number, minCapacity);
        }

        /// <summary>
        /// 增加或减少总产量;
        /// </summary>
        public virtual void AddThroughput(object sender, float number)
        {
            this.Throughput += number;
        }

        /// <summary>
        /// 增加或减少生产效率;
        /// </summary>
        public virtual void AddThroughputPercentage(object sender, float number)
        {
            const float minPercentage = 0f;
            this.ThroughputPercentage = Math.Max(this.ThroughputPercentage + number, minPercentage);
        }

        /// <summary>
        /// 增加或减少消耗量;
        /// </summary>
        public virtual void AddConsume(object sender, float number)
        {
            this.Consume += number;
        }

        /// <summary>
        /// 增加或减少消耗效率;
        /// </summary>
        public virtual void AddConsumePercentage(object sender, float number)
        {
            const float minPercentage = 0f;
            this.ConsumePercentage = Math.Max(this.ConsumePercentage + number, minPercentage);
        }


        /// <summary>
        /// 周期性更新;
        /// </summary>
        public void OnUpdate()
        {
            AddStock(this, Surplus);
        }


    }

}
