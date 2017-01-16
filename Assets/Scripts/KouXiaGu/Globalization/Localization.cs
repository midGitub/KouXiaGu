using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

namespace KouXiaGu.Globalization
{


    /// <summary>
    /// 本地信息提供;
    /// </summary>
    [XmlType("LocalizationConfig"), Serializable]
    public class Localization
    {
        Localization() { }


        const string FILE_NAME = "Localization";
        const string FILE_EXTENSION = ".xml";

        static readonly XmlSerializer serializer = new XmlSerializer(typeof(Localization));


        static string ConfigFilePath
        {
            get { return Path.Combine(Resources.DirectoryPath, FILE_NAME); }
        }


        /// <summary>
        /// 所有存在的语言;
        /// </summary>
        public static List<LanguageFile> ReadOnlyLanguageFiles { get; private set; }

        /// <summary>
        /// 所有存在的语言;
        /// </summary>
        public static List<string> ReadOnlyLanguageNames { get; private set; }

        /// <summary>
        /// 选中的下标;
        /// </summary>
        public static int LanguageIndex { get; private set; }

        /// <summary>
        /// 当前选中的语言,若不存在则返回 null;
        /// </summary>
        public static LanguageFile LanguageFile
        {
            get
            {
                return LanguageIndex >= ReadOnlyLanguageFiles.Count || LanguageIndex < 0 ?
                    null :
                    ReadOnlyLanguageFiles[LanguageIndex];
            }
        }

        /// <summary>
        /// 当前语言信息,若不存在则返回 默认的;
        /// </summary>
        public static Culture Culture
        {
            get
            {
                return LanguageIndex >= ReadOnlyLanguageFiles.Count || LanguageIndex < 0 ?
                    Culture.CurrentLanguage :
                    ReadOnlyLanguageFiles[LanguageIndex].Language;
            }
        }


        /// <summary>
        /// 初始化;
        /// </summary>
        /// <param name="languages">存在的语言信息;</param>
        public static void Initialize(List<LanguageFile> languageFiles)
        {
            ReadOnlyLanguageFiles = languageFiles;
            ReadOnlyLanguageNames = languageFiles.Select(item => item.Language.LanguageName).ToList();
            Localization config = Read();

            LanguageIndex = config.FindIndex(ReadOnlyLanguageNames);
        }

        static Localization Read()
        {
            string filePath = Path.ChangeExtension(ConfigFilePath, FILE_EXTENSION);
            var descr = (Localization)serializer.DeserializeXiaGu(filePath);
            return descr;
        }


        public static void SetLanguage(int index)
        {
            LanguageFile languageFile = ReadOnlyLanguageFiles[index];
            Localization config;
            try
            {
                config = Read();
                config.FirstLanguage = languageFile.Language.LanguageName;
            }
            catch 
            {
                config = new Localization(languageFile.Language.LanguageName);
            }
            Write(config);

            LanguageIndex = index;
        }

        public static void SetLanguage(Localization config)
        {
            Write(config);
            LanguageIndex = config.FindIndex(ReadOnlyLanguageNames);
        }

        static void Write(Localization config)
        {
            string filePath = Path.ChangeExtension(ConfigFilePath, FILE_EXTENSION);
            serializer.SerializeXiaGu(filePath, config);
        }



        public Localization(params string[] languagePrioritys)
        {
            if (languagePrioritys == null)
                throw new ArgumentNullException();
            if (languagePrioritys.Length < 1)
                throw new ArgumentOutOfRangeException();

            this.languagePrioritys = languagePrioritys;
        }

        /// <summary>
        /// 语言读取顺序
        /// </summary>
        [SerializeField]
        string[] languagePrioritys = new string[]
            {
                "English",
                "English",
                "简体中文",
                "中文",
            };


        [XmlArray("LanguagePriority"), XmlArrayItem("Language")]
        public string[] LanguagePrioritys
        {
            get { return languagePrioritys; }
            private set { languagePrioritys = value; }
        }

        [XmlIgnore]
        public string FirstLanguage
        {
            get { return LanguagePrioritys[0]; }
            set { LanguagePrioritys[0] = value; }
        }


        /// <summary>
        /// 获取到优先级最高的语言 下标,若不存在则返回 -1;
        /// </summary>
        /// <param name="languages">存在的语言;</param>
        public int FindIndex(IList<string> languages)
        {
            string[] prioritys = LanguagePrioritys;
            return FindIndex(languages, prioritys);
        }

        /// <summary>
        /// 获取到优先级最高的语言 下标,若不存在则返回 -1;
        /// </summary>
        /// <param name="languages">存在的语言;</param>
        /// <param name="prioritys">优先级降序排序;</param>
        int FindIndex(IList<string> languages, IEnumerable<string> prioritys)
        {
            int index = -1;
            foreach (var priority in prioritys)
            {
                index = languages.IndexOf(priority);
                if (index != -1)
                    return index;
            }
            return index;
        }

    }

}
