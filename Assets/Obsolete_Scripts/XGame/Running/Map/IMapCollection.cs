using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XGame.Running.Map
{

    /// <summary>
    /// 地图结构;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMapDictionary<T> : IDictionary<IntVector2, T>, IEnumerable<T>
    {

        /// <summary>
        /// 获取到这个点的周围 元素(包括这个点的元素);
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        IEnumerable<T> GetEnumerable(IntVector2 position);

    }

}
