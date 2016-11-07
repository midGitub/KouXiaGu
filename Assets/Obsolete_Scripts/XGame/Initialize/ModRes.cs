using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

namespace XGame
{

    /// <summary>
    /// Mod资源目录获取;
    /// </summary>
    [DisallowMultipleComponent]
    public class ModRes : MonoBehaviour
    {
        protected ModRes() { }

        /// <summary>
        /// 游戏Mod所在的目录;
        /// </summary>
        [SerializeField]
        private string modsResPath;

        [SerializeField]
        private string modInfoPath;

        /// <summary>
        /// 获取到所有Mod目录,不分次序;
        /// </summary>
        /// <returns></returns>
        public string[] GetModPaths()
        {
            string modsPath = Path.Combine(Application.dataPath, modsResPath);
            string[] modsPaths = Directory.GetDirectories(modsPath);
            return modsPaths;
        }

        /// <summary>
        /// 获取到所有需要读取的Mod;
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ModInfo> GetModInfos()
        {
            foreach (var modPath in GetModPaths())
            {
                ModInfo modInfo = GetModInfo(modPath);
                if (modInfo != null)
                    yield return modInfo;
            }
        }

        public ModInfo GetModInfo(string modPath)
        {
            string filepath = Path.Combine(modPath, modInfoPath);
            if (!File.Exists(filepath))
                throw new Exception("缺少模组描述文件!");

            XElement modInfoElement = XElement.Load(filepath);
            XElement modName = modInfoElement.Element("Name");

            if (modName == null)
            {
                Debug.LogWarning("未指定Mod名!跳过目录 :" + modPath);
                return null;
            }
            else
            {
                return new ModInfo(modName.Value, modPath);
            }
        }

    }

}
