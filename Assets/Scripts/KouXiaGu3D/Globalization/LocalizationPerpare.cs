using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Initialization;
using UnityEngine;

namespace KouXiaGu.Globalization
{

    /// <summary>
    /// 负责游戏一开始对本地化信息初始化;
    /// </summary>
    public class LocalizationPerpare : MonoBehaviour, IOperateAsync
    {

        public bool IsCompleted { get; private set; }

        public bool IsFaulted { get; private set; }

        public Exception Ex { get; private set; }


        void Start()
        {
            try
            {
                List<LanguageFile> ReadOnlyLanguageFiles = Resources.FindLanguageFiles().ToList();
                Localization.Initialize(ReadOnlyLanguageFiles);
                ReadTexts();
            }
            catch (Exception e)
            {
                IsFaulted = true;
                Ex = e;
            }

            IsCompleted = true;
        }

        /// <summary>
        /// 根据本地化信息读取文本;
        /// </summary>
        public static void ReadTexts()
        {
            LanguageFile file = Localization.LanguageFile;
            if (file == null)
            {
                Debug.LogError("无法读取!");
            }
            ReadTexts(file);
        }

        public static void ReadTexts(LanguageFile file)
        {
            LocalizationText.UpdateTextDictionary(file.ReadTexts());
        }

    }

}
