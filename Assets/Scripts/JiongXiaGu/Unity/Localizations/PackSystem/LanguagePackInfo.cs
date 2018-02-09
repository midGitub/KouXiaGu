using JiongXiaGu.Unity.Resources;
using System.IO;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Localizations
{


    public interface ILanguagePackInfo
    {
        LanguagePackDescription Description { get; }
        Stream GetInputStream();
    }

    /// <summary>
    /// 指语言包信息,但是不保证文件可以正常读取;
    /// </summary>
    public class LanguagePackInfo : ILanguagePackInfo
    {
        public LanguagePackDescription Description { get; private set; }
        public Content Content { get; private set; }
        public string RelativePath { get; private set; }

        /// <summary>
        /// 指定语言包文件信息;
        /// </summary>
        public LanguagePackInfo(LanguagePackDescription description, Content content, string relativePath)
        {
            Description = description;
            Content = content;
            RelativePath = relativePath;
        }

        public Stream GetInputStream()
        {
            return Content.GetInputStream(RelativePath);
        }
    }
}
