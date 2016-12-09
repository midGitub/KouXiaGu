using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using KouXiaGu.HexTerrain;

namespace KouXiaGu.Test
{


    public class HexRangeTest : MonoBehaviour
    {
        [SerializeField]
        Transform parent;

        [SerializeField]
        GameObject textObject;

        HashSet<CubicHexCoord> insCoord;

        void Awake()
        {
            insCoord = new HashSet<CubicHexCoord>();
        }

        [ContextMenu("输出到场景")]
        void Output()
        {
            Output(ShortVector2.Zero);
            Output(ShortVector2.Up);
            Output(ShortVector2.Down);
            Output(ShortVector2.Left);
            Output(ShortVector2.Right);
            Output(ShortVector2.Down + ShortVector2.Left);
            Output(ShortVector2.Down + ShortVector2.Right);
            Output(ShortVector2.Up + ShortVector2.Left);
            Output(ShortVector2.Up + ShortVector2.Right);
        }

        void Output(ShortVector2 coord)
        {

            foreach (var item in TerrainBlock.GetBlockCover(coord))
            {
                if (!insCoord.Contains(item))
                {
                    Vector3 point = HexGrids.HexToPixel(item);
                    GameObject gt = Instantiate(textObject, point, Quaternion.identity, parent) as GameObject;
                    Text t = gt.GetComponentInChildren<Text>();
                    t.text = item.ToString();
                    insCoord.Add(item);
                }
            }

        }


    }

}
