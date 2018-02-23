using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 负责对可读的资源进行搜索;
    /// </summary>
    public class ModificationSearcher
    {
        public ModificationFactory Factory { get; private set; }

        public ModificationSearcher()
        {
            Factory = new ModificationFactory();
        }

        public ModificationSearcher(ModificationFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            Factory = factory;
        }

        /// <summary>
        /// 获取目录下的所有可读资源;
        /// </summary>
        /// <param name="modsDirectory">目标目录</param>
        public List<ModificationInfo> Searche(string modsDirectory)
        {
            var list = new List<ModificationInfo>();

            list.AddRange(EnumerateDirectory(modsDirectory));
            return list;
        }

        public IEnumerable<ModificationInfo> Enumerate(string modsDirectory)
        {
            return EnumerateDirectory(modsDirectory);
        }

        /// <summary>
        /// 枚举目录下所有 目录 类型的资源;
        /// </summary>
        public IEnumerable<ModificationInfo> EnumerateDirectory(string modsDirectory, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            foreach (var directory in Directory.EnumerateDirectories(modsDirectory, "*", searchOption))
            {
                string directoryName = Path.GetFileName(directory);
                if (!SearcheHelper.IsIgnore(directoryName))
                {
                    ModificationInfo info;

                    try
                    {
                        info = Factory.ReadInfo(directory);
                    }
                    catch
                    {
                        continue;
                    }

                    yield return info;
                }
            }
        }

        ///// <summary>
        ///// 枚举目录下所有 压缩包 类型的资源;
        ///// </summary>
        //[Obsolete]
        //public IEnumerable<ModificationInfo> EnumerateZip(string modsDirectory, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        //{
        //    foreach (var filePath in Directory.EnumerateFiles(modsDirectory, "*.zmod", searchOption))
        //    {
        //        string fileName = Path.GetFileName(filePath);
        //        if (!Ignore(fileName))
        //        {
        //            ModificationInfo info;

        //            try
        //            {
        //                info = Factory.ReadZipInfo(filePath);
        //            }
        //            catch(Exception ex)
        //            {
        //                Debug.LogWarning(string.Format("[读取模组失败]Path : {0}, Exception : {1}", filePath, ex));
        //                continue;
        //            }

        //            yield return info;
        //        }
        //    }
        //}
    }
}
