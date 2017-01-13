using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    [DisallowMultipleComponent]
    public class TerrainWatcher : MonoBehaviour
    {

        TerrainWatcher() { }

        /// <summary>
        /// x 显示半径,在这个半径内的地形块会创建并显示;
        /// y 最大半径,超出这个半径内的将会销毁;
        /// </summary>
        [SerializeField]
        RectCoord radius = new RectCoord(2, 2);

        /// <summary>
        /// 中心点;
        /// </summary>
        Vector3 position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        void Update()
        {
            RectCoord center = TerrainChunk.ChunkGrid.GetCoord(position);
            HashSet<RectCoord> displayCoords = new HashSet<RectCoord>(GetDisplayCoords(center));

            foreach (var item in displayCoords)
            {
                TerrainCreater.CreateChunk(item);
            }
        }

        /// <summary>
        /// 获取到需要显示到场景的坐标;
        /// </summary>
        IEnumerable<RectCoord> GetDisplayCoords(RectCoord center)
        {
            return RectCoord.Range(center, radius.x, radius.y);
        }

    }

}
