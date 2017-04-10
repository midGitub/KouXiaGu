﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 读取游戏地图方法类;
    /// </summary>
    public abstract class MapReader
    {
        static readonly MapReader defaultReader = new ProtoMapReader();

        /// <summary>
        /// 默认的读写方式;
        /// </summary>
        public static MapReader DefaultReader
        {
            get { return defaultReader; }
        }

        public virtual string FileSearchPattern
        {
            get { return "*" + FileExtension; }
        }

        public virtual string SearchPattern
        {
            get { return "*" + FileExtension; }
        }

        public abstract string FileExtension { get; }
        public abstract MapData Read(string filePath);
        public abstract void Write(string filePath, MapData data);
    }

    /// <summary>
    /// 使用 ProtoBuf 的方式读取游戏地图;
    /// </summary>
    public class ProtoMapReader : MapReader
    {
        public override string FileExtension
        {
            get { return ".map"; }
        }

        public override MapData Read(string filePath)
        {
            MapData data = ProtoBufExtensions.Deserialize<MapData>(filePath);
            return data;
        }

        public override void Write(string filePath, MapData data)
        {
            ProtoBufExtensions.Serialize(filePath, data);
        }
    }

}