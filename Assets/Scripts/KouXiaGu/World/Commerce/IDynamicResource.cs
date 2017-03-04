using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{


    public interface IDynamicResource<T>
    {

        /// <summary>
        /// 优先级;
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// 需求;
        /// </summary>
        T Demand { get;  }

        /// <summary>
        /// 实际分配;
        /// </summary>
        T Practice { get; }

        /// <summary>
        /// 改变需求数目;
        /// </summary>
        void SetDemand(T number);

    }

}
