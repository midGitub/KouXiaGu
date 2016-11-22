using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 读取地图块文件的接口
    /// </summary>
    public interface IMapBlockIO<T>
    {
        T Load(ShortVector2 address);
        void Unload(ShortVector2 address, T block);
    }

}
