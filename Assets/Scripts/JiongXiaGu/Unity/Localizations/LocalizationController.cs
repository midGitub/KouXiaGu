﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using JiongXiaGu.Unity.Resources;
using System.Collections;
using JiongXiaGu.Unity.Initializers;

namespace JiongXiaGu.Unity.Localizations
{

    /// <summary>
    /// 提供在Unity编辑器内调整具体参数;
    /// </summary>
    [DisallowMultipleComponent]
    class LocalizationController : MonoBehaviour
    {
        private const string InitializerName = "游戏组件初始化";

        /// <summary>
        /// 默认语言,在找不到指定语言和系统语言时使用的语言;
        /// </summary>
        [SerializeField]
        private SystemLanguage defaultLanguage;
        private LanguagePackReader languagePackReader;
        private Task<LanguagePackGroup> readLanguagePackTask;
        public SystemLanguage SystemLanguage { get; private set; }

        private void Awake()
        {
            languagePackReader = new LanguagePackReader();
            SystemLanguage = Application.systemLanguage;
        }

        private void Update()
        {
            if (readLanguagePackTask != null && readLanguagePackTask.IsCompleted)
            {
                if (readLanguagePackTask.IsCompleted && !readLanguagePackTask.IsFaulted)
                {
                    Localization.SetLanguage(readLanguagePackTask.Result);
                }
                readLanguagePackTask = null;
            }
        }

        //Task IGameComponentInitializeHandle.Initialize(CancellationToken token)
        //{
        //    readLanguagePackTask = ReadLanguagePack(token);
        //    var waitReadedTask = readLanguagePackTask.ContinueWith(delegate (Task task)
        //    {
        //        if (task.IsFaulted)
        //        {
        //            throw task.Exception;
        //        }
        //        while (readLanguagePackTask != null)
        //        {
        //            token.ThrowIfCancellationRequested();
        //        }
        //        InitializerHelper.LogComplete(InitializerName, GetInfoLog());
        //    });
        //    return waitReadedTask;
        //}

        private string GetInfoLog()
        {
            string log = "语言:" + Localization.Language
                + ", 系统语言::" + SystemLanguage.ToString()

                + "\n文件总数:" + Localization.LanguagePackGroup.Count
                + ", 条目总数:" + Localization.LanguagePackGroup.TextItemCount()
                ;
            return log;
        }

        [ContextMenu("报告详细信息")]
        private void LogInfo()
        {
            Debug.Log(GetInfoLog());
        }

        /// <summary>
        /// 读取到语言文件,完成时调用 callback(无论是否失败);
        /// </summary>
        public void ReadLanguagePack(LanguagePackFileInfo languagePackFileInfo, Action<Task> callback = null)
        {
            throw new NotImplementedException();
            //var task = Task.Run(delegate ()
            //{
            //    var languagePack = languagePackReader.Read(languagePackFileInfo);
            //    return new LanguagePackGroup(languagePack);
            //});
            //StartCoroutine(WaitLanguagePackCoroutine(task, callback));
        }

        ///// <summary>
        ///// 等待读取完成,并且应用到本地化组件(若读取失败则不进行此操作),完成时调用 callback(无论是否失败);
        ///// </summary>
        //private IEnumerator WaitLanguagePackCoroutine(Task<LanguagePackGroup> languagePackTask, Action<Task> callback = null)
        //{
        //    while (!languagePackTask.IsCompleted)
        //    {
        //        yield return null;
        //    }
        //    if (!languagePackTask.IsFaulted)
        //    {
        //        Localization.SetLanguage(languagePackTask.Result);
        //    }
        //    callback?.Invoke(languagePackTask);
        //}

        /// <summary>
        /// 读取到预定义的语言;
        /// </summary>
        private Task<LanguagePackGroup> ReadLanguagePack(CancellationToken token)
        {
            return Task.Run(delegate ()
            {
                token.ThrowIfCancellationRequested();

                LocalizationConfig config;
                IEnumerable<LanguagePackFileInfo> targetFileInfos;

                if (TryReadLocalizationConfig(out config))
                {
                    targetFileInfos = EnumerateLanguagePask(config);
                }
                else
                {
                    targetFileInfos = EnumerateLanguagePask();
                }

                LanguagePack targetPack;
                foreach (var targetFileInfo in targetFileInfos)
                {
                    token.ThrowIfCancellationRequested();
                    try
                    {
                        targetPack = languagePackReader.Read(targetFileInfo);
                        LanguagePackGroup languagePackGroup = new LanguagePackGroup(targetPack);
                        return languagePackGroup;
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(string.Format( "读取语言包{0}时发生异常:{1}", targetFileInfo, ex));
                        continue;
                    }
                }

                throw new FileNotFoundException("未找到合适的语言文件;");
            }, token);
        }

        /// <summary>
        /// 尝试读取到配置文件;
        /// </summary>
        private bool TryReadLocalizationConfig(out LocalizationConfig config)
        {
            LocalizationConfigReader configReader = new LocalizationConfigReader();
            try
            {
                config = configReader.Read();
                return true;
            }
            catch (FileNotFoundException)
            {
                config = default(LocalizationConfig);
                return false;
            }
        }

        /// <summary>
        /// 枚举所有符合要求的语言包;
        /// </summary>
        private IEnumerable<LanguagePackFileInfo> EnumerateLanguagePask()
        {
            string systemLanguage = SystemLanguage.ToString();
            Func<LanguagePackFileInfo, bool> targetMatch = delegate (LanguagePackFileInfo info)
            {
                return info.Language == systemLanguage;
            };

            var spareLanguages = new string[]
            {
                defaultLanguage.ToString(),
            };

            IEnumerable<LanguagePackFileInfo> fileInfos = LanguagePackReader.EnumerateInfos();
            return FindLanguagePask(fileInfos, targetMatch, spareLanguages);
        }

        /// <summary>
        /// 枚举所有符合要求的语言包;
        /// </summary>
        private IEnumerable<LanguagePackFileInfo> EnumerateLanguagePask(LocalizationConfig config)
        {
            Func<LanguagePackFileInfo, bool> targetMatch = delegate (LanguagePackFileInfo info)
            {
                return config.LanguagePackInfo == info;
            };

            var spareLanguages = new string[]
            {
                SystemLanguage.ToString(),
                defaultLanguage.ToString(),
            };

            IEnumerable<LanguagePackFileInfo> fileInfos = LanguagePackReader.EnumerateInfos();
            return FindLanguagePask(fileInfos, targetMatch, spareLanguages);
        }

        /// <summary>
        /// 获取到指定的语言,获取优先值按数组排列顺序;
        /// </summary>
        /// <param name="fileInfos">所以可用的语言信息</param>
        /// <param name="targetMatch">优选语言筛选器</param>
        /// <param name="spareLanguages">备选语言合集(优先级为数组排列顺序)</param>
        private IEnumerable<LanguagePackFileInfo> FindLanguagePask(IEnumerable<LanguagePackFileInfo> fileInfos, Func<LanguagePackFileInfo, bool> targetMatch, params string[] spareLanguages)
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
    }
}