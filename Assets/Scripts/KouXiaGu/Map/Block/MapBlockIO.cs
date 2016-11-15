﻿using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace KouXiaGu.Map
{

    /// <summary>
    /// 预制地图读取,存档地图读取保存;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MapBlockIO<T> : DynaBlocksMap<MapBlock<T>, T>
    {

        protected MapBlockIO(BlocksMapInfo info) :
            base(info)
        {
            
        }

        protected abstract string GetFullPrefabMapFilePath(ShortVector2 address);
        protected abstract string GetFullArchiveTempFilePath(ShortVector2 address);

        #region Load

        public override MapBlock<T> Load(ShortVector2 address)
        {
            Dictionary<ShortVector2, T> prefabMap;
            Dictionary<ShortVector2, T> archiveMap;
            MapBlock<T> mapBlock;

            prefabMap = LoadPrefab(address);

            if (TryLoadArchive(address, out archiveMap))
            {
                mapBlock = new MapBlock<T>(prefabMap, archiveMap);
            }
            else
            {
                mapBlock = new MapBlock<T>(prefabMap);
            }

            return mapBlock;
        }

        private Dictionary<ShortVector2, T> LoadPrefab(ShortVector2 address)
        {
            string fullPrefabMapFilePath = GetFullPrefabMapFilePath(address);
            Dictionary<ShortVector2, T> prefabMap = Load(address, fullPrefabMapFilePath);
            return prefabMap;
        }

        private Dictionary<ShortVector2, T> Load(ShortVector2 address, string fullFilePath)
        {
            var dictionary = SerializeHelper.Deserialize_ProtoBuf<Dictionary<ShortVector2, T>>(fullFilePath);
            return dictionary;
        }

        private bool TryLoadArchive(ShortVector2 address, out Dictionary<ShortVector2, T> dictionary)
        {
            string fullArchiveTempFilePath = GetFullArchiveTempFilePath(address);
            return TryLoad(address, fullArchiveTempFilePath, out dictionary);
        }

        private bool TryLoad(ShortVector2 address, string fullFilePath, out Dictionary<ShortVector2, T> dictionary)
        {
            try
            {
                dictionary = Load(address, fullFilePath);
                return true;
            }
            catch (Exception e)
            {
                Debug.Log("读取存档地图" + address.ToString() + "未成功;\n" + e);
            }

            dictionary = default(Dictionary<ShortVector2, T>);
            return false;
        }

        public override bool LoadAsyn(ShortVector2 address, Action<MapBlock<T>> onComplete, Action<Exception> onFail)
        {
            WaitCallback waitCallback = delegate
            {
                MapBlock<T> mapBlock;
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

        #endregion

        #region Save

        public override void Save(ShortVector2 address, MapBlock<T> mapBlock)
        {
            Dictionary<ShortVector2, T> archiveMap = mapBlock.ArchiveMap;

            if (archiveMap.Count != 0)
            {
                string fullArchiveTempFilePath = GetFullArchiveTempFilePath(address);
                SerializeHelper.Serialize_ProtoBuf<Dictionary<ShortVector2, T>>(fullArchiveTempFilePath, archiveMap);
            }
            else
            {
                Debug.Log("未改变的地图,跳过保存地图块 :" + address.ToString());
            }
        }

        public override void SaveAsyn(ShortVector2 address, MapBlock<T> mapBlock, Action onComplete, Action<Exception> onFail)
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

        #endregion

    }

}