using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Inputs
{

    /// <summary>
    /// 按键映射读取器;
    /// </summary>
    public class KeyMapReader
    {

    }

    /// <summary>
    /// 按键映射;
    /// </summary>
    public class KeyMap
    {
        public KeyMap()
        {
        }

        public KeyMap(IDictionary<string, FunctionInfo> dictionary)
        {
        }

        /// <summary>
        /// 按键映射信息;
        /// </summary>
        readonly Dictionary<string, FunctionInfo> keyDictionary;


        public FunctionInfo this[string name]
        {

        }
    }
}
