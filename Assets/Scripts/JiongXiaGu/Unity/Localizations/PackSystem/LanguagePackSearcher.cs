﻿using JiongXiaGu.Unity.Resources;
using System.Collections.Generic;
using System;
using System.IO;

namespace JiongXiaGu.Unity.Localizations
{


    public static class PackSearcher
    {
        [PathDefinition(PathDefinitionType.DataDirectory, "本地化资源目录;")]
        internal const string MainPackDirectoryName = @"Localization";

        [PathDefinition(PathDefinitionType.DataDirectory, "本地化模组补充资源目录;")]
        internal const string SupplementaryPackDirectoryName = @"Localization/Supplementary";

        internal const string packFileExtension = ".zip";

        /// <summary>
        /// 语言包搜索模式;
        /// </summary>
        private static string LanguagePackFileSearchPattern
        {
            get { return "*" + packFileExtension; }
        }

        /// <summary>
        /// 枚举所有语言包;
        /// </summary>
        public static IEnumerable<LanguagePackInfo> EnumeratePack(this LanguagePackSerializer packSerializer, Content content, SearchOption searchOption)
        {
            return Enumerate(packSerializer, content, MainPackDirectoryName, searchOption);
        }

        /// <summary>
        /// 枚举所有语言补充包;
        /// </summary>
        public static IEnumerable<LanguagePackInfo> EnumerateSupplementaryPack(this LanguagePackSerializer packSerializer, Content content, SearchOption searchOption)
        {
            return Enumerate(packSerializer, content, SupplementaryPackDirectoryName, searchOption);
        }

        /// <summary>
        /// 枚举所有可用的语言文件;文件命名需要符合要求;
        /// </summary>
        public static IEnumerable<LanguagePackInfo> Enumerate(this LanguagePackSerializer packSerializer, Content content, string rootDirectory, SearchOption searchOption)
        {
            if (packSerializer == null)
                throw new ArgumentNullException(nameof(packSerializer));
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            foreach (string entry in content.EnumerateFiles(rootDirectory, LanguagePackFileSearchPattern, searchOption))
            {
                LanguagePackInfo languagePack;
                try
                {
                    using (var stream = content.GetInputStream(entry))
                    {
                        var description = packSerializer.Deserialize(stream);
                        languagePack = new LanguagePackInfo(description, content, entry);
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
    }


    /// <summary>
    /// 搜索可使用的语言文件;
    /// </summary>
    public class LanguagePackSearcher
    {
        [PathDefinition(PathDefinitionType.DataDirectory, "本地化资源目录;")]
        internal const string LocalizationDirectoryName = "Localization";

        private const string packFilePrefix = "Language_";
        private const string packFileExtension = ".zip";

        private readonly LanguagePackSerializer packSerializer = new LanguagePackSerializer();

        /// <summary>
        /// 语言包搜索模式;
        /// </summary>
        private static string LanguagePackFileSearchPattern
        {
            get { return packFilePrefix + "*" + packFileExtension; }
        }

        /// <summary>
        /// 枚举所有可用的语言文件;
        /// </summary>
        public List<LanguagePackInfo> FindPacks(IEnumerable<Content> loadableContents)
        {
            List<LanguagePackInfo> languagePacks = new List<LanguagePackInfo>();

            foreach (var content in loadableContents)
            {
                var packs = EnumeratePack(content);
                languagePacks.AddRange(packs);
            }

            return languagePacks;
        }

        /// <summary>
        /// 枚举所有可用的语言文件;
        /// </summary>
        public IEnumerable<LanguagePackInfo> EnumeratePack(Content loadableContentInfo)
        {
            return EnumeratePack(loadableContentInfo, SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        /// 枚举所有可用的语言文件;文件命名需要符合要求;
        /// </summary>
        public IEnumerable<LanguagePackInfo> EnumeratePack(Content contentConstruct, SearchOption searchOption)
        {
            foreach (string entry in contentConstruct.EnumerateFiles(LocalizationDirectoryName, LanguagePackFileSearchPattern, searchOption))
            {
                LanguagePackInfo languagePack;
                try
                {
                    using (var stream = contentConstruct.GetInputStream(entry))
                    {
                        var description = packSerializer.Deserialize(stream);
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
