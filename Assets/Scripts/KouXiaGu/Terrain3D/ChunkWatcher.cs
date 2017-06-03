using KouXiaGu.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    public abstract class ChunkWatcher : MonoBehaviour, IChunkWatcher<RectCoord>
    {
        protected ChunkWatcher()
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
            get { return ChunkInfo.ChunkGrid; }
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
            coord.x = MathI.Clamp(coord.x, coord.x, short.MaxValue);
            coord.y = MathI.Clamp(coord.y, coord.y, short.MaxValue);
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
