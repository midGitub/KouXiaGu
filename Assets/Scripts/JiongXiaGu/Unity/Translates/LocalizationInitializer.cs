using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using JiongXiaGu.Unity.Resources;
using JiongXiaGu.Resources;

namespace JiongXiaGu.Unity.Translates
{

    /// <summary>
    /// 本地化组件控制;
    /// </summary>
    [DisallowMultipleComponent]
    class LocalizationInitializer : MonoBehaviour, IGameInitializeHandle
    {
        LocalizationInitializer()
        {
        }

        [PathDefinition(ResourceTypes.DataDirectory, "本地化组件资源目录;")]
        internal const string LocalizationDirectoryName = "Localization";

        /// <summary>
        /// 默认语言;
        /// </summary>
        [SerializeField]
        SystemLanguage defaultLanguage;

        /// <summary>
        /// 在游戏开始时进行初始化;
        /// </summary>
        Task IGameInitializeHandle.StartInitialize(CancellationToken token)
        {
            return Task.Run(delegate ()
            {
                token.ThrowIfCancellationRequested();

                IEnumerable<LanguagePackFileInfo> fileInfos = GetLanguagePasks();
                LocalizationConfig config;
                IEnumerable<LanguagePackFileInfo> targetFileInfos;

                if (TryReadLocalizationConfig(out config))
                {
                    Func<LanguagePackFileInfo, bool> targetMatch = delegate (LanguagePackFileInfo info)
                    {
                        return config.LanguagePackInfo == info;
                    };

                    var spareLanguages = new string[]
                    {
                            Application.systemLanguage.ToString(),
                            defaultLanguage.ToString(),
                            SystemLanguage.ChineseSimplified.ToString(),
                    };

                    targetFileInfos = FindLanguagePask(fileInfos, targetMatch, spareLanguages);
                }
                else
                {
                    string defaultLanguage = this.defaultLanguage.ToString();
                    Func<LanguagePackFileInfo, bool> targetMatch = delegate (LanguagePackFileInfo info)
                    {
                        return info.Language == defaultLanguage;
                    };

                    var spareLanguages = new string[]
                    {
                        Application.systemLanguage.ToString(),
                        defaultLanguage.ToString(),
                        SystemLanguage.ChineseSimplified.ToString(),
                    };

                    targetFileInfos = FindLanguagePask(fileInfos, targetMatch, spareLanguages);
                }



            }, token);
        }

        /// <summary>
        /// 尝试读取到配置文件;
        /// </summary>
        bool TryReadLocalizationConfig(out LocalizationConfig config)
        {
            LocalizationConfigReader configReader = new LocalizationConfigReader();
            try
            {
                config = configReader.Deserialize();
                return true;
            }
            catch (FileNotFoundException)
            {
                config = default(LocalizationConfig);
                return false;
            }
        }

        /// <summary>
        /// 获取到指定的语言,获取优先值按数组排列顺序;
        /// </summary>
        public IEnumerable<LanguagePackFileInfo> FindLanguagePask(IEnumerable<LanguagePackFileInfo> fileInfos, Func<LanguagePackFileInfo, bool> targetMatch, params string[] spareLanguages)
        {
            List<LanguagePackFileInfo>[] spareFileInfo = new List<LanguagePackFileInfo>[spareLanguages.Length];

            foreach (var fileInfo in fileInfos)
            {
                if (targetMatch(fileInfo))
                {
                    yield return fileInfo;
                    continue;
                }

                for (int i = 1; i < spareLanguages.Length; i++)
                {
                    string language = spareLanguages[i];
                    if (fileInfo.Language == language)
                    {
                        if (spareFileInfo[i] == null)
                        {
                            spareFileInfo[i] = new List<LanguagePackFileInfo>();
                        }
                        spareFileInfo[i].Add(fileInfo);
                        break;
                    }
                }
            }

            foreach (var fileInfoCollection in spareFileInfo)
            {
                if (fileInfoCollection != null)
                {
                    foreach (var fileInfo in fileInfoCollection)
                    {
                        yield return fileInfo;
                    }
                }
            }
        }

        /// <summary>
        /// 获取到所有语言包信息;
        /// </summary>
        IEnumerable<LanguagePackFileInfo> GetLanguagePasks()
        {
            string languagePasksDirectory = GetLanguagePasksDirectory();
            return LanguagePackReader.EnumerateInfos(languagePasksDirectory, SearchOption.AllDirectories);
        }

        /// <summary>
        /// 获取到语言包存放目录;
        /// </summary>
        /// <returns></returns>
        string GetLanguagePasksDirectory()
        {
            string path = Path.Combine(Resource.DataDirectoryPath, LocalizationDirectoryName);
            return path;
        }
    }
}
