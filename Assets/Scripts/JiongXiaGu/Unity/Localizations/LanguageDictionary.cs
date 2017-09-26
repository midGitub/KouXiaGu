using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Localizations
{

    /// <summary>
    /// 文本字典;
    /// </summary>
    public class LanguageDictionary : Dictionary<string, string>, IDictionary<string, string>
    {
        public LanguageDictionary()
        {
        }

        public LanguageDictionary(IDictionary<string, string> dictionary) : base(dictionary)
        {
        }
    }
}
