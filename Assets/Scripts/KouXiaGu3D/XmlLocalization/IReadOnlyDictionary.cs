using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.XmlLocalization
{

    public interface IReadOnlyDictionary
    {

        /// <summary>
        /// 获取到文本,若无法获取到则返回 KeyNotFoundException;
        /// </summary>
        string this[string key] { get; }

        /// <summary>
        /// 获取到文本,若获取失败则返回false;
        /// </summary>
        bool TryGetValue(string key, out string text);

    }

}
