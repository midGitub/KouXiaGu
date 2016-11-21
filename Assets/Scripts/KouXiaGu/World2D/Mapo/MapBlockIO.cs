
#define DETAILED_DEBUG

using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Text;

namespace KouXiaGu.World2D
{

    public static class MapBlockIO
    {

        /// <summary>
        /// 检索文件方法;
        /// </summary>
        public static string GetMapSearchPattern(this IMapBlockInfo info)
        {
            return info.AddressPrefix + "*";
        }

        /// <summary>
        /// 获取到地图块名;
        /// </summary>
        public static string GetMapBlockName(this IMapBlockInfo info, ShortVector2 address)
        {
            return info.AddressPrefix + address.GetHashCode();
        }

        /// <summary>
        /// 获取到完整的预制地图文件路径;
        /// </summary>
        public static string GetFullPrefabMapFilePath(this IMapBlockInfo info, ShortVector2 address)
        {
            string blockName = info.GetMapBlockName(address);
            string fullPrefabMapFilePath = Path.Combine(info.FullPrefabMapDirectoryPath, blockName);
            return fullPrefabMapFilePath;
        }

        /// <summary>
        /// 获取到完整的存档缓存文件路径;
        /// </summary>
        public static string GetFullArchiveTempFilePath(this IMapBlockInfo info, ShortVector2 address)
        {
            string blockName = info.GetMapBlockName(address);
            string fullArchiveTempFilePath = Path.Combine(info.FullArchiveTempDirectoryPath, blockName);
            return fullArchiveTempFilePath;
        }


        /// <summary>
        /// 保存这个结构到这个目录;
        /// </summary>
        public static void SaveMapBlock<T>(string fullFilePath, Dictionary<ShortVector2, T> mapBlock)
        {
            SerializeHelper.Serialize_ProtoBuf(fullFilePath, mapBlock);
        }

        /// <summary>
        ///  保存这个结构到这个目录,若元素小于 0 则不保存;
        /// </summary>
        public static void SaveMapBlockOrNot<T>(string fullFilePath, Dictionary<ShortVector2, T> mapBlock)
        {
            if (mapBlock.Count > 0)
            {
                SaveMapBlock(fullFilePath, mapBlock);
            }
        }

        /// <summary>
        /// 保存到预制地图;
        /// </summary>
        public static void SavePrefabMapBlockOrNot<T>(this IMapBlockInfo info, ShortVector2 blockAddress, MapBlock<T> mapBlock)
            where T : struct
        {
            string fullPrefabMapFilePath = info.GetFullPrefabMapFilePath(blockAddress);
            SaveMapBlockOrNot(fullPrefabMapFilePath, mapBlock.Archive());
        }

        /// <summary>
        /// 保存到存档缓存;
        /// </summary>
        public static void SaveArchiveMapBlockOrNot<T>(this IMapBlockInfo info, ShortVector2 blockAddress, MapBlock<T> mapBlock)
             where T : struct
        {
            string fullArchiveTempFilePath = info.GetFullArchiveTempFilePath(blockAddress);
            SaveMapBlockOrNot(fullArchiveTempFilePath, mapBlock.Archive());
        }




        /// <summary>
        /// 获取到这个路径的地图块;
        /// </summary>
        public static Dictionary<ShortVector2, T> LoadMapBlock<T>(string fullFilePath)
        {
            var dictionary = SerializeHelper.Deserialize_ProtoBuf<Dictionary<ShortVector2, T>>(fullFilePath);
            return dictionary;
        }

        /// <summary>
        /// 尝试获取到地图块;
        /// </summary>
        public static bool TryLoadMapBlock<T>(string fullFilePath, out Dictionary<ShortVector2, T> mapBlock)
        {
            try
            {
                mapBlock = LoadMapBlock<T>(fullFilePath);
                return true;
            }
            catch
            {
                mapBlock = default(Dictionary<ShortVector2, T>);
                return false;
            }
        }

        /// <summary>
        /// 尝试获取到这个区域的存档缓存地图块;
        /// </summary>
        public static bool TryLoadArchiveMapBlock<T>(this IMapBlockInfo info, ShortVector2 blockAddress, out Dictionary<ShortVector2, T> archiveMap)
        {
            string fullArchiveTempFilePath = info.GetFullArchiveTempFilePath(blockAddress);
            return TryLoadMapBlock(fullArchiveTempFilePath, out archiveMap);
        }

