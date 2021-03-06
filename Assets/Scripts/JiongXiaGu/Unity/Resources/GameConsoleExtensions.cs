﻿using JiongXiaGu.Unity.GameConsoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    [ConsoleMethodClass]
    public static class GameConsoleExtensions
    {
        private const string Prefix = "Resource.";

        [ConsoleMethod(Prefix + nameof(ShowPaths), Message = "显示所有资源路径")]
        public static void ShowPaths()
        {
            string message =
                "游戏配置的文件夹 : " + Resource.ConfigDirectory + ";" + Environment.NewLine +
                "用户配置的文件夹 : " + Resource.UserConfigDirectory + ";" + Environment.NewLine +
                "用户存档的文件夹 : " + Resource.ArchiveDirectory + ";" + Environment.NewLine +
                "用户模组的文件夹 : " + Resource.UserModDirectory + ";";

            GameConsole.Write(message);
        }
    }
}
