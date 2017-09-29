using System.IO;

namespace JiongXiaGu.Unity.Localizations
{

    /// <summary>
    /// 语言包文件信息;
    /// </summary>
    public class LanguagePackFileInfo
    {
        /// <summary>
        /// 语言包名;
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 语言类型;
        /// </summary>
        public string Language { get; private set; }

        /// <summary>
        /// 文件信息实例;
        /// </summary>
        public FileInfo FileInfo { get; private set; }

        /// <summary>
        /// 指定语言包文件信息;
        /// </summary>
        public LanguagePackFileInfo(FileInfo fileInfo, string name, string language)
        {
            FileInfo = fileInfo;
            Name = name;
            Language = language;
        }

        public static implicit operator LanguagePackInfo(LanguagePackFileInfo fileInfo)
        {
            return new LanguagePackInfo(fileInfo.Name, fileInfo.Language);
        }
    }
}
