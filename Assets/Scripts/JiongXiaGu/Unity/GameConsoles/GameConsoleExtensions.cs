using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.GameConsoles
{

    /// <summary>
    /// 控制台方法;
    /// </summary>
    [ConsoleMethodClass]
    public class GameConsoleMethodExtensions
    {

        [ConsoleMethod("Help", Message = "显示帮助")]
        public static void Help()
        {

        }
    }

}
