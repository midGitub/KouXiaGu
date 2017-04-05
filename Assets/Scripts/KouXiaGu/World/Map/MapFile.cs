//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;

//namespace KouXiaGu.World.Map
//{


//    public class MapFile
//    {
//        static MapFile()
//        {
//            infoReader = new MapInfoReader();
//            mapReader = new ProtoMapReader();
//        }

//        static readonly MapInfoReader infoReader;
//        static readonly MapReader mapReader;

//        public MapInfo Info { get; private set; }
//        public string InfoPath { get; private set; }

//        public MapInfoReader InfoReader
//        {
//            get { return infoReader; }
//        }

//        public MapReader MapReader
//        {
//            get { return mapReader; }
//        }

//        public string InfoFileSearchPattern
//        {
//            get { return "*" + InfoReader.FileExtension; }
//        }

//        public MapFile(string filePath)
//        {
//            ReadInfoFile(filePath);
//        }

//        public MapFile(string dirPath, SearchOption searchOption)
//        {
//            var filePaths = Directory.GetFiles(dirPath, InfoFileSearchPattern, searchOption);

//            if (filePaths.Length == 0)
//                throw new FileNotFoundException();

//            string filePath = filePaths[0];
//            ReadInfoFile(filePath);
//        }

//        void ReadInfoFile(string filePath)
//        {
//            InfoPath = filePath;
//            Info = InfoReader.Read(filePath);
//        }

//        public string GetMapDataFilePath()
//        {
//            return Path.ChangeExtension(InfoPath, MapReader.FileExtension);
//        }

//        public Map ReadMap()
//        {
//            string filePath = GetMapDataFilePath();
//            Map map = MapReader.Read(filePath);
//            map.File = this;
//            return map;
//        }

//        public void WriteMap(Map data)
//        {
//            if (data.File != this)
//                throw new ArgumentException();

//            string filePath = GetMapDataFilePath();
//            MapReader.Write(filePath, data);
//        }

//        public void WriteInfo(MapInfo data)
//        {
//            string filePath = InfoPath;
//            InfoReader.Write(filePath, data);
//        }

//    }

//}
