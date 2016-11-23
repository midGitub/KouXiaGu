using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World2D.Map
{

    public interface IMapNodeIO<T>
    {
        T Load(IntVector2 mapPoint);
        void Unload(IntVector2 mapPoint);
    }

}
