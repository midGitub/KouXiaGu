using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Inputs
{

    /// <summary>
    /// 按键映射;
    /// </summary>
    public class KeyMap : Dictionary<string, CustomKey>
    {
        public KeyMap()
        {
        }

        public KeyMap(int capacity) : base(capacity)
        {
        }

        public KeyMap(IDictionary<string, CustomKey> dictionary) : base(dictionary)
        {
        }
    }
}
