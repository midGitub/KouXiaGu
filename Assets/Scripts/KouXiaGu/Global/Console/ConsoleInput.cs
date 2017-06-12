using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KouXiaGu.Collections;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 控制台输入控制,通过反射获得所有 关键词 和其 重载的方法;
    /// </summary>
    [ConsoleMethodsClass]
    public class ConsoleInput
    {
        const int DefaultMaxRecordNumber = 30;

        public ConsoleInput()
        {
            methodMap = new ConsoleCommand();
            inputContents = new List<string>();
            MaxRecordNumber = DefaultMaxRecordNumber;

            methodMap.Add("help", new CommandItem("help", HelpMethod, "显示所有命令"));
        }

        internal ConsoleCommand methodMap;

        /// <summary>
        /// 记录输入内容;
        /// </summary>
        List<string> inputContents;
        public int MaxRecordNumber { get; private set; }

        public string this[int index]
        {
            get { return inputContents[index]; }
        }

        public int RecordCount
        {
            get { return inputContents.Count; }
        }

        public string FinalInputContent
        {
            get { return inputContents.Count == 0 ? string.Empty : inputContents[inputContents.Count - 1]; }
        }

        public bool Operate(string message)
        {
            RecordInputContent(message);
            return methodMap.Operate(message);
        }

        /// <summary>
        /// 记录这个消息;
        /// </summary>
        void RecordInputContent(string message)
        {
            if (inputContents.Count >= MaxRecordNumber)
            {
                inputContents.RemoveAt(0);
            }
            inputContents.Add(message);
        }

        /// <summary>
        /// 控制台命令 help;
        /// </summary>
        void HelpMethod(string[] none)
        {
            foreach (var methodGroup in methodMap.CommandDictionary.Values)
            {
                foreach (var method in methodGroup)
                {
                    if (method.Message != string.Empty)
                    {
                        GameConsole.Log(method.Message);
                    }
                }
            }
        }

    //#region 测试方法

    //    const string testPrefix = "Test:";

    //    [ConsoleMethod("test")]
    //    public static void Test()
    //    {
    //        Debug.Log(testPrefix + "Null");
    //    }

    //    [ConsoleMethod("test")]
    //    public static void Test(string str)
    //    {
    //        Debug.Log(testPrefix + str);
    //    }

    //    [ConsoleMethod("test")]
    //    public static void Test(string str0, string str1)
    //    {
    //        Debug.Log(testPrefix + str0 + "," + str1);
    //    }
    //    #endregion
    }
}
