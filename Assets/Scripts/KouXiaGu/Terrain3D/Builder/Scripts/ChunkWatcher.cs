using KouXiaGu.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{


    public class ChunkWatcher : MonoBehaviour
    {
        protected ChunkWatcher()
        {
        }

        /// <summary>
        /// 显示半径,在这个半径内的地形块会创建并显示;
        /// </summary>
        [SerializeField]
        RectCoord displayRadius = new RectCoord(2, 2);

        RectGrid Grid
        {
            get { return ChunkInfo.ChunkGrid; }
        }

        void OnValidate()
        {
            Normalize(ref displayRadius);
        }

        public IEnumerable<RectCoord> GetDispaly()
        {
            return GetDisplay(transform.position);
        }

        /// <summary>
        /// 使其符合要求;
        /// </summary>
        void Normalize(ref RectCoord coord)
        {
            coord.x = MathI.Clamp(coord.x, coord.x, short.MaxValue);
            coord.y = MathI.Clamp(coord.y, coord.y, short.MaxValue);
        }

        /// <summary>
        /// 获取到需要显示到场景的坐标;
        /// </summary>
        IEnumerable<RectCoord> GetDisplay(Vector3 pos)
        {
            var center = Grid.GetCoord(pos);
            IEnumerable<RectCoord> coords = RectCoord.Range(center, displayRadius.x, displayRadius.y);
            return coords;
        }
    }

}
