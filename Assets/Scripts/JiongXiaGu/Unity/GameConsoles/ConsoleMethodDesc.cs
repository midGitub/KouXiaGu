﻿using System.Collections.Generic;
using System;

namespace JiongXiaGu.Unity.GameConsoles
{

    /// <summary>
    /// 控制台方法描述;
    /// </summary>
    public class ConsoleMethodDesc
    {
        /// <summary>
        /// 完全名,如 ConsoleMethod.Info.Run ;
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 预留消息;
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 参数描述;
        /// </summary>
        public ParametersDesc Parameters { get; set; }
    }

    /// <summary>
    /// 参数描述合集;
    /// </summary>
    public class ParametersDesc
    {
        private List<ParameterDesc> parameterDescs;

        /// <summary>
        /// 参数描述;
        /// </summary>
        public IList<ParameterDesc> ParameterDescs
        {
            get { return parameterDescs; }
        }

        public ParametersDesc()
        {
            parameterDescs = new List<ParameterDesc>();
        }

        public ParametersDesc(IEnumerable<ParameterDesc> parameters)
        {
            parameterDescs = new List<ParameterDesc>(parameters);
        }

        /// <summary>
        /// 添加描述到下一个参数;
        /// </summary>
        private void Add(ParameterDesc desc)
        {
            parameterDescs.Add(desc);
        }

        /// <summary>
        /// 添加描述到下一个参数;
        /// </summary>
        /// <param name="type">参数类型</param>
        /// <param name="message">参数描述</param>
        private void Add(string type, string message)
        {
            ParameterDesc desc = new ParameterDesc(type, message);
            parameterDescs.Add(desc);
        }

        /// <summary>
        /// 转换类型;合集格式为:[方法1类型, 方法1消息, 方法2类型, 方法2消息, ....]
        /// </summary>
        internal static ParametersDesc Convert(IReadOnlyList<string> desc)
        {
            ParametersDesc collection = new ParametersDesc();
            if (desc != null)
            {
                for (int i = 0; i < desc.Count; i += 2)
                {
                    int typeIndex = i * 2;
                    string typeString = desc[typeIndex];

                    int messageIndex = i * 2 + 1;
                    string messageSrting = messageIndex < desc.Count ? desc[messageIndex] : string.Empty;

                    collection.Add(typeString, messageSrting);
                }
            }
            return collection;
        }

        /// <summary>
        /// 返回数组格式的描述信息;
        /// </summary>
        internal string[] Convert()
        {
            if (parameterDescs == null)
            {
                return null;
            }
            else
            {
                string[] array = new string[parameterDescs.Count * 2];
                for (int i = 0; i < parameterDescs.Count; i++)
                {
                    ParameterDesc desc = parameterDescs[i];
                    array[2 * i] = desc.Type;
                    array[2 * i + 1] = desc.Message;
                }
                return array;
            }
        }
    }

    /// <summary>
    /// 参数描述;
    /// </summary>
    public struct ParameterDesc
    {
        public ParameterDesc(string type, string message)
        {
            Type = type;
            Message = message;
        }

        /// <summary>
        /// 参数类型文本表示;
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 预留消息;
        /// </summary>
        public string Message { get; set; }
    }
}
