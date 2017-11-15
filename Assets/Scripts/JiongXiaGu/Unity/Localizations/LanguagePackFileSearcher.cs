using JiongXiaGu.Unity.Resources;
using System.Collections.Generic;
using System;
using System.IO;

namespace JiongXiaGu.Unity.Localizations
{
    /// <summary>
    /// 搜索可使用的语言文件;
    /// </summary>
    public class LanguagePackFileSearcher
    {
        [PathDefinition(PathDefinition.DataDirectory, "本地化资源目录;")]
        internal const string LocalizationDirectoryName = "Localization";

        private const string packFilePrefix = "";
        private const string packFileExtension = ".language";

        private readonly LanguagePackSerializer packSerializer;

        /// <summary>
        /// 语言包搜索模式;
        /// </summary>
        private static string LanguagePackFileSearchPattern
        {
            get { return packFilePrefix + "*" + packFileExtension; }
        }

        public LanguagePackFileSearcher()
        {
            packSerializer = new LanguagePackSerializer();
        }

        public LanguagePackFileSearcher(LanguagePackSerializer packSerializer)
        {
            this.packSerializer = packSerializer;
        }

        /// <summary>
        /// 枚举所有可用的语言文件;
        /// </summary>
        public IEnumerable<LanguagePackFileInfo> EnumeratePack(LoadableContentInfo loadableContentInfo)
        {
            string directory = Path.Combine(loadableContentInfo.DirectoryInfo.FullName, LocalizationDirectoryName);
            return EnumeratePack(directory, SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        /// 枚举所有可用的语言文件;文件命名需要符合要求;
        /// </summary>
        public IEnumerable<LanguagePackFileInfo> EnumeratePack(string directory, SearchOption searchOption)
        {
            foreach (var filePath in Directory.EnumerateFiles(directory, LanguagePackFileSearchPattern, searchOption))
            {
                LanguagePackFileInfo languagePack;

                try
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    LanguagePackDescription description = packSerializer.DeserializeDesc(filePath);
                    languagePack = new LanguagePackFileInfo(description, fileInfo);
                }
                catch (Exception)
                {
                    languagePack = null;
                }

                if (languagePack != null)
                {
                    yield return languagePack;
                }
            }
        }

        /// <summary>
        /// 获取到规范的文件名;
        /// </summary>
        public string GetFileName(LanguagePack pack)
        {
            if (string.IsNullOrWhiteSpace(pack.Description.Name))
            {
                return packFilePrefix + pack.Description.Name + packFileExtension;
            }
            else if (string.IsNullOrWhiteSpace(pack.Description.Language))
            {
                return packFilePrefix + pack.Description.Language + packFileExtension;
            }
            else
            {
                return packFilePrefix + DateTime.Now.Ticks + packFileExtension;
            }
        }
    }
}
