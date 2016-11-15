using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace KouXiaGu.Map
{

    /// <summary>
    /// 地图保存和读取;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MapBlockEditIO<T> : DynaBlocksMap<MapBlockEdit<T>, T>
    {
        protected MapBlockEditIO(BlocksMapInfo info) :
            base(info)
        {
            
        }

        protected abstract string GetFullPrefabMapFilePath(ShortVector2 address);

        public override MapBlockEdit<T> Load(ShortVector2 address)
        {
            MapBlockEdit<T> block;
            string fullPrefabMapFilePath = GetFullPrefabMapFilePath(address);
            try
            {
                Dictionary<ShortVector2, T> map =
                    SerializeHelper.Deserialize_ProtoBuf<Dictionary<ShortVector2, T>>(fullPrefabMapFilePath);
                block = new MapBlockEdit<T>(map);
            }
            catch (FileNotFoundException)
            {
                block = new MapBlockEdit<T>();
            }
            return block;
        }

        public override bool LoadAsyn(ShortVector2 address, Action<MapBlockEdit<T>> onComplete, Action<Exception> onFail)
        {
            WaitCallback waitCallback = delegate
            {
                MapBlockEdit<T> mapBlock;
                try
                {
                    mapBlock = Load(address);
                    onComplete(mapBlock);
                }
                catch (Exception e)
                {
                    onFail(e);
                }
            };
            return ThreadPool.QueueUserWorkItem(waitCallback);
        }

        public override void Save(ShortVector2 address, MapBlockEdit<T> mapBlock)
        {
            Dictionary<ShortVector2, T> map = mapBlock.MapCollection;

            if (map.Count != 0)
            {
                string fullPrefabMapFilePath = GetFullPrefabMapFilePath(address);
                SerializeHelper.Serialize_ProtoBuf<Dictionary<ShortVector2, T>>(fullPrefabMapFilePath, map);
            }
            else
            {
                Debug.Log("未改变的地图,跳过保存地图块 :" + address.ToString());
            }
        }

        public override void SaveAsyn(ShortVector2 address, MapBlockEdit<T> mapBlock, Action onComplete, Action<Exception> onFail)
        {
            WaitCallback waitCallback = delegate
            {
                try
                {
                    Save(address, mapBlock);
                    onComplete();
                }
                catch (Exception e)
                {
                    onFail(e);
                }
            };
            ThreadPool.QueueUserWorkItem(waitCallback);
        }

    }

}
