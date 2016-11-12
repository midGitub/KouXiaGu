using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Linq;
using System.Linq;

namespace KouXiaGu
{

    /// <summary>
    /// 初始化游戏使用的信息;
    /// </summary>
    public struct ModGroup
    {
        public ModGroup(ModInfo[] modIReadingOrder)
        {
            this.ModIReadingOrder = modIReadingOrder;
        }

        /// <summary>
        /// 需要读取的模组,按读取顺序升序排列;
        /// </summary>
        public ModInfo[] ModIReadingOrder { get; private set; }

    }

    /// <summary>
    /// 单个模组信息;
    /// </summary>
    public class ModInfo
    {
        private ModInfo() { }

        private ModInfo(string modDirectoryPath, XElement modInfoElement)
        {
            this.ModDirectoryPath = modDirectoryPath;
            this.ModInfoElement = modInfoElement;
            LoadFromXML(modInfoElement);
        }

        public string ModDirectoryPath { get; private set; }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public float Version { get; private set; }

        public XElement ModInfoElement { get; private set; }

        public static ModInfo Load(string modDirectoryPath, string modInfoFilePath)
        {
            XElement modInfoElement = XElement.Load(modInfoFilePath);
            return Load(modDirectoryPath, modInfoElement);
        }

        public static ModInfo Load(string modDirectoryPath, XElement modInfoElement)
        {
            ModInfo modInfo = new ModInfo(modDirectoryPath, modInfoElement);
            return modInfo;
        }

        private void LoadFromXML(XElement modInfoElement)
        {
            XElement Properties = modInfoElement.Element("Properties");

            this.Name = (string)Properties.Element("Name");
            this.Description = (string)Properties.Element("Description");
            this.Version = (float)Properties.Element("Version");
        }


        public override bool Equals(object obj)
        {
            if (!(obj is ModInfo))
                return false;
            return (obj as ModInfo).Name == Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

    }

    /// <summary>
    /// 模组信息管理;
    /// </summary>
    [Serializable]
    public sealed class ModData
    {
        private ModData() { }

        [SerializeField, Tooltip("模组资源目录")]
        private string modsDirectory;

        [SerializeField, Tooltip("模组信息文件路径")]
        private string modInfoFileName;

        [SerializeField, Tooltip("读取模组顺序的文件路径")]
        private string readingOrderFilePath;


        private ModInfo[] modIReadingOrder;


        public ModGroup GetModGroup()
        {
            if (modIReadingOrder == null)
            {
                modIReadingOrder = GetDefaultModReadingOrder();
            }
            return new ModGroup(modIReadingOrder);
        }


        #region ReadingOrder;

        /// <summary>
        /// 获取到模组读取顺序;
        /// </summary>
        public ModInfo[] GetModReadingOrder()
        {
            return this.modIReadingOrder;
        }

        /// <summary>
        /// 设置模组读取顺序为;
        /// </summary>
        public void SetModReadingOrder(ModInfo[] modIReadingOrder)
        {
            this.modIReadingOrder = modIReadingOrder;
            SaveDefaultModReadingOrder(modIReadingOrder);
        }

        /// <summary>
        /// 获取到默认的模组读取顺序;
        /// </summary>
        public ModInfo[] GetDefaultModReadingOrder()
        {
            string defaultReadingOrderFilePath = GetDefaultReadingOrderFilePath();
            Dictionary<string, ModInfo> existingModInfos = GetModInfos();

            try
            {
                var readingOrderElement = XElement.Load(defaultReadingOrderFilePath);
                return ReadDefaultModReadingOrder(readingOrderElement, existingModInfos).ToArray();
            }
            catch (FileNotFoundException)
            {
                Debug.LogWarning("找不到默认读取顺序,返回读取所有模组!");
                return existingModInfos.Values.ToArray();
            }
        }