        /// <summary>
        /// 尝试获取到这个区域的预制地图块;
        /// </summary>
        public static bool TryLoadPrefabMapBlock<T>(this IMapBlockInfo info, ShortVector2 blockAddress, out Dictionary<ShortVector2, T> archiveMap)
        {
            string fullPrefabMapFilePath = info.GetFullPrefabMapFilePath(blockAddress);
            return TryLoadMapBlock(fullPrefabMapFilePath, out archiveMap);
        }

        /// <summary>
        /// 尝试获取到这个区域的完整地图块信息;
        /// </summary>
        public static MapBlock<T> LoadMapBlock<T>(this IMapBlockInfo info, ShortVector2 blockAddress)
            where T : struct
        {
            Dictionary<ShortVector2, T> prefabMap;
            Dictionary<ShortVector2, T> archiveMap;
            MapBlock<T> mapBlock;

            if (info.TryLoadPrefabMapBlock(blockAddress, out prefabMap))
            {
                if (info.TryLoadArchiveMapBlock(blockAddress, out archiveMap))
                {
#if DETAILED_DEBUG
                    Debug.Log(blockAddress + "找到预制地图 和 存档地图");
#endif
                    mapBlock = new MapBlock<T>(prefabMap, archiveMap);
                }
                else
                {
#if DETAILED_DEBUG
                    Debug.Log(blockAddress + "找到预制地图");
#endif
                    mapBlock = new MapBlock<T>(prefabMap, archiveMap);
                }
            }
            else
            {
                if (info.TryLoadArchiveMapBlock(blockAddress, out archiveMap))
                {
#if DETAILED_DEBUG
                    Debug.Log(blockAddress + "找到存档地图");
#endif
                    mapBlock = new MapBlock<T>(prefabMap, archiveMap);
                }
                else
                {
#if DETAILED_DEBUG
                    Debug.Log(blockAddress + "未找到地图");
#endif
                    mapBlock = new MapBlock<T>(prefabMap, archiveMap);
                }
            }
            return mapBlock;
        }


        /// <summary>
        /// 将地图文件复制到目标目录;
        /// </summary>
        public static void MapFileCopyTo(this IMapBlockInfo info, string sourceDirectoryPath, string destDirectoryPath, bool overwrite)
        {
            FileHelper.CopyDirectory(sourceDirectoryPath, destDirectoryPath, info.GetMapSearchPattern(), overwrite);
        }

        /// <summary>
        /// 将缓存的地图文件复制到目标目录;
        /// </summary>
        public static void ArchiveMapCopyTo(this IMapBlockInfo info, string destDirectoryPath, bool overwrite)
        {
            FileHelper.CopyDirectory(info.FullArchiveTempDirectoryPath, destDirectoryPath, info.GetMapSearchPattern(), overwrite);
        }

        /// <summary>
        /// 删除这个文件夹内的地图文件;
        /// </summary>
        public static void DeleteMapFile(this IMapBlockInfo info, string directoryPath)
        {
            FileHelper.DeleteFileInDirectory(directoryPath, info.GetMapSearchPattern());
        }


        /// <summary>
        /// 合并两个地图块(2 覆盖 1)输出到文件,若只存在一个则直接移动到,若两个都不存在则返回异常;
        /// </summary>
        public static void CombineMapBlock<T>(string filePath1, string filePath2, string outputFilePath)
        {
            Dictionary<ShortVector2, T> blockMap1;
            Dictionary<ShortVector2, T> blockMap2;

            if (TryLoadMapBlock(filePath1, out blockMap1))
            {
                if (TryLoadMapBlock(filePath2, out blockMap2))
                {
                    blockMap1.AddOrReplace(blockMap2);
                    SaveMapBlockOrNot(outputFilePath, blockMap1);
                }
                else
                {
                    File.Copy(filePath1, outputFilePath, true);
                }
            }
            else
            {
                if (TryLoadMapBlock(filePath2, out blockMap2))
                {
                    File.Copy(filePath2, outputFilePath, true);
                }
                else
                {
                    throw new FileNotFoundException();
                }
            }
        }


    }

}
