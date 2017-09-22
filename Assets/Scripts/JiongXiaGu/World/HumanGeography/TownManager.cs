using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu.World.HumanGeography
{

    /// <summary>
    /// 城镇管理;
    /// </summary>
    public class TownManager
    {
        public TownManager()
        {
            towns = new Dictionary<int, TownRange>();
        }

        readonly Dictionary<int, TownRange> towns;

        public TownRange Create(IWorld world)
        {
            throw new NotImplementedException();
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }
    }
}
