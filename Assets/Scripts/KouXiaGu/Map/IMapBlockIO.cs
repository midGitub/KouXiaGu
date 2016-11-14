using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Map
{

    /// <summary>
    /// 负责对地图内容进行保存和读取的接口;
    /// </summary>
    /// <typeparam name="TMapBlock">地图块类型</typeparam>
    /// <typeparam name="T">地图块保存的内容</typeparam>
    public interface IMapBlockIO<TMapBlock, T>
    {
        string Save(TMapBlock mapPaging);
        void SaveAsyn(TMapBlock mapPaging);

        TMapBlock Load(ShortVector2 address);
        bool TryLoad(ShortVector2 address, out TMapBlock mapPaging);

        void Delete(ShortVector2 address);
    }


}
