using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiongXiaGu.Unity;
using UnityEditor;
using JiongXiaGu.Unity.GameConsoles;

namespace JiongXiaGu.Unity.EditorTools
{

    /// <summary>
    /// 控制台方法拓展;
    /// </summary>
    public class ConsoleUnityExtensions
    {

        /// <summary>
        /// 显示控制台可执行的所有方法;
        /// </summary>
        [MenuItem(XiaGuEditorTool.DebugItemName + "Console/LogConsoleMethodSchema")]
        public static void LogConsoleMethodSchema()
        {
        }
    }
}
