using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World2D.Map
{

    public interface IMapNodeIO<T>
    {
        T Load(ShortVector2 mapPoint);
        void Unload(ShortVector2 mapPoint);
    }

}
