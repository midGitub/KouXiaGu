using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JiongXiaGu.Unity.Localizations
{

    /// <summary>
    /// 语言包文件信息;
    /// </summary>
    public class LanguagePackFileInfo
    {
        /// <summary>
        /// 文件信息实例;
        /// </summary>
        public FileInfo FileInfo { get; private set; }

        /// <summary>
        /// 语言包名;
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 语言类型;
        /// </summary>
        public string Language { get; private set; }
    }

    /// <summary>
    /// 语言包文件搜索;
    /// </summary>
    public class LanguagePackFileSearcher
    {
        /// <summary>
        /// 迭代获取到所有语言包信息;
        /// </summary>
        public IEnumerable<LanguagePackFileInfo> EnumerateLanguagePack()
        {
            throw new NotImplementedException();
        }
    }
}
