using KouXiaGu.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    public abstract class CubicHexCoordGuider : MonoBehaviour, IGuider<CubicHexCoord>
    {
        /// <summary>
        /// 显示半径,在这个半径内的地形块会创建并显示;
        /// </summary>
        [SerializeField]
        int radius = 3;

        public abstract CubicHexGrid Grid { get; }

        void OnValidate()
        {
            Normalize();
        }

        void Normalize()
        {
            radius = MathXiaGu.Clamp(radius, 1, int.MaxValue);
        }

        public IEnumerable<CubicHexCoord> GetPointsToDisplay()
        {
            var center = Grid.GetCubic(transform.position);
            return CubicHexCoord.Spiral_in(center, radius);
        }
    }
}
