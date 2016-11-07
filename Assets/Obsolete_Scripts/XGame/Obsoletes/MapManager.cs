//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//namespace XGame.GameRunning
//{

//    /// <summary>
//    /// 地图控制;
//    /// </summary>
//    public class MapManager : Manager<MapManager>, IGameSave
//    {

//        [SerializeField]
//        private CallOrder moduleType;

//        /// <summary>
//        /// 初始化次序;
//        /// </summary>
//        CallOrder ICallOrder.CallOrder { get { return moduleType; } }

//        /// <summary>
//        /// 地图信息;
//        /// </summary>
//        public MapInfo MapInfo { get; private set; }

//        protected override MapManager This
//        {
//            get{ return this; }
//        }

//        /// <summary>
//        /// 从全局控制获取到地图信息;
//        /// </summary>
//        private MapInfo LoadFormController
//        {
//           get { return MapController.GetInstance.MapInfo; }
//        }

//        void IGameO.GameStart()
//        {
//            return;
//        }

//        IEnumerator IGameSave.OnLoad(GameSaveInfo info, GameSaveData data)
//        {
//            MapInfo = data.MapInfo;
//            yield return AsyncHelper.WaitForFixedUpdate;
//        }

//        IEnumerator IGameSave.OnSave(GameSaveInfo info, GameSaveData data)
//        {
//            data.MapInfo = MapInfo;
//            yield return AsyncHelper.WaitForFixedUpdate;
//        }

//        IEnumerator IGameO.OnStart()
//        {
//            MapInfo = LoadFormController;
//            yield return AsyncHelper.WaitForFixedUpdate;
//        }

//    }

//}
