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
    [Serializable]
    public abstract class RectChunkGuider : MonoBehaviour, IChunkGuider<RectCoord>
    {
        /// <summary>
        /// 显示半径,在这个半径内的地形块会创建并显示;
        /// </summary>
        [SerializeField]
        RectCoord displayRadius = new RectCoord(2, 2);

        protected abstract RectGrid Grid { get; }

        void OnValidate()
        {
            Normalize(ref displayRadius);
        }

        void Normalize(ref RectCoord coord)
        {
            coord.x = MathXiaGu.Clamp(coord.x, coord.x, short.MaxValue);
            coord.y = MathXiaGu.Clamp(coord.y, coord.y, short.MaxValue);
        }

        /// <summary>
        /// 获取到需要显示的坐标;
        /// </summary>
        public IReadOnlyCollection<RectCoord> GetPointsToDisplay()
        {
            var center = Grid.GetCoord(transform.position);
            return RectCoord.Range(center, displayRadius.x, displayRadius.y).ToArray();
        }
    }
}
