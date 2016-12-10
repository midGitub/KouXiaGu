using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;


namespace KouXiaGu.HexTerrain.MapProtoBuf
{

    /// <summary>
    /// 对 IBlockArchive 接口使用 ProtoBuf 方式保存的方法拓展;
    /// </summary>
    public static class BlockArchiveExtensions
    {

        /// <summary>
        /// 地图文件的后缀名;
        /// </summary>
        public const string fileSuffix = ".MAP";

        static readonly string searchPattern = string.Concat("?", fileSuffix);

        /// <summary>
        /// 将需要保存的地图块,保存到这个文件夹下;
        /// </summary>
        public static void Save<TP, T>(this IBlockArchive<TP, T> blockArchive, string directoryPath, FileMode fileMode)
        {
            IEnumerable<BlockArchive<TP, T>> blocks = blockArchive.Save();
            foreach (var block in blocks)
            {
                block.Save(directoryPath, fileMode);
            }
        }

        /// <summary>
        /// 保存所有地图到这个文件夹下;
        /// </summary>
        public static void SaveAll<TP, T>(this IBlockArchive<TP, T> blockArchive, string directoryPath, FileMode fileMode)
        {
            IEnumerable<BlockArchive<TP, T>> blocks = blockArchive.SaveAll();
            foreach (var block in blocks)
            {
                block.Save(directoryPath, fileMode);
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
        /// 读取这个目录下的所有地图文件;
        /// </summary>
        public static void LoadAll<TP, T>(this IBlockArchive<TP, T> blockArchive, string directoryPath)
        {

        }

        /// <summary>
        /// 从块文件读取到;
        /// </summary>
        public static BlockArchive<TP, T> Load<TP, T>(ShortVector2 coord, string directoryPath)
        {
            string filePath = coord.GetFilePath(directoryPath);
            BlockArchive<TP, T> block = SerializeHelper.DeserializeProtoBuf<BlockArchive<TP, T>>(filePath);
            return block;
        }

        /// <summary>
        /// 获取到这个目录下存在的所有地图块文件;
        /// </summary>
        public static string[] GetBlockFilePaths(string directoryPath)
        {
            return Directory.GetFiles(directoryPath, searchPattern);
        }




        /// <summary>
        /// 获取到保存到的文件路径;
        /// </summary>
        static string GetFilePath<TP, T>(this BlockArchive<TP, T> block, string directoryPath)
        {
            string filePath = Path.Combine(directoryPath, GetFileName(block));
            return filePath;
        }

        /// <summary>
        /// 获取到保存到的文件路径;
        /// </summary>
        static string GetFilePath(this ShortVector2 coord, string directoryPath)
        {
            string filePath = Path.Combine(directoryPath, GetFileName(coord));
            return filePath;
        }

        /// <summary>
        /// 获取到保存为的文件名;
        /// </summary>
        public static string GetFileName<TP, T>(BlockArchive<TP, T> block)
        {
            return GetFileName(block.Coord);
        }

        /// <summary>
        /// 获取到保存为的文件名;
        /// </summary>
        public static string GetFileName(ShortVector2 blockCoord)
        {
            string fileName = string.Concat(blockCoord.GetHashCode(), fileSuffix);
            return fileName;
        }

    }

}
