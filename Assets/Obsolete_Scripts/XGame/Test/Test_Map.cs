using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XGame.Running.Map;

namespace XGame.Test
{

    [DisallowMultipleComponent]
    public class Test_Map : TestUIBase
    {

        private MapController mapController;

        private MapInfo mapInfo { get { return mapController.MapInfo; } }

        protected override void Awake()
        {
            base.Awake();
            mapController = ControllerHelper.GameController.GetComponentInChildren<MapController>();
        }

        protected override string Log()
        {
            string str;
            IntVector2 position = (IntVector2)Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);

            str = "当前位置 :" + position;
            str += "  超出地图 :" + mapInfo.IsOuterBoundary(position);
            return str;
        }
    }

}
