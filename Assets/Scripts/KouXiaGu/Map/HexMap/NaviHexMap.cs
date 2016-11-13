using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Map.Navigation;
using KouXiaGu.Map.HexMap;
using UniRx;

namespace KouXiaGu.Map
{

    /// <summary>
    /// 为游戏实现的导航地图;
    /// </summary>
    [Serializable]
    public class NaviHexMap : INavMap<NavNode, Navigator>, IBuildGameInThread, IArchiveInThread
    {
        private NaviHexMap()
        {
            //navHexMap = new DynamicMapDictionary<NavNode>();
        }

        private DynamicMapDictionary<NavNode> navHexMap;

        #region INavigationMap<TMover>

        IEnumerable<KeyValuePair<ShortVector2, NavNode>> INavMap<NavNode, Navigator>.GetAround(ShortVector2 position)
        {
            //NavNode item;
            //IntVector2 vectorPosition;
            //foreach (HexDirection direction in Enum.GetValues(typeof(HexDirection)))
            //{
            //    vectorPosition = HexMapConvert.GetVector(position, direction) + position;
            //    if (navHexMap.TryGetValue(vectorPosition, out item))
            //    {
            //        yield return new KeyValuePair<IntVector2, NavNode>(vectorPosition, item);
            //    }
            //}
            throw new NotImplementedException();
        }

        NavNode INavMap<NavNode, Navigator>.GetAt(ShortVector2 position)
        {
            //NavNode navNode;
            //if (navHexMap.TryGetValue(position, out navNode))
            //    return navNode;
            //else
            //    throw new KeyNotFoundException("尝试获取到不存在的节点!" + position.ToString());

            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// 在创建游戏时需要初始化的内容;
        /// </summary>
        void IThreadInitialize<BuildGameData>.Initialize(
            BuildGameData item, ICancelable cancelable, Action<Exception> onError, Action runningDoneCallBreak)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 在保存游戏时需要初始化的内容;
        /// </summary>
        void IThreadInitialize<ArchivedGroup>.Initialize(
            ArchivedGroup item, ICancelable cancelable, Action<Exception> onError, Action runningDoneCallBreak)
        {
            throw new NotImplementedException();
        }

    }

}
