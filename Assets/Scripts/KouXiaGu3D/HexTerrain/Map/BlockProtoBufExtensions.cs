using System;
using System.IO;
using System.Collections.Generic;


namespace KouXiaGu.Terrain3D
{


    /// <summary>
    /// 对 IBlockArchive 接口使用 ProtoBuf 方式保存的方法拓展;
    /// </summary>
    public static class BlockProtoBufExtensions
    {

        /// <summary>
        /// 地图文件的后缀名;
        /// </summary>
        public const string fileSuffix = ".MAPP";

        /// <summary>
        /// 搜索文件时的通配符;
        /// </summary>
        static readonly string searchPattern = string.Concat("*", fileSuffix);


        /// <summary>
        /// 将需要保存的地图块,保存到这个文件夹下;
        /// </summary>
        public static void Save<TP, T>(this IBlockArchive<TP, T> blockArchive, string directoryPath, FileMode fileMode)
        {
            IEnumerable<BlockArchive<TP, T>> blocks = blockArchive.GetArchives();
            foreach (var block in blocks)
            {
                lock (blockArchive.SyncWriteRoot)
                {
                    block.Save(directoryPath, fileMode);
                }
            }
        }

        /// <summary>
        /// 保存所有地图到这个文件夹下;
        /// </summary>
        public static void SaveAll<TP, T>(this IBlockArchive<TP, T> blockArchive, string directoryPath, FileMode fileMode)
        {
            IEnumerable<BlockArchive<TP, T>> blocks = blockArchive.GetArchiveAll();
            foreach (var block in blocks)
            {
                lock (blockArchive.SyncWriteRoot)
                {
                    block.Save(directoryPath, fileMode);
                }
            }
        }

        /// <summary>
        /// 保存到这个文件夹下;
        /// </summary>
        public static void Save<TP, T>(this BlockArchive<TP, T> block, string directoryPath, FileMode fileMode)
        {
            string filePath = block.GetFilePath(directoryPath);
            SerializeHelper.SerializeProtoBuf(filePath, block, fileMode);
        }



        /// <summary>
        /// 读取这个目录下的所有地图文件,保存到地图结构内;
        /// 注意地图内容是否存在重复,这个函数不进行重复块检查;
        /// </summary>
        public static void Load<TP, T>(this IBlockArchive<TP, T> blockArchive, string directoryPath)
        {
            string[] paths = GetFilePaths(directoryPath);
            foreach (var path in paths)
            {
                var block = Load<TP, T>(path);
                blockArchive.AddArchives(block);
            }
        }

        /// <summary>
        /// 读取这个目录下的所有地图文件,保存到地图结构内;
        /// 若地图内已经存在这个地图块,则跳过这个块的读取;
        /// </summary>
        public static void LoadContrastive<TP, T>(this IBlockArchive<TP, T> blockArchive, string directoryPath)
        {
            string[] paths = GetFilePaths(directoryPath);
            foreach (var path in paths)
            {
                if (!blockArchive.Contains(path))
                {
                    var block = Load<TP, T>(path);
                    blockArchive.AddArchives(block);
                }
            }
        }

        /// <summary>
        /// 从文件读取到;
        /// </summary>
        public static BlockArchive<TP, T> Load<TP, T>(ShortVector2 coord, string directoryPath)
        {
            string filePath = coord.GetFilePath(directoryPath);
            return Load<TP, T>(filePath);
        }

        /// <summary>
        /// 从文件读取到;
        /// </summary>
        public static BlockArchive<TP, T> Load<TP, T>(string filePath)
        {
            BlockArchive<TP, T> block = SerializeHelper.DeserializeProtoBuf<BlockArchive<TP, T>>(filePath);
            return block;
        }



        /// <summary>
        /// 确认这个地图结构是否已经读取了这个块节点(检查文件名);
        /// </summary>
        public static bool Contains<TP, T>(this IBlockArchive<TP, T> blockArchive, string filePath)
        {
            var coord = FilePathToCoord(filePath);
            return blockArchive.Contains(coord);
        }



        /// <summary>
        /// 获取到这个目录下存在的所有地图块文件标号;
        /// </summary>
        public static ShortVector2[] GetCoords(string directoryPath)
        {
            string[] paths = GetFilePaths(directoryPath);
            ShortVector2[] coords = new ShortVector2[paths.Length];
            int index = 0;

            foreach (var path in paths)
            {
                coords[index] = FilePathToCoord(path);
            }

            return coords;
        }

        /// <summary>
        /// 获取到这个目录下存在的所有地图块文件;
        /// </summary>
        public static string[] GetFilePaths(string directoryPath)
        {
            return Directory.GetFiles(directoryPath, searchPattern);
        }


        /// <summary>
        /// 获取到保存到的文件路径;
        /// </summary>
        static string GetFilePath<TP, T>(this BlockArchive<TP, T> block, string directoryPath)
        {
            string filePath = Path.Combine(directoryPath, CoordToFileName(block));
            return filePath;
        }

        /// <summary>
        /// 获取到保存到的文件路径;
        /// </summary>
        static string GetFilePath(this ShortVector2 coord, string directoryPath)
        {
            string filePath = Path.Combine(directoryPath, CoordToFileName(coord));
            return filePath;
        }

        /// <summary>
        /// 获取到保存为的文件名;
        /// </summary>
        public static string CoordToFileName<TP, T>(BlockArchive<TP, T> block)
        {
            return CoordToFileName(block.Coord);
        }

        /// <summary>
        /// 文件路径转换为块坐标;
        /// </summary>
        public static ShortVector2 FilePathToCoord(string filePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            int hashCode = Convert.ToInt32(fileName);
            ShortVector2 coord = ShortVector2.HashCodeToVector(hashCode);
            return coord;
        }

        /// <summary>
        /// 转换为保存到的文件名;
        /// </summary>
        public static string CoordToFileName(ShortVector2 blockCoord)
        {
            string fileName = string.Concat(blockCoord.GetHashCode(), fileSuffix);
            return fileName;
        }

    }

}
