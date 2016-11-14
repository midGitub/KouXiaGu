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
        void Save(ShortVector2 address, TMapBlock mapBlock);
        void SaveAsyn(ShortVector2 address, TMapBlock mapBlock, Action onComplete, Action<Exception> onFail);

        TMapBlock Load(ShortVector2 address);
        bool LoadAsyn(ShortVector2 address, Action<TMapBlock> onComplete, Action<Exception> onFail);
    }


}
