using JiongXiaGu.Unity.GameConsoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.UI.Cursors
{

    [ConsoleMethodClass]
    public class GameConsoleExtensions
    {

        /// <summary>
        /// 设置鼠标类型;
        /// </summary>
        [ConsoleMethod(nameof(SetCursorStyle), Message = "设置鼠标类型", ParameterDes = new string[]
            {
                "string", "指定鼠标类型名称"
            })]
        public static void SetCursorStyle(string name)
        {

        }

    }
}
