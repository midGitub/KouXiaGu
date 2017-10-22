using System.Collections.Generic;
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
        public string FullName { get; set; }

        /// <summary>
        /// 预留消息;
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 参数描述;
        /// </summary>
        public ParameterDescCollection Parameters { get; set; }
    }

    /// <summary>
    /// 参数描述合集;
    /// </summary>
    public struct ParameterDescCollection
    {
        private List<ParameterDesc> parameterDescs;

        /// <summary>
        /// 参数描述;
        /// </summary>
        public IList<ParameterDesc> ParameterDescs
        {
            get { return parameterDescs; }
        }

        public ParameterDescCollection(IEnumerable<ParameterDesc> parameters)
        {
            parameterDescs = new List<ParameterDesc>(parameters);
        }

        /// <summary>
        /// 添加描述到下一个参数;
        /// </summary>
        public void Add(ParameterDesc desc)
        {
            if (parameterDescs == null)
            {
                parameterDescs = new List<ParameterDesc>();
            }
            parameterDescs.Add(desc);
        }

        /// <summary>
        /// 添加描述到下一个参数;
        /// </summary>
        /// <param name="type">参数类型</param>
        /// <param name="message">参数描述</param>
        public void Add(string type, string message)
        {
            if (parameterDescs == null)
            {
                parameterDescs = new List<ParameterDesc>();
            }
            ParameterDesc desc = new ParameterDesc(type, message);
            parameterDescs.Add(desc);
        }

        /// <summary>
        /// 转换类型;
        /// </summary>
        internal static ParameterDescCollection Convert(IReadOnlyList<string> desc)
        {
            ParameterDescCollection collection = new ParameterDescCollection();
            if (desc != null)
            {
                for (int i = 0; i < desc.Count; i++)
                {
                    collection.Add(desc[i * 2], desc[i * 2 + 1]);
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
