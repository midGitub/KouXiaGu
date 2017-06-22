using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World.Map;
using KouXiaGu.Resources;

namespace KouXiaGu.World
{

    [ConsoleMethodsClass]
    class DevelopmentTool
    {


        [ConsoleMethod("write_map", "输出地图到文件")]
        public static void WriteMap()
        {
            var worldInitializer = WorldSceneManager.WorldInitializer;
            if (worldInitializer != null && worldInitializer.IsCompleted)
            {
                IWorld world = worldInitializer.Result;
                var file = new MapFile();
                MapDataWriter mapDataSerializer = new MapDataWriter(new MapFile());
                world.WorldData.MapData.Write(mapDataSerializer);
                GameConsole.LogSuccessful("已输出地图文件到 " + file.GetFullPath());
                return;
            }
            else
            {
                GameConsole.LogError("游戏场景还未初始化完成;");
            }
        }

        [ConsoleMethod("archive_map", "将地图存档输出到临时目录")]
        public static void WriteMapArchive()
        {
            var worldInitializer = WorldSceneManager.WorldInitializer;
            if (worldInitializer != null && worldInitializer.IsCompleted)
            {
                IWorld world = worldInitializer.Result;
                var file = new MapArchiveFile(Resource.TempDirectoryPath);
                MapDataWriter mapDataSerializer = new MapDataWriter(new MapArchiveFile(Resource.TempDirectoryPath));
                world.WorldData.MapData.WriteArchivedData(mapDataSerializer);
                GameConsole.LogSuccessful("已输出地图存档文件到 " + file.GetFullPath());
            }
            else
            {
                GameConsole.LogError("游戏场景还未初始化完成;");
            }
        }
    }
}
