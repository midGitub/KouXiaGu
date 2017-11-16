using System.IO;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Localizations
{

    /// <summary>
    /// 指语言包文件信息,但是不保证文件可以正常读取;
    /// </summary>
    public class LanguagePackFileInfo
    {
        /// <summary>
        /// 描述;
        /// </summary>
        public LanguagePackDescription Description { get; private set; }

        /// <summary>
        /// 文件信息实例;
        /// </summary>
        public FileInfo FileInfo { get; private set; }

        /// <summary>
        /// 指定语言包文件信息;
        /// </summary>
        public LanguagePackFileInfo(LanguagePackDescription description, FileInfo fileInfo)
        {
            Description = description;
            FileInfo = fileInfo;
        }
    }
}
