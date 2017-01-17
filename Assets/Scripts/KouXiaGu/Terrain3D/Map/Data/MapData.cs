using KouXiaGu.Collections;
using KouXiaGu.Grids;
using ProtoBuf;
using System.IO;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形地图;
    /// </summary>
    [ProtoContract]
    public class MapData : ObservableDictionary<CubicHexCoord, TerrainNode>
    {
        
        /// <summary>
        /// 存储文件后缀名;
        /// </summary>
        public const string FILE_EXTENSION = ".data";

        static readonly MapData emptyMap = new MapData();


        /// <summary>
        /// 输出一个空白的地图文件;
        /// </summary>
        public static void WriteEmpty(string filePath)
        {
            Write(filePath, emptyMap);
        }

        /// <summary>
        /// 输出到文件;
        /// </summary>
        public static void Write(string filePath, MapData map)
        {
            filePath = Path.ChangeExtension(filePath, FILE_EXTENSION);
            ProtoBufExtensions.Serialize(filePath, map);
        }

        /// <summary>
        /// 从文件读取到;
        /// </summary>
        public static MapData Read(string filePath)
        {
            filePath = Path.ChangeExtension(filePath, FILE_EXTENSION);
            return ProtoBufExtensions.Deserialize<MapData>(filePath);
        }


        public MapData() : base()
        {

        }




    }

}
