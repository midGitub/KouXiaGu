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
        public static List<XmlLanguageFile> ReadOnlyLanguageFiles { get; private set; }

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
        public static XmlLanguageFile LanguageFile
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
        public static Language Language
        {
            get
            {
                return LanguageIndex >= ReadOnlyLanguageFiles.Count || LanguageIndex < 0 ?
                    Language.CurrentLanguage :
                    ReadOnlyLanguageFiles[LanguageIndex].Language;
            }
        }


        /// <summary>
        /// 初始化;
        /// </summary>
        /// <param name="languages">存在的语言信息;</param>
        public static void Initialize(List<XmlLanguageFile> languageFiles)
        {
            ReadOnlyLanguageFiles = languageFiles;
            ReadOnlyLanguageNames = languageFiles.Select(item => item.Language.LanguageName).ToList();
            Localization config = Read(ConfigFilePath);

            LanguageIndex = config.FindIndex(ReadOnlyLanguageNames);
        }

        static Localization Read(string filePath)
        {
            filePath = Path.ChangeExtension(filePath, FILE_EXTENSION);
            var descr = (Localization)serializer.DeserializeXiaGu(filePath);
            return descr;
        }


        public static void SetLanguage(int index)
        {
            XmlLanguageFile file = ReadOnlyLanguageFiles[index];
            Localization config = new Localization(file.Language.LanguageName);
            Write(ConfigFilePath, config);

            LanguageIndex = index;
        }

        public static void SetLanguage(Localization config)
        {
            Write(ConfigFilePath, config);
            LanguageIndex = config.FindIndex(ReadOnlyLanguageNames);
        }

        static void Write(string filePath, Localization config)
        {
            filePath = Path.ChangeExtension(filePath, FILE_EXTENSION);
            serializer.SerializeXiaGu(filePath, config);
        }






        public Localization(string[] languagePrioritys)
        {
            if (languagePrioritys == null)
                throw new ArgumentNullException();
            if (languagePrioritys.Length < 1)
                throw new ArgumentOutOfRangeException();

            this.languagePrioritys = languagePrioritys;
        }

        public Localization(string firstLanguage)
        {
            this.FirstLanguage = firstLanguage;
        }

        /// <summary>
        /// 语言读取顺序
        /// </summary>
        [SerializeField]
        string[] languagePrioritys = new string[]
            {
                "English",
                "English",
                "ChineseSimplified",
                "Chinese",
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