        /// <summary>
        /// 将 默认读取顺序 设置为;
        /// </summary>
        public void SaveDefaultModReadingOrder(ModInfo[] modIReadingOrder)
        {
            var existingModInfos = GetModInfos();
            XElement modIReadingOrderXml = GetDefaultModReadingOrderXML(modIReadingOrder, existingModInfos);

            string defaultReadingOrderFilePath = GetDefaultReadingOrderFilePath();
            modIReadingOrderXml.Save(defaultReadingOrderFilePath);
        }

        /// <summary>
        /// 根据现在存在的Mod 和 XML文件 获取到模组读取顺序;
        /// </summary>
        private IEnumerable<ModInfo> ReadDefaultModReadingOrder(XElement readingOrderElement,
            Dictionary<string, ModInfo> existingModInfos)
        {
            ModInfo modInfo;
            foreach (var mod in readingOrderElement.Elements("Mod"))
            {
                string modName = (string)mod.Attribute("name");
                if (existingModInfos.TryGetValue(modName, out modInfo))
                {
                    yield return modInfo;
                }
            }
        }
        
        /// <summary>
        /// 获取到保存为 XML 的节点格式(未持续化保存);
        /// </summary>
        private XElement GetDefaultModReadingOrderXML(ModInfo[] modIReadingOrder, Dictionary<string, ModInfo> existingModInfos)
        {
            XElement readingOrderElement = new XElement("ModReadingOrder");

            foreach (var modInfo in modIReadingOrder)
            {
                if (existingModInfos.ContainsKey(modInfo.Name))
                {
                    readingOrderElement.Add(new XElement("Mod", new XAttribute("name", modInfo.Name)));
                }
            }

            return readingOrderElement;
        }

        private string GetDefaultReadingOrderFilePath()
        {
            string defaultReadingOrderFilePath = Path.Combine(Application.dataPath, readingOrderFilePath);
            return defaultReadingOrderFilePath;
        }

        #endregion

        #region ModInfo

        public Dictionary<string, ModInfo> GetModInfos()
        {
            Dictionary<string, ModInfo> modInfos = new Dictionary<string, ModInfo>();
            IEnumerable<string> AllModDirectoryPaths = GetModDirectoryPaths();
            foreach (var modDirectoryPath in AllModDirectoryPaths)
            {
                ModInfo modInfo;
                if (TryGetModInfo(modDirectoryPath, out modInfo))
                {
                    try
                    {
                        modInfos.Add(modInfo.Name, modInfo);
                    }
                    catch (ArgumentException)
                    {
                        continue;
                    }
                }
            }
            return modInfos;
        }

        /// <summary>
        /// 获取到所有存在于Mods文件夹下的文件夹,不做检查;
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> GetModDirectoryPaths()
        {
            string[] modDirectoryPaths = Directory.GetDirectories(GetModsDirectoryPath());

            foreach (var modDirectoryPath in modDirectoryPaths)
            {
                yield return modDirectoryPath;
            }
        }

        private bool TryGetModInfo(string modDirectoryPath, out ModInfo modInfo)
        {
            string modInfoFilePath;
            if (TryGetModInfoFilePath(modDirectoryPath,out modInfoFilePath))
            {
                modInfo = ModInfo.Load(modDirectoryPath, modInfoFilePath);
                return true;
            }
            modInfo = default(ModInfo);
            return false;
        }

        private string GetModsDirectoryPath()
        {
            string modsDirectoryPath = Path.Combine(Application.dataPath, modsDirectory);
            return modsDirectoryPath;
        }

        private string GetModInfoFilePath(string modPath)
        {
            string modInfoFilePath = Path.Combine(modPath, modInfoFileName);
            return modInfoFilePath;
        }

        private bool TryGetModInfoFilePath(string modPath, out string modInfoFilePath)
        {
            modInfoFilePath = GetModInfoFilePath(modPath);
            return File.Exists(modInfoFilePath);
        }

        #endregion

    }

}
