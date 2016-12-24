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
            Output(RectCoord.Self);
            Output(RectCoord.North);
            Output(RectCoord.South);
            Output(RectCoord.West);
            Output(RectCoord.East);
            Output(RectCoord.South + RectCoord.West);
            Output(RectCoord.South + RectCoord.East);
            Output(RectCoord.North + RectCoord.West);
            Output(RectCoord.North + RectCoord.East);
        }

        [ContextMenu("环")]
        void Raing()
        {
            foreach (var point in CubicHexCoord.Ring(CubicHexCoord.Self, radius))
            {
                Instantiate(point);
            }
        }

        [ContextMenu("螺旋")]
        void Spiral()
        {
            foreach (var point in CubicHexCoord.Spiral(CubicHexCoord.Self, radius))
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

        void Output(RectCoord coord)
        {
            foreach (var item in Terrain3D.TerrainChunk.GetChunkCover(coord))
            {
                this.Instantiate(item);
            }
        }

        void Instantiate(CubicHexCoord coord)
        {
            if (!insObjects.ContainsKey(coord))
            {
                Vector3 point = GridConvert.Grid.GetPixel(coord);
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
                + "\n" + CubicHexCoord.GetRadius(coord);
        }

    }

}
