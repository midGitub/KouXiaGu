using System;
using System.Collections.Generic;
using System.Linq;

namespace KouXiaGu.World.Commerce
{

    public class Factory
    {

        /// <summary>
        /// 产出;
        /// </summary>
        public IEnumerable<ProductContainer> Output
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// 建造此建筑的前提;
        /// </summary>
        public bool Precondition(Town town)
        {
            throw new NotImplementedException();
        }


    }


    public interface IFactory
    {
        /// <summary>
        /// 产出;
        /// </summary>
        IEnumerable<ProductContainer> Output { get; }
    }

    public class TownProduction
    {



    }


}
