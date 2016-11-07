using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XGame.Running.Map
{

    /// <summary>
    /// 游戏地图控制;
    /// </summary>
    [DisallowMultipleComponent]
    public class MapController : Controller<MapController>, IGameArchive
    {
        protected MapController() { }

        [SerializeField]
        private CallOrder callOrder = CallOrder.Static;

        /// <summary>
        /// 地图信息;
        /// </summary>
        [SerializeField]
        private MapInfo mapInfo;

        protected override MapController This { get { return this; } }
        CallOrder ICallOrder.CallOrder { get { return callOrder; } }

        /// <summary>
        /// 地图信息;
        /// </summary>
        public MapInfo MapInfo
        {
            get{ return mapInfo; }
            set
            {
#if UNITY_EDITOR
                if (StateController.GetInstance.GameState > StatusType.WaitStart)
                {
                    Debug.LogWarning("在运行状态尝试改变地图信息!");
                    return;
                }
#endif
                mapInfo = value;
            }
        }

        IEnumerator IGameLoad.OnStart()
        {
            yield break;
        }

        IEnumerator IGameArchive.OnLoad(GameSaveInfo info, GameSaveData data)
        {
            this.mapInfo = info.mapInfo;
            yield break;
        }

        IEnumerator IGameArchive.OnSave(GameSaveInfo info, GameSaveData data)
        {
            info.mapInfo = this.mapInfo;
            yield break;
        }

        IEnumerator IGameLoad.OnClear()
        {
            yield break;
        }


#if UNITY_EDITOR

        [ContextMenu("地图信息")]
        private void Test_Log()
        {
            Debug.Log(mapInfo.ToString());
        }

#endif

    }

}
