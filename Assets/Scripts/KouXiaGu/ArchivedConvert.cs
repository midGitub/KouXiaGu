using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Map;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 不同类型的存档互相转换方法定义;
    /// </summary>
    public static class ArchivedConvert
    {

        /// <summary>
        /// 获取到主角的地图位置;
        /// </summary>
        public static IntVector2 GetProtagonistMapPosition(this ArchivedExpand archived)
        {
            Vector2 pretagonistPosition = archived.Character.ProtagonistTransfrom.Position;
            IntVector2 mapPosition = GameMap.GetInstance.PlaneToMapPoint(pretagonistPosition);
            return mapPosition;
        }

    }

}
