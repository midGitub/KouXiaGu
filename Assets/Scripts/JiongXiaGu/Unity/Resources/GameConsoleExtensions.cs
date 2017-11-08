using JiongXiaGu.Unity.GameConsoles;
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
            string str =
                "DataDirectoryPath : " + Resource.CoreDirectory +
                "\nUserConfigDirectoryPath" + Resource.UserConfigDirectory +
                "\nArchiveDirectoryPath : " + Resource.ArchivesDirectory;
            GameConsole.Write(str);
        }
    }
}
