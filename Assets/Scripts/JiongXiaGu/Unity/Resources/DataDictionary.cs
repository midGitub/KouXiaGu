using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JiongXiaGu.Collections;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 不允许重复Key的合集;
    /// </summary>
    public class DataDictionary<T>
    {
        public LoadOrder LoadOrder { get; private set; }
        public Dictionary<string, T> Dictionary { get; private set; }


    }
}
