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
    /// 搜索模组语言资源,加载合适的语言资源,加载合适的模组语言资源;
    /// </summary>
    [DisallowMultipleComponent]
    internal class ModificationLocalizations : MonoBehaviour, IResourceInitializeHandle
    {
        private const string InitializerName = "本地化组件初始化";

        /// <summary>
        /// 默认语言,在找不到指定语言和系统语言时使用的语言;
        /// </summary>
        [SerializeField]
        private SystemLanguage defaultLanguage = SystemLanguage.ChineseSimplified;
        public SystemLanguage SystemLanguage { get; private set; }

        /// <summary>
        /// 读取配置文件时遇到的异常,若不存在则为Null;
        /// </summary>
        internal Exception ReadConfigFileException { get; private set; }

        private List<LanguagePackInfo> LanguagePacks
        {
            get { return Localization.AvailableLanguagePacks; }
            set { Localization.AvailableLanguagePacks = value; }
        }

        private void Awake()
        {
            SystemLanguage = Application.systemLanguage;
        }

        /// <summary>
        /// 搜索模组语言资源,加载合适的语言资源,加载合适的模组语言资源;
        /// </summary>
        void IResourceInitializeHandle.Initialize(IReadOnlyList<ModificationContent> mods, CancellationToken token)
        {
            LocalizationConfigSerializer configSerializer = new LocalizationConfigSerializer();
            LanguagePackSerializer packSerializer = new LanguagePackSerializer();

            //读取语言配置
            LocalizationConfig? config = TrySerializeConfig(configSerializer);

            //搜索主语言包
            LanguagePacks = SearchLanguagePack(packSerializer, mods);

            //读取合适的主语言包
            LanguagePack languagePack = null;
            foreach (LanguagePackInfo info in EnumerateLanguagePask(config, LanguagePacks))
            {
                if (TryDeserializePack(packSerializer, info, out languagePack))
                {
                    break;
                }
            }

            if (languagePack == null)
            {
                throw new FileNotFoundException("未找到合适的语言包!");
            }


            //搜索读取语言补充包
            foreach (var mod in mods)
            {
                LanguagePackInfo supplementaryPackInfo = FindSupplementaryPack(packSerializer, mod, languagePack.Description);
                if (supplementaryPackInfo != null)
                {
                    LanguagePack supplementaryPack;
                    if (TryDeserializePack(packSerializer, supplementaryPackInfo, out supplementaryPack))
                    {
                        AddTo(languagePack, supplementaryPack);
                    }
                }
            }


            //若未成功读取语言配置,则输出;
            if (!config.HasValue)
            {
                DeserializeConfig(configSerializer, languagePack.Description);
            }

            Localization.LanguagePack = languagePack;
            Localization.NotifyLanguageChanged();
            UnityDebugHelper.SuccessfulReport(InitializerName, () => GetInfoLog());
        }

        private LocalizationConfig? TrySerializeConfig(LocalizationConfigSerializer configSerializer)
        {
            try
            {
                LocalizationConfig? config = configSerializer.Deserialize(Resource.ConfigContent);
                return config;
            }
            catch (Exception ex)
            {
                Debug.LogWarning("读取语言配置失败:" + ex);
                return null;
            }
        }

        private void DeserializeConfig(LocalizationConfigSerializer configSerializer, LanguagePackDescription description)
        {
            try
            {
                LocalizationConfig config = new LocalizationConfig()
                {
                    Name = description.Name,
                    Language = description.Language,
                };

                using (Resource.ConfigContent.BeginUpdate())
                {
                    configSerializer.Serialize(Resource.ConfigContent, config);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning("输出语言配置失败:" + ex);
            }
        }

        /// <summary>
        /// 搜索所有可用的语言包;
        /// </summary>
        private List<LanguagePackInfo> SearchLanguagePack(LanguagePackSerializer packSerializer, IEnumerable<ModificationContent> mods)
        {
            List<LanguagePackInfo> packInfos = new List<LanguagePackInfo>();

            foreach (var mod in mods)
            {
                var packs = packSerializer.EnumeratePack(mod.BaseContent, SearchOption.TopDirectoryOnly);
                packInfos.AddRange(packs);
            }

            return packInfos;
        }

        /// <summary>
        /// 按优先级枚举所有符合要求的语言包;
        /// </summary>
        private IEnumerable<LanguagePackInfo> EnumerateLanguagePask(LocalizationConfig? config, IEnumerable<LanguagePackInfo> languagePacks)
        {
            //语言优先级排列顺序
            string[] spareLanguages;

            if (config.HasValue)
            {
                //匹配语言包信息,并且返回;
                LocalizationConfig _config = config.Value;
                foreach (var info in languagePacks)
                {
                    if (_config.Language == info.Description.Language && _config.Name == info.Description.Name)
                    {
                        yield return info;
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

            //按语言优先级返回合适的语言包;
            for (int i = 0; i < spareLanguages.Length; i++)
            {
                string language = spareLanguages[i];

                foreach (var fileInfo in LanguagePacks)
                {
                    if (fileInfo.Description.Language == language)
                    {
                        yield return fileInfo;
                    }
                }
            }

            //若之前都未成功,则再次返回所有语言包;
            foreach (var fileInfo in LanguagePacks)
            {
                yield return fileInfo;
            }
        }

        [SerializeField]
        private bool IsReadAsSplitPack;

        /// <summary>
        /// 尝试读取到语言包,不返回异常;
        /// </summary>
        private bool TryDeserializePack(LanguagePackSerializer packSerializer, ILanguagePackInfo info, out LanguagePack languagePack)
        {
            using (var stream = info.GetInputStream())
            {
                try
                {
                    if (IsReadAsSplitPack)
                    {
                        languagePack = packSerializer.DeserializeSplit(stream);
                    }
                    else
                    {
                        languagePack = packSerializer.DeserializePack(stream);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.LogWarning(string.Format("读取语言包[{0}]时失败!{1}", info, ex));
                    languagePack = default(LanguagePack);
                    return false;
                }
            }
        }

        /// <summary>
        /// 获取到该模组合适的语言补充包,若不存在则返回null;
        /// </summary>
        private LanguagePackInfo FindSupplementaryPack(LanguagePackSerializer packSerializer, ModificationContent mod, LanguagePackDescription target)
        {
            var supplementaryPackInfo = packSerializer.EnumerateSupplementaryPack(mod.BaseContent, SearchOption.TopDirectoryOnly);
            LanguagePackInfo defaultInfo = null;

            foreach (var packInfo in supplementaryPackInfo)
            {
                if (packInfo.Description.Language == target.Language)
                {
                    return packInfo;
                }
                else if (packInfo.Description.IsDefault)
                {
                    defaultInfo = packInfo;
                }
            }

            return defaultInfo;
        }

        private void AddTo(LanguagePack target, LanguagePack source)
        {
            foreach (var item in source.LanguageDictionary)
            {
                target.LanguageDictionary.Add(item.Key, item.Key);
            }
        }


        //void IBasicResourceInitializeHandle.Initialize(IReadOnlyList<ModificationContent> mods, CancellationToken token)
        //{
        //    token.ThrowIfCancellationRequested();

        //    packSearcher = new LanguagePackSearcher();
        //    packSerializer = new LanguagePackSerializer();
        //    configFileReader = new LocalizationConfigFileReader();

        //    availableLanguagePacks = packSearcher.FindPacks(mods);
        //    if (availableLanguagePacks.Count == 0)
        //    {
        //        throw new FileNotFoundException("未找到合适的语言包文件");
        //    }

        //    LocalizationConfig? config = ReadConfigFile();
        //    LanguagePack languagePack = null;

        //    foreach (var packInfo in EnumerateLanguagePask(config))
        //    {
        //        try
        //        {
        //            Content content = packInfo.Content;
        //            using (var stream = content.GetInputStream(packInfo.RelativePath))
        //            {
        //                languagePack = packSerializer.DeserializePack(stream);
        //                break;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            UnityDebugHelper.LogWarning(InitializerName, "读取语言包时需要错误", ex);
        //        }
        //    }

        //    if (languagePack != null)
        //    {
        //        Localization.SetLanguage(languagePack);
        //    }
        //    else
        //    {
        //        throw new FileNotFoundException("未找到合适的语言包文件");
        //    }

        //    Localization.NotifyLanguageChanged();

        //    UnityDebugHelper.SuccessfulReport(InitializerName, () => GetInfoLog());
        //}

        ///// <summary>
        ///// 读取到配置文件;
        ///// </summary>
        //private LocalizationConfig? ReadConfigFile()
        //{
        //    try
        //    {
        //        return configFileReader.Read();
        //    }
        //    catch (Exception ex)
        //    {
        //        ReadConfigFileException = ex;
        //        return null;
        //    }
        //}

        [ContextMenu("报告详细信息")]
        private void LogInfo()
        {
            Debug.Log(GetInfoLog());
        }

        private string GetInfoLog()
        {
            string log = 
                "当前语言 : " + Localization.LanguagePack.Description.Language
                + ", 系统语言 : " + SystemLanguage.ToString()
                + ", 可读语言 : " + string.Join(", ", Localization.AvailableLanguagePacks.Select(item => string.Format("[Language : {0}, Name : {1}]", item.Description.Language, item.Description.Name)))
                + ", 语言组内容总数 : " + Localization.LanguagePack.LanguageDictionary.Count
                ;
            return log;
        }
    }
}
