using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D.Tests
{

    /// <summary>
    /// 将鼠标所指向地图节点的内容输出为文本;
    /// </summary>
    [DisallowMultipleComponent]
    public class MapContentTest : MonoBehaviour
    {

        [SerializeField]
        Text textObject;

        IDictionary<CubicHexCoord, TerrainNode> Data
        {
            get { return MapDataManager.Data.Data; }
        }

        void Update()
        {
            textObject.text = TextUpdate();

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                CubicHexCoord coord = LandformTrigger.MouseRayPointOrDefault().GetTerrainCubic();
                MapDataManager.Data.Road.Create(coord);
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                CubicHexCoord coord = LandformTrigger.MouseRayPointOrDefault().GetTerrainCubic();
                MapDataManager.Data.Road.Destroy(coord);
            }
        }


        string TextUpdate()
        {
            Vector3 mousePoint;
            if (LandformTrigger.TryGetMouseRayPoint(out mousePoint))
            {
                CubicHexCoord coord = mousePoint.GetTerrainCubic();
                TerrainNode node;

                if (Data.TryGetValue(coord, out node))
                {
                    return
                        "坐标:" + coord.ToString() +
                        "\n" + node.ToString();
                }
            }
            return "Empty Content";
        }

    }

}
