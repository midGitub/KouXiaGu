using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.WorldEvents
{

    /// <summary>
    /// 增加资源产量;
    /// </summary>
    public class IncreaseResThroughput : ITownEffect
    {

        public IncreaseResThroughput(ResourceType type, float increment)
        {
            this.Type = type;
            this.Increment = increment;
        }


        /// <summary>
        /// 资源类型;
        /// </summary>
        public ResourceType Type { get; private set; }

        /// <summary>
        /// 增量;
        /// </summary>
        public float Increment { get; private set; }

        /// <summary>
        /// 是否为增长的?
        /// </summary>
        public bool IsIncrease
        {
            get { return Increment > 0; }
        }


        /// <summary>
        /// 启用;
        /// </summary>
        public void Enable(Town country)
        {
            
        }

        /// <summary>
        /// 停用;
        /// </summary>
        public void Disable(Town country)
        {

        }

    }

}
