using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形创建到场景;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TerrainCreater : MonoBehaviour
    {

        TerrainCreater() { }

        /// <summary>
        /// 最小显示半径,在这个半径内的地形块会创建并显示;
        /// </summary>
        [SerializeField]
        RectCoord minRadius;

        /// <summary>
        /// 最大缓存半径,超出这个半径的地形块贴图将会销毁;
        /// </summary>
        [SerializeField]
        RectCoord maxRadius;

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
            RectCoord center = TerrainData.ChunkGrid.GetCoord(position);
            IEnumerable<RectCoord> pp = GetDisplayCoords(center);

            foreach (var item in pp)
            {
                BakeRequest.Create(item);
            }
        }

        /// <summary>
        /// 获取到需要显示到场景的坐标;
        /// </summary>
        IEnumerable<RectCoord> GetDisplayCoords(RectCoord center)
        {
            return RectCoord.Range(center, minRadius.x);
        }

        [ContextMenu("渲染所有")]
        void BakingAll()
        {
            foreach (var item in RectCoord.Range(RectCoord.Self, 4))
            {
                try
                {
                    BakeRequest.Create(item);
                }
                catch (IndexOutOfRangeException)
                {
                    continue;
                }
            }
        }

        static TerrainCreater creater;

        public static void Load()
        {
            if (creater != null)
                throw new Exception("已经进行游戏?");

            creater = GameObject.FindObjectOfType<TerrainCreater>();
            creater.enabled = true;
        }

        public static void Unload()
        {
            creater.enabled = false;
            creater = null;
        }

    }

}
