using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using JiongXiaGu.Collections;

namespace JiongXiaGu.Unity.Translates
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

        /// <summary>
        /// 添加字典内容;
        /// </summary>
        public void AddOrUpdate(LanguageDictionary languageDictionary)
        {
            if (languageDictionary == this)
                return;

            foreach (var pair in languageDictionary)
            {
                this.AddOrUpdate(pair);
            }
        }
    }
}
