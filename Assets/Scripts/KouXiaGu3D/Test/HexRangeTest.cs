using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using KouXiaGu.Terrain3D;
using KouXiaGu.Grids;

namespace KouXiaGu.Test
{

    [DisallowMultipleComponent, ExecuteInEditMode]
    public class HexRangeTest : MonoBehaviour
    {
        [SerializeField]
        Transform parent;

        [SerializeField]
        GameObject textObject;

        [SerializeField]
        public int radius;

        [SerializeField]
        Dictionary<CubicHexCoord, GameObject> insObjects = new Dictionary<CubicHexCoord, GameObject>();

        void Start()
        {
            //insObjects = new Dictionary<CubicHexCoord, GameObject>();
        }

        [ContextMenu("块输出")]
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

        [ContextMenu("环")]
        void Raing()
        {
            foreach (var point in HexGrids.GetHexRing(CubicHexCoord.Zero, radius))
            {
                Instantiate(point);
            }
        }

        [ContextMenu("螺旋")]
        void Spiral()
        {
            foreach (var point in HexGrids.GetHexSpiral(CubicHexCoord.Zero, radius))
            {
                Instantiate(point);
            }
        }

        [ContextMenu("移除所有创建")]
        void Clear()
        {
            foreach (var item in insObjects.Values)
            {
                DestroyImmediate(item);
            }
            insObjects.Clear();
        }

        void Output(ShortVector2 coord)
        {
            foreach (var item in Terrain3D.TerrainData.GetBlockCover(coord))
            {
                this.Instantiate(item);
            }
        }

        void Instantiate(CubicHexCoord coord)
        {
            if (!insObjects.ContainsKey(coord))
            {
                Vector3 point = HexGrids.HexToPixel(coord);
                GameObject gt = Instantiate(textObject, point, Quaternion.identity, parent) as GameObject;
                gt.SetActive(true);
                Text t = gt.GetComponentInChildren<Text>();

                t.text = GetText(coord);

                insObjects.Add(coord, gt);
            }
        }

        string GetText(CubicHexCoord coord)
        {
            return coord.ToString()
                + "\n" + HexGrids.GetRadius(coord);
        }

    }

}
