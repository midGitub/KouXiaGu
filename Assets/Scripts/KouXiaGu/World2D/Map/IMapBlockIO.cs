using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.World2D.Map
{

    /// <summary>
    /// 读取地图块资源的接口
    /// </summary>
    public interface IMapBlockIO<TBlock>
    {
        TBlock Load(RectCoord address);
        void Unload(RectCoord address, TBlock block);
    }

}
