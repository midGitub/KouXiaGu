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
        private const string DefaultMethodTag = "GameConsole";

        [ConsoleMethod(nameof(ShowMethods), Tag = DefaultMethodTag, Message = "展示所有方法")]
        public static void ShowMethods()
        {
            StringBuilder stringBuilder = new StringBuilder();

            AddString(stringBuilder, GameConsole.MethodMap.Methods);

            string message = stringBuilder.ToString();
            GameConsole.Write(message);
        }

        [ConsoleMethod(nameof(ShowMethods), Tag = DefaultMethodTag, Message = "展示所有方法", ParameterDes = new string[]
            {
                "string", "按标签筛选,仅允许单个标签"
            })]
        public static void ShowMethods(string tag)
        {
            StringBuilder stringBuilder = new StringBuilder();

            AddString(stringBuilder, GameConsole.MethodMap.Methods.Where(item => string.Equals(item.Description.Tag, tag, StringComparison.OrdinalIgnoreCase)));

            string message = stringBuilder.ToString();
            GameConsole.Write(message);
        }

        /// <summary>
        /// 将方法描述转为文本;
        /// </summary>
        private static void AddString(StringBuilder stringBuilder, IEnumerable<IMethod> methods)
        {
            int count = 0;
            foreach (var method in methods)
            {
                AddString(stringBuilder, method);
                stringBuilder.AppendLine();
                count++;
            }

            stringBuilder.AppendFormat("方法总数 : {0}", count);
        }

        private static void AddString(StringBuilder stringBuilder, IMethod consoleMethod)
        {
            const string NullStr = "Null";

            MethodDescription description = consoleMethod.Description;
            if (description.Parameters.Count > 0)
            {
                stringBuilder.AppendFormat("Name : {0}", description.Name);
                foreach (var parameter in description.Parameters)
                {
                    stringBuilder.Append(string.Format(" [{0}]", parameter.Type));
                }

                stringBuilder.AppendFormat("; Tag : {0}; Message : {1} ;", description.Tag ?? NullStr, description.Message ?? NullStr);

                stringBuilder.Append(" Parameters : ");
                for (int index = 0; index < description.Parameters.Count; index++)
                {
                    var parameter = description.Parameters[index];
                    stringBuilder.AppendFormat("[{0}]({1}){2} ", index, parameter.Type, parameter.Message);
                }

                stringBuilder.Append(';');
            }
            else
            {
                stringBuilder.AppendFormat("Name : {0}; Tag : {1}; Message : {2} ;", description.Name, description.Tag ?? NullStr, description.Message ?? NullStr);
            }
        }
    }
}
