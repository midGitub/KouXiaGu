using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.RectMaps
{


    [DisallowMultipleComponent]
    public sealed class RectMapSceneController : SceneSington<RectMapSceneController>
    {
        RectMapSceneController()
        {
        }

        /// <summary>
        /// 当前游戏使用的地图数据;
        /// </summary>
        public GameMap Map { get; private set; }
    }
}
