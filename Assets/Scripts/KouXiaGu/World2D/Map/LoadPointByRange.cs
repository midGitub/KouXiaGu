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
        ShortVector2 loadRange = new ShortVector2(20, 20);
        /// <summary>
        /// 已经读取过的点;
        /// </summary>
        List<ShortVector2> loadedPoin = new List<ShortVector2>();
        /// <summary>
        /// 读取的接口;
        /// </summary>
        public IMapNodeIO<T> MapNodeIO { get; set; }

        /// <summary>
        /// 更新地图的中心点;
        /// </summary>
        public void UpdateCenterPoint(ShortVector2 mapPoint)
        {
            ShortVector2[] newBlock = GetBlock(mapPoint).ToArray();
            ShortVector2[] unloadPoints = loadedPoin.Except(newBlock).ToArray();
            ShortVector2[] loadPoints = newBlock.Except(loadedPoin).ToArray();

            Load(loadPoints);
            Unload(unloadPoints);
        }

        /// <summary>
        /// 读取到这些位置的资源;
        /// </summary>
        void Load(IEnumerable<ShortVector2> mapPoints)
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
        void Unload(IEnumerable<ShortVector2> mapPoints)
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
        public IEnumerable<ShortVector2> GetBlock(ShortVector2 centerPoint)
        {
            ShortVector2 southwestPoint = GetSouthwestPoint(centerPoint);
            ShortVector2 northeastPoint = GetNortheastPoint(centerPoint);

            return ShortVector2.Range(southwestPoint, northeastPoint);
        }

        /// <summary>
        /// 获取到西南角的点;
        /// </summary>
        ShortVector2 GetSouthwestPoint(ShortVector2 centerPoint)
        {
            centerPoint.x -= loadRange.x;
            centerPoint.y -= loadRange.y;
            return centerPoint;
        }

        /// <summary>
        /// 获取到东北角的点;
        /// </summary>
        ShortVector2 GetNortheastPoint(ShortVector2 centerPoint)
        {
            centerPoint.x += loadRange.x;
            centerPoint.y += loadRange.y;
            return centerPoint;
        }

    }

}
