using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class TerrainMap : ObservableDictionary<CubicHexCoord, TerrainNode>
    {
        
        /// <summary>
        /// 存储文件后缀名;
        /// </summary>
        public const string FILE_EXTENSION = ".mapp";

        static readonly TerrainMap emptyMap = new TerrainMap();


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
        public static void Write(string filePath, TerrainMap map)
        {
            filePath = Path.ChangeExtension(filePath, FILE_EXTENSION);
            ProtoBufExtensions.SerializeProtoBuf(filePath, map);
        }

        /// <summary>
        /// 从文件读取到;
        /// </summary>
        public static TerrainMap Read(string filePath)
        {
            filePath = Path.ChangeExtension(filePath, FILE_EXTENSION);
            return ProtoBufExtensions.DeserializeProtoBuf<TerrainMap>(filePath);
        }



        public TerrainMap() : base()
        {

        }


    }

}
