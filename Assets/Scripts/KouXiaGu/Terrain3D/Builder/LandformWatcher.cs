using System;
using System.Collections.Generic;
using KouXiaGu.Grids;
using KouXiaGu.Collections;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 根据挂载物体所在位置创建附近地形;
    /// </summary>
    [DisallowMultipleComponent]
    public class LandformWatcher : MonoBehaviour
    {

        static bool isStart = false;
        static readonly List<LandformWatcher> watcherList = new List<LandformWatcher>();

        internal static void Initialize(LandformScene scene)
        {
            foreach (var item in watcherList)
            {
                item.StartUpdate(scene);
            }
        }


        LandformWatcher()
        {
        }

        /// <summary>
        /// 显示半径,在这个半径内的地形块会创建并显示;
        /// </summary>
        [SerializeField]
        RectCoord displayRadius = new RectCoord(2, 2);

        [SerializeField]
        int updateInterval = 60;

        int lastUpdateTime;
        LandformScene scene;

        RectGrid Grid
        {
            get { return ChunkInfo.ChunkGrid; }
        }

        void Awake()
        {
            enabled = false;
            watcherList.Add(this);
        }

        void StartUpdate(LandformScene scene)
        {
            this.scene = scene;
            SendDisplayCoords();
            enabled = true;
        }

        void Update()
        {
            lastUpdateTime++;
            if (lastUpdateTime >= updateInterval)
            {
                lastUpdateTime = 0;
                SendDisplayCoords();
            }
        }

        void OnValidate()
        {
            Normalize(ref displayRadius);
        }

        /// <summary>
        /// 使其符合要求;
        /// </summary>
        public void Normalize(ref RectCoord coord)
        {
            coord.x = MathI.Clamp(coord.x, coord.x, short.MaxValue);
            coord.y = MathI.Clamp(coord.y, coord.y, short.MaxValue);
        }

        void SendDisplayCoords()
        {
            IEnumerable<RectCoord> coords = GetDisplay(transform.position);
            scene.Display(coords);
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
