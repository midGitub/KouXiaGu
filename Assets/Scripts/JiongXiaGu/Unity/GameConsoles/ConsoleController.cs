using JiongXiaGu.Unity.Initializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.GameConsoles
{

    /// <summary>
    /// 游戏控制台控制器;
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class ConsoleController : MonoBehaviour
    {
        [SerializeField]
        private ConsoleStringBuilderRichTextFormat format;

        public void Initialize()
        {
            //GameConsole.MethodSchema = new ConsoleMethodSchema();
            //initializer.RuntimeReflection.ReflectionHandlers.Add(ConsoleMethodReflection.Default);
        }

        private void OnComplete()
        {
            EditorHelper.LogComplete("控制台", ToConsoleString);
        }

        public string ToConsoleString()
        {
            string info = string.Format("方法总数:{0}", GameConsole.MethodSchema.Count);
            info += "\n" + ConsoleMethodReflection.Default.Exceptions.ToLog("Exceptions:");
            return info;
        }
    }
}
