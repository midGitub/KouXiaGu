using KouXiaGu.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 矩形坐标显示;
    /// </summary>
    public abstract class RectCoordGuider : MonoBehaviour, IGuider<RectCoord>
    {
        /// <summary>
        /// 显示半径,在这个半径内的地形块会创建并显示;
        /// </summary>
        [SerializeField]
        int radius = 3;

        protected abstract RectGrid Grid { get; }

        void OnValidate()
        {
            Normalize();
        }

        void Normalize()
        {
            radius = MathXiaGu.Clamp(radius, 1, int.MaxValue);
        }

        /// <summary>
        /// 获取到需要显示的坐标;
        /// </summary>
        public IEnumerable<RectCoord> GetPointsToDisplay()
        {
            var center = Grid.GetCoord(transform.position);
            return RectCoord.Spiral_in(center, radius).ToArray();
        }
    }
}
