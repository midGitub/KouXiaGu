using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.HumanGeography
{

    /// <summary>
    /// 城镇管理;
    /// </summary>
    public class TownManager
    {
        public TownManager()
        {
            towns = new Dictionary<int, Town>();
        }

        readonly Dictionary<int, Town> towns;

        public Town Create(IWorld world)
        {
            throw new NotImplementedException();
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }
    }
}
