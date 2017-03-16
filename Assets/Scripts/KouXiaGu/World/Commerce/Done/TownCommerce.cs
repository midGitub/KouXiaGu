using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{


    public class TownCommerce
    {

        public TownCommerce()
        {
            Production = new Production();
        }

        public Production Production { get; private set; }

    }

}
