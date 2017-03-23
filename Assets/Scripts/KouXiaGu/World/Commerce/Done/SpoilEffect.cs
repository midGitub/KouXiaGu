using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{

    /// <summary>
    /// 产品腐坏效果;
    /// </summary>
    public class SpoilEffect : National
    {

        public SpoilEffect(Country belongToCountry) : base(belongToCountry)
        {
            rooms = new List<IWareroom>();
        }

        List<IWareroom> rooms;

        public void OnCreated(IWareroom room)
        {
            throw new NotImplementedException();
        }

        public void OnDestroy(IWareroom room)
        {
            throw new NotImplementedException();
        }

        public void DayUpdate()
        {

        }

        class Wareroom
        {

            public IWareroom Room { get; private set; }

        }

    }

}
