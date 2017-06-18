using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World.Commerce
{

    /// <summary>
    /// 城镇生产信息;
    /// </summary>
    public class TownProduction
    {

        public List<IProductionItem> Items { get; private set; }
    }

    /// <summary>
    /// 生产条目;
    /// </summary>
    public interface IProductionItem
    {
        /// <summary>
        /// 描述信息;
        /// </summary>
        string Description { get; }

        /// <summary>
        /// 图标信息;
        /// </summary>
        Texture Icon { get; }

        /// <summary>
        /// 更新库存信息;
        /// </summary>
        void UpdateStock(IWorld world, Stock stock);
    }
}
