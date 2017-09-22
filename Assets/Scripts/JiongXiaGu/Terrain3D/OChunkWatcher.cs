using JiongXiaGu.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JiongXiaGu.Terrain3D
{

    public abstract class OChunkWatcher : MonoBehaviour, IChunkWatcher<RectCoord>
    {
        protected OChunkWatcher()
        {
        }

        /// <summary>
        /// 显示半径,在这个半径内的地形块会创建并显示;
        /// </summary>
        [SerializeField]
        RectCoord displayRadius = new RectCoord(2, 2);
        Vector3 position;

        RectGrid Grid
        {
            get { return LandformChunkInfo.ChunkGrid; }
        }

        void Update()
        {
            position = transform.position;
        }

        void OnValidate()
        {
            Normalize(ref displayRadius);
        }

        void Normalize(ref RectCoord coord)
        {
            coord.x = MathXiaGu.Clamp(coord.x, coord.x, short.MaxValue);
            coord.y = MathXiaGu.Clamp(coord.y, coord.y, short.MaxValue);
        }

        public void UpdateDispaly(ICollection<RectCoord> dispalyCoords)
        {
            var center = Grid.GetCoord(position);
            IEnumerable<RectCoord> coords = RectCoord.Range(center, displayRadius.x, displayRadius.y);
            foreach (var coord in coords)
            {
                dispalyCoords.Add(coord);
            }
        }
    }
}
