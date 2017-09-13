using KouXiaGu.Concurrent;
using KouXiaGu.Grids;
using KouXiaGu.World.RectMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KouXiaGu.RectTerrain
{

    /// <summary>
    /// 地形创建范围提供者;
    /// </summary>
    public class LandformGuider
    {
        public LandformGuider(WorldMap map, LandformBuilder builder)
        {
            ChangedRecorder = new DictionaryChangedRecorder<RectCoord, MapNode>();
            unsubscriber = map.ObservableMap.Subscribe(ChangedRecorder);
        }

        IDisposable unsubscriber;
        public DictionaryChangedRecorder<RectCoord, MapNode> ChangedRecorder { get; private set; }
        public LandformBuilder Builder { get; private set; }

        public void UpdateInOtherThread()
        {
            RecordeItem<RectCoord, MapNode> recorde;
            while (ChangedRecorder.TryDequeue(out recorde))
            {
                RectCoord chunkPos;
                if (IsWithinRange(recorde.Key, out chunkPos))
                {
                    Builder.Update(chunkPos);
                }
            }
        }

        /// <summary>
        /// 该坐标是否在创建范围内?
        /// </summary>
        /// <param name="point">地图坐标;</param>
        /// <param name="chunkPos">地形块坐标;</param>
        public bool IsWithinRange(RectCoord point, out RectCoord chunkPos)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// 地形创建器;
    /// </summary>
    [Serializable]
    public class LandformBuilder
    {
        public LandformBuilder(WorldMap map)
        {
            Map = map;
        }

        public WorldMap Map { get; private set; }

        /// <summary>
        /// 创建的地形块;
        /// </summary>
        public Dictionary<RectCoord, LandformChunkData> Chunks { get; private set; }

        public LandformChunkData Create(RectCoord chunkPos)
        {
            throw new NotImplementedException();
        }

        public LandformChunkData Update(RectCoord chunkPos)
        {
            throw new NotImplementedException();
        }

        public LandformChunkData Destroy(RectCoord chunkPos)
        {
            throw new NotImplementedException();
        }
    }


    public class LandformChunkData : IRequest
    {
        public LandformChunkData()
        {
        }

        /// <summary>
        /// 块坐标;
        /// </summary>
        public RectCoord Point { get; private set; }

        /// <summary>
        /// 地形块实例;
        /// </summary>
        public LandformChunkRenderer Chunk { get; private set; }

        /// <summary>
        /// 块状态;
        /// </summary>
        public ChunkState State { get; private set; }

        /// <summary>
        /// 是否完成?
        /// </summary>
        public bool IsCompleted { get; private set; }

        void IRequest.Operate()
        {
            throw new NotImplementedException();
        }
    }
}
