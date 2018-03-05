using JiongXiaGu.Collections;
using System;
using System.Collections.Generic;
using System.Threading;

namespace JiongXiaGu.Unity.Maps
{
    /// <summary>
    /// 地图抽象接口;
    /// </summary>
    public interface IMap<TKey> : IDictionary<TKey, MapNode>, IReadOnlyDictionary<TKey, MapNode>, IObservable<MapEvent<TKey>>
    {
        AddOrUpdateStatus AddOrUpdate(TKey key, MapNode value);
    }
}
