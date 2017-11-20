using JiongXiaGu.Unity.Initializers;
using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Localizations
{

    /// <summary>
    /// 提供在Unity编辑器内调整具体参数;
    /// </summary>
    [DisallowMultipleComponent]
    internal class LocalizationController : MonoBehaviour, IComponentInitializeHandle
    {
        private const string InitializerName = "本地化组件初始化";

        /// <summary>
        /// 默认语言,在找不到指定语言和系统语言时使用的语言;
        /// </summary>
        [SerializeField]
        private SystemLanguage defaultLanguage = SystemLanguage.ChineseSimplified;
        private LanguagePackSearcher packSearcher;
        private LanguagePackSerializer packSerializer;
        private LocalizationConfigFileReader configFileReader;
        public SystemLanguage SystemLanguage { get; private set; }

        /// <summary>
        /// 读取配置文件时遇到的异常,若不存在则为Null;
        /// </summary>
        internal Exception ReadConfigFileException { get; private set; }

        private List<LanguagePackInfo> availableLanguagePacks
        {
            get { return Localization.AvailableLanguagePacks; }
            set { Localization.AvailableLanguagePacks = value; }
        }

        private void Awake()
        {
            packSearcher = new LanguagePackSearcher();
            packSerializer = new LanguagePackSerializer();
            configFileReader = new LocalizationConfigFileReader();
            SystemLanguage = Application.systemLanguage;
        }

        Task IComponentInitializeHandle.Initialize(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            availableLanguagePacks = packSearcher.FindPacks(Resource.All);
            if (availableLanguagePacks.Count == 0)
            {
                throw new FileNotFoundException("未找到合适的语言包文件");
            }

            return Task.Run(delegate ()
            {
                token.ThrowIfCancellationRequested();

                LocalizationConfig? config = ReadConfigFile();
                LanguagePack languagePack = null;

                foreach (var packInfo in EnumerateLanguagePask(config))
                {
                    try
                    {
                        languagePack = packSerializer.Deserialize(packInfo.ContentConstruct, packInfo.LoadableEntry);
                        break;
                    }
                    catch (Exception ex)
                    {
                        UnityDebugHelper.LogWarning(InitializerName, "读取语言包时需要错误", ex);
                    }
                    token.ThrowIfCancellationRequested();
                }

                if (languagePack != null)
                {
                    Localization.SetLanguage(languagePack);
                }
                else
                {
                    throw new FileNotFoundException("未找到合适的语言包文件");
                }

                token.ThrowIfCancellationRequested();
                Localization.NotifyLanguageChanged();

                UnityDebugHelper.SuccessfulReport(InitializerName, () => GetInfoLog());
            });
        }
        
        /// <summary>
        /// 获取到所有可使用的语言包;
        /// </summary>
        private List<LanguagePackInfo> GetLanguagePackAll()
        {
            List<LanguagePackInfo> languagePacks = new List<LanguagePackInfo>();

            foreach (var load in Resource.All)
            {
                var packs = packSearcher.EnumeratePack(load);
                languagePacks.AddRange(packs);
            }

            return languagePacks;
        }

        /// <summary>
        /// 读取到配置文件;
        /// </summary>
        private LocalizationConfig? ReadConfigFile()
        {
            try
            {
                return configFileReader.Read();
            }
            catch (Exception ex)
            {
                ReadConfigFileException = ex;
                return null;
            }
        }

        /// <summary>
        /// 枚举所有符合要求的语言包;
        /// </summary>
        private IEnumerable<LanguagePackInfo> EnumerateLanguagePask(LocalizationConfig? config)
        {
            string[] spareLanguages;

            if (config.HasValue)
            {
                LocalizationConfig _config = config.Value;
                foreach (var fileInfo in availableLanguagePacks)
                {
                    if (_config.Language == fileInfo.Description.Language && _config.Name == fileInfo.Description.Name)
                    {
                        yield return fileInfo;
                    }
                }

                spareLanguages = new string[]
                {
                    _config.Language,
                    SystemLanguage.ToString(),
                    defaultLanguage.ToString(),
                };
            }
            else
            {
                spareLanguages = new string[]
                {
                    SystemLanguage.ToString(),
                    defaultLanguage.ToString(),
                };
            }

            for (int i = 0; i < spareLanguages.Length; i++)
            {
                string language = spareLanguages[i];

                foreach (var fileInfo in availableLanguagePacks)
                {
                    if (fileInfo.Description.Language == language)
                    {
                        yield return fileInfo;
                    }
                }
            }

            foreach (var fileInfo in availableLanguagePacks)
            {
                yield return fileInfo;
            }
        }

        [ContextMenu("报告详细信息")]
        private void LogInfo()
        {
            Debug.Log(GetInfoLog());
        }

        private string GetInfoLog()
        {
            string log = 
                "当前语言 : " + Localization.Language.Language
                + ", 系统语言 : " + SystemLanguage.ToString()
                + ", 可读语言总数 : " + Localization.AvailableLanguagePacks.Count
                + ", 可读语言 : " + string.Join(", ", Localization.AvailableLanguagePacks.Select(item => string.Format("[Language : {0}, Name : {1}]", item.Description.Language, item.Description.Name)))
                + ", 语言组内容总数 : " + Localization.language.Count
                + ", 语言组条目总数 : " + Localization.language.TextItemCount()
                ;
            return log;
        }
    }
}
