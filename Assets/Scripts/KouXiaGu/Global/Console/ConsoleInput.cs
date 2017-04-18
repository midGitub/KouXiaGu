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
    [ConsoleClass]
    public class ConsoleInput
    {
        const int DefaultMaxRecordNumber = 33;

        public ConsoleInput()
        {
            methodMap = new ConsoleMethodReflection();
            inputContents = new LinkedList<string>();
            MaxRecordNumber = DefaultMaxRecordNumber;
        }

        ConsoleMethodReflection methodMap;

        /// <summary>
        /// 记录输入内容;
        /// </summary>
        LinkedList<string> inputContents;
        public int MaxRecordNumber { get; private set; }

        internal IDictionary<string, MethodGroup> MethodMap
        {
            get { return methodMap; }
        }

        public void Operate(string message)
        {
            RecordInputContent(message);
            object[] parameters;
            string keyword = GetKeywrod(message, out parameters);
            MethodItem method;

            if (methodMap.TryGetMethod(keyword, parameters,out method))
            {
                method.Invoke(parameters);
            }
            else
            {
                throw new ArgumentException("未知命令:" + message);
            }
        }

        void RecordInputContent(string message)
        {
            if (inputContents.Count >= MaxRecordNumber)
            {
                inputContents.RemoveFirst();
            }
            inputContents.AddLast(message);
        }

        static readonly char[] separator = new char[]
            {
                ' ',
            };

        string GetKeywrod(string message, out object[] parameters)
        {
            string[] wordArray = message.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            string keyWord = wordArray[0];

            int parameterCount = wordArray.Length - 1;
            parameters = new string[parameterCount];
            Array.Copy(wordArray, 1, parameters, 0, parameterCount);

            return keyWord;
        }

        const string testPrefix = "Test:";

        [ConsoleMethod("test")]
        public static void Test()
        {
            Debug.Log(testPrefix + "Null");
        }

        [ConsoleMethod("test")]
        public static void Test(string str)
        {
            Debug.Log(testPrefix + str);
        }

        [ConsoleMethod("test")]
        public static void Test(string str0, string str1)
        {
            Debug.Log(testPrefix + str0 + "," + str1);
        }
    }

}
