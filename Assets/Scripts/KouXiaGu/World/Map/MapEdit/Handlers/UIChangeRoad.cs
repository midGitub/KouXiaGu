using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.OperationRecord;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.World.Map.MapEdit
{

    [DisallowMultipleComponent]
    public class UIChangeRoad : UIMapEditHandler
    {
        const string messageFormat = "Road:[{0}]";
        bool existRoad;
        int roadType;
        public Toggle roadExistToggle;

        public override string GetMessage()
        {
            return string.Format(messageFormat, existRoad);
        }

        public override bool Contrast(IMapEditHandler handler)
        {
            return handler is UIChangeRoad;
        }

        public override IVoidable Execute(IEnumerable<EditMapNode> nodes)
        {
            IWorldComplete world = WorldSceneManager.World;
            if (world != null)
            {
                MapData map = world.WorldData.MapData.data;
                foreach (var node in nodes)
                {
                    node.Value.Road.Update(map, new NodeRoadInfo(roadType));
                }
            }
            return null;
        }
    }
}
