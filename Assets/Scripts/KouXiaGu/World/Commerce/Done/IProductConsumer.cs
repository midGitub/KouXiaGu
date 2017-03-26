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

}
