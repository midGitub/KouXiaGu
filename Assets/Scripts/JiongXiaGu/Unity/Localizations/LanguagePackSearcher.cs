using JiongXiaGu.Unity.Resources;
using System.Collections.Generic;
using System;
using System.IO;

namespace JiongXiaGu.Unity.Localizations
{
    /// <summary>
    /// 搜索可使用的语言文件;
    /// </summary>
    public class LanguagePackSearcher
    {
        [PathDefinition(PathDefinition.DataDirectory, "本地化资源目录;")]
        internal const string LocalizationDirectoryName = "Localization";

        private const string packFileExtension = ".language";

        private readonly LanguagePackSerializer packSerializer;

        /// <summary>
        /// 语言包搜索模式;
        /// </summary>
        private static string LanguagePackFileSearchPattern
        {
            get { return "*" + packFileExtension; }
        }

        public LanguagePackSearcher()
        {
            packSerializer = new LanguagePackSerializer();
        }

        public LanguagePackSearcher(LanguagePackSerializer packSerializer)
        {
            this.packSerializer = packSerializer;
        }

        /// <summary>
        /// 枚举所有可用的语言文件;
        /// </summary>
        public List<LanguagePackInfo> FindPacks()
        {
            List<LanguagePackInfo> languagePacks = new List<LanguagePackInfo>();

            foreach (var load in Resource.All)
            {
                var packs = EnumeratePack(load);
                languagePacks.AddRange(packs);
            }

            return languagePacks;
        }

        /// <summary>
        /// 枚举所有可用的语言文件;
        /// </summary>
        public IEnumerable<LanguagePackInfo> EnumeratePack(LoadableContent loadableContentInfo)
        {
            return EnumeratePack(loadableContentInfo, SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        /// 枚举所有可用的语言文件;文件命名需要符合要求;
        /// </summary>
        public IEnumerable<LanguagePackInfo> EnumeratePack(LoadableContent contentConstruct, SearchOption searchOption)
        {
            foreach (ILoadableEntry entry in contentConstruct.EnumerateFiles(LocalizationDirectoryName, LanguagePackFileSearchPattern, searchOption))
            {
                LanguagePackInfo languagePack;
                try
                {
                    Stream stream;
                    if (contentConstruct is LoadableDirectory)
                    {
                        stream = contentConstruct.GetStream(entry);
                    }
                    //读取压缩文件的压缩包采用内存流;
                    else
                    {
                        stream = contentConstruct.GetMemoryStream(entry);
                    }

                    using (stream)
                    {
                        LanguagePackDescription description = packSerializer.DeserializeDesc(stream);
                        languagePack = new LanguagePackInfo(description, contentConstruct, entry);
                    }
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.Log(ex);
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
                return pack.Description.Name + packFileExtension;
            }
            else if (string.IsNullOrWhiteSpace(pack.Description.Language))
            {
                return pack.Description.Language + packFileExtension;
            }
            else
            {
                return DateTime.Now.Ticks + packFileExtension;
            }
        }
    }
}
