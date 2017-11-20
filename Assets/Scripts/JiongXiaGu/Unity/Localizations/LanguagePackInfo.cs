using JiongXiaGu.Unity.Resources;
using System.IO;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Localizations
{

    /// <summary>
    /// 指语言包信息,但是不保证文件可以正常读取;
    /// </summary>
    public class LanguagePackInfo
    {
        /// <summary>
        /// 描述;
        /// </summary>
        public LanguagePackDescription Description { get; private set; }

        public LoadableContent ContentConstruct { get; private set; }

        public ILoadableEntry LoadableEntry { get; private set; }

        /// <summary>
        /// 指定语言包文件信息;
        /// </summary>
        public LanguagePackInfo(LanguagePackDescription description, LoadableContent contentConstruct, ILoadableEntry loadableEntry)
        {
            Description = description;
            ContentConstruct = contentConstruct;
            LoadableEntry = loadableEntry;
        }
    }
}
