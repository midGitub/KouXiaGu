using System.Collections.Generic;
using System.IO;
using KouXiaGu.Collections;
using KouXiaGu.Grids;
using ProtoBuf;

namespace KouXiaGu.Terrain3D
{

    [ProtoContract]
    public class ArchiveMapData : DictionaryArchiver<CubicHexCoord, TerrainNode>
    {
        /// <summary>
        /// 存储文件后缀名;
        /// </summary>
        public const string FILE_EXTENSION = ".data";

        /// <summary>
        /// 输出到文件;
        /// </summary>
        public static void Write(string filePath, ArchiveMapData map)
        {
            filePath = Path.ChangeExtension(filePath, FILE_EXTENSION);
            ProtoBufExtensions.Serialize(filePath, map);
        }

        /// <summary>
        /// 从文件读取到;
        /// </summary>
        public static ArchiveMapData Read(string filePath)
        {
            filePath = Path.ChangeExtension(filePath, FILE_EXTENSION);
            return ProtoBufExtensions.Deserialize<ArchiveMapData>(filePath);
        }


        public ArchiveMapData() : base() { }
        public ArchiveMapData(IDictionary<CubicHexCoord, TerrainNode> dictionary) : base(dictionary) { }
        public ArchiveMapData(int capacity) : base(capacity) { }

    }

}
