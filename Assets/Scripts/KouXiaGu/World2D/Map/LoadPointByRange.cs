using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World2D.Map
{

    /// <summary>
    /// 按范围读取点内容;
    /// </summary>
    [Serializable]
    public abstract class LoadPointByRange<T>
    {
        protected LoadPointByRange() { }

        public LoadPointByRange(IMapNodeIO<T> mapNodeIO)
        {
            this.MapNodeIO = mapNodeIO;
        }

        /// <summary>
        /// 地图加载的范围;
        /// </summary>
        [SerializeField]
        IntVector2 loadRange = new IntVector2(20, 20);
        /// <summary>
        /// 已经读取过的点;
        /// </summary>
        List<IntVector2> loadedPoin = new List<IntVector2>();
        /// <summary>
        /// 读取的接口;
        /// </summary>
        public IMapNodeIO<T> MapNodeIO { get; set; }

        /// <summary>
        /// 更新地图的中心点;
        /// </summary>
        public void UpdateCenterPoint(IntVector2 mapPoint)
        {
            IntVector2[] newBlock = GetBlock(mapPoint).ToArray();
            IntVector2[] unloadPoints = loadedPoin.Except(newBlock).ToArray();
            IntVector2[] loadPoints = newBlock.Except(loadedPoin).ToArray();

            Load(loadPoints);
            Unload(unloadPoints);
        }

        /// <summary>
        /// 读取到这些位置的资源;
        /// </summary>
        void Load(IEnumerable<IntVector2> mapPoints)
        {
            foreach (var point in mapPoints)
            {
                MapNodeIO.Load(point);
                loadedPoin.Add(point);
            }
        }

        /// <summary>
        /// 卸载这些区域的资源;
        /// </summary>
        void Unload(IEnumerable<IntVector2> mapPoints)
        {
            foreach (var point in mapPoints)
            {
                MapNodeIO.Unload(point);
                loadedPoin.Remove(point);
            }
        }

        /// <summary>
        /// 获取到需要读取的点;
        /// </summary>
        public IEnumerable<IntVector2> GetBlock(IntVector2 centerPoint)
        {
            IntVector2 southwestPoint = GetSouthwestPoint(centerPoint);
            IntVector2 northeastPoint = GetNortheastPoint(centerPoint);

            return IntVector2.Range(southwestPoint, northeastPoint);
        }

        /// <summary>
        /// 获取到西南角的点;
        /// </summary>
        IntVector2 GetSouthwestPoint(IntVector2 centerPoint)
        {
            centerPoint.x -= loadRange.x;
            centerPoint.y -= loadRange.y;
            return centerPoint;
        }

        /// <summary>
        /// 获取到东北角的点;
        /// </summary>
        IntVector2 GetNortheastPoint(IntVector2 centerPoint)
        {
            centerPoint.x += loadRange.x;
            centerPoint.y += loadRange.y;
            return centerPoint;
        }

    }

}
