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
        /// <summary>
        /// 忽略符,置于名称前缀,用于忽略某文件/文件夹;
        /// </summary>
        private const string IgnoreSymbol = "#ignore_";
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
            list.AddRange(EnumerateZip(modsDirectory));

            return list;
        }

        /// <summary>
        /// 枚举目录下所有 目录 类型的资源;
        /// </summary>
        public IEnumerable<ModificationInfo> EnumerateDirectory(string modsDirectory, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            foreach (var directory in Directory.EnumerateDirectories(modsDirectory, "*", searchOption))
            {
                string directoryName = Path.GetFileName(directory);
                if (!Ignore(directoryName))
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

        /// <summary>
        /// 枚举目录下所有 压缩包 类型的资源;
        /// </summary>
        public IEnumerable<ModificationInfo> EnumerateZip(string modsDirectory, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            foreach (var filePath in Directory.EnumerateFiles(modsDirectory, "*.zmod", searchOption))
            {
                string fileName = Path.GetFileName(filePath);
                if (!Ignore(fileName))
                {
                    ModificationInfo info;

                    try
                    {
                        info = Factory.ReadZipInfo(filePath);
                    }
                    catch(Exception ex)
                    {
                        Debug.LogWarning(string.Format("[读取模组失败]Path : {0}, Exception : {1}", filePath, ex));
                        continue;
                    }

                    yield return info;
                }
            }
        }

        private bool Ignore(string name)
        {
            return name.StartsWith(IgnoreSymbol, StringComparison.OrdinalIgnoreCase);
        }


        ///// <summary>
        ///// 获取目录下的所有可读资源;
        ///// </summary>
        ///// <param name="modsDirectory">目标目录</param>
        ///// <param name="type">指定找到的模组类型</param>
        ///// <returns></returns>
        //public List<ModificationContent> Find(string modsDirectory)
        //{
        //    List<ModificationContent> list = new List<ModificationContent>();

        //    list.AddRange(EnumerateDirectory(modsDirectory));
        //    list.AddRange(EnumerateZipFile(modsDirectory));

        //    return list;
        //}


        ///// <summary>
        ///// 枚举目录下所有 目录 类型的资源;
        ///// </summary>
        //public IEnumerable<ModificationContent> EnumerateDirectory(string modsDirectory)
        //{
        //    foreach (var directory in Directory.EnumerateDirectories(modsDirectory, "*", SearchOption.TopDirectoryOnly))
        //    {
        //        string directoryName = Path.GetDirectoryName(directory);
        //        if (directoryName.StartsWith(IgnoreSymbol, StringComparison.OrdinalIgnoreCase))
        //        {
        //            continue;
        //        }

        //        ModificationContent info = null;

        //        try
        //        {
        //            info = Factory.Read(directory);
        //        }
        //        catch(Exception)
        //        {
        //            continue;
        //        }

        //        yield return info;
        //    }
        //}

        ///// <summary>
        ///// 枚举目录下所有 压缩包 类型的资源;
        ///// </summary>
        //public IEnumerable<ModificationContent> EnumerateZipFile(string modsDirectory)
        //{
        //    foreach (var filePath in Directory.EnumerateFiles(modsDirectory, "*.zmod", SearchOption.AllDirectories))
        //    {
        //        string fileName = Path.GetFileName(filePath);
        //        if (fileName.StartsWith(IgnoreSymbol, StringComparison.OrdinalIgnoreCase))
        //        {
        //            continue;
        //        }

        //        ModificationContent info = null;

        //        try
        //        {
        //            info = Factory.ReadZip(filePath);
        //        }
        //        catch(Exception)
        //        {
        //            continue;
        //        }

        //        yield return info;
        //    }
        //}
    }
}
