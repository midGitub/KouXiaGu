using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.World.Commerce
{

    public class ShopManager
    {
        public ShopManager()
        {
        }

        /// <summary>
        /// 场景中激活的商店;
        /// </summary>
        public IDictionary<CubicHexCoord, Shop> ActivatedShops { get; private set; }


    }

    public class Shop
    {
        public Shop()
        {
        }

        public CubicHexCoord Position { get; private set; }
        public IEnumerable<Shop> Neighbors { get; private set; }

        public IEnumerable<Shop> FindNeighbors()
        {
            throw new NotImplementedException();
        }
    }
}
