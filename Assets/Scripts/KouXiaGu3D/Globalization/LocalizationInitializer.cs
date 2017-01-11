
//#define USE_THREAD

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Initialization;
using UnityEngine;
using System.Threading;

namespace KouXiaGu.Globalization
{

    /// <summary>
    /// 负责游戏一开始对本地化信息初始化;
    /// </summary>
    public class LocalizationInitializer : MonoBehaviour, IOperateAsync
    {

        public bool IsCompleted { get; private set; }

        public bool IsFaulted { get; private set; }

        public Exception Ex { get; private set; }


        void Start()
        {
#if USE_THREAD
            ThreadPool.QueueUserWorkItem(_ => FirstRead());
#else
            FirstRead();
#endif
        }


        void FirstRead()
        {
            try
            {
                FirstReadTexts();
            }
            catch (Exception e)
            {
                IsFaulted = true;
                Ex = e;
            }

            IsCompleted = true;
        }

        static void FirstReadTexts()
        {
            List<LanguageFile> ReadOnlyLanguageFiles = Resources.FindLanguageFiles().ToList();
            Localization.Initialize(ReadOnlyLanguageFiles);

            if (Localization.LanguageFile == null)
            {
                throw new KeyNotFoundException("没有合适的语言文件;");
            }

            LocalizationText.UpdateTextDictionary(Localization.LanguageFile.ReadTexts());
        }


        public static void Read()
        {
#if USE_THREAD
            ThreadPool.QueueUserWorkItem(_ => ReadTexts());
#else
            ReadTexts();
#endif
        }

        static void ReadTexts()
        {
            LocalizationText.UpdateTextDictionary(Localization.LanguageFile.ReadTexts());
        }


        //class Reader : CustomYieldInstruction
        //{
        //    public Reader()
        //    {
        //        _keepWaiting = true;
        //        ThreadPool.QueueUserWorkItem(_ => ReadTexts());
        //    }

        //    public Reader(LanguageFile file)
        //    {
        //        _keepWaiting = true;
        //        ThreadPool.QueueUserWorkItem(_ => ReadTexts(file));
        //    }

        //    bool _keepWaiting;

        //    public override bool keepWaiting
        //    {
        //        get { return _keepWaiting; }
        //    }

        //    void ReadTexts()
        //    {
        //        List<LanguageFile> ReadOnlyLanguageFiles = Resources.FindLanguageFiles().ToList();
        //        Localization.Initialize(ReadOnlyLanguageFiles);
        //        ReadTexts(Localization.LanguageFile);
        //    }

        //    void ReadTexts(LanguageFile file)
        //    {
        //        LocalizationText.UpdateTextDictionary(file.ReadTexts());
        //    }

        //}

    }

}
