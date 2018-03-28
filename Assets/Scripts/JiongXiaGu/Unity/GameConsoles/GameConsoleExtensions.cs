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

        [ConsoleMethod(nameof(ShowMethods), Message = "展示所有方法")]
        public static void ShowMethods()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("方法总数 : {0}", GameConsole.MethodMap.Methods.Count);
            stringBuilder.AppendLine();

            foreach (var consoleMethod in GameConsole.MethodMap.Methods)
            {
                AddMethodString(stringBuilder, consoleMethod);
                stringBuilder.AppendLine();
            }

            string message = stringBuilder.ToString();
            GameConsole.Write(message);
        }

        [ConsoleMethod(nameof(ShowMethods), Message = "按条件展示方法", ParameterDes = new string[]
            {
                "bool", "是否仅展示可执行的方法?"
            })]
        public static void ShowMethods(string onlyActivated)
        {
            bool _onlyActivated = Convert.ToBoolean(onlyActivated);

            if (!_onlyActivated)
            {
                ShowMethods();
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendFormat("方法总数 : {0}", GameConsole.MethodMap.Methods.Count);
                stringBuilder.AppendLine();

                foreach (var consoleMethod in GameConsole.MethodMap.Methods)
                {
                    AddMethodString(stringBuilder, consoleMethod);
                    stringBuilder.AppendLine();
                }

                string message = stringBuilder.ToString();
                GameConsole.Write(message);
            }
        }

        private static void AddMethodString(StringBuilder stringBuilder, IMethod consoleMethod)
        {
            MethodDescription description = consoleMethod.Description;
            if (description.Parameters.Count > 0)
            {
                stringBuilder.AppendFormat("MethodName : {0}", description.Name);
                foreach (var parameter in description.Parameters)
                {
                    stringBuilder.Append(string.Format(" [{0}]", parameter.Type));
                }

                stringBuilder.AppendFormat(", Message : {0}, ParamCount : {1}", description.Message, description.Parameters.Count);

                stringBuilder.Append(", ParamDesc : ");
                for (int index = 0; index < description.Parameters.Count; index++)
                {
                    var parameter = description.Parameters[index];
                    stringBuilder.AppendFormat("[{0}]({1}){2} ", index, parameter.Type, parameter.Message);
                }
            }
            else
            {
                stringBuilder.AppendFormat("MethodName : {0}, Message : {1}, ParamCount : {2}", description.Name, description.Message, description.Parameters.Count);
            }
        }
    }
}
