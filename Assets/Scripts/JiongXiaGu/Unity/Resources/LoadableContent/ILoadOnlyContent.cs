using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 多线程安全的资源;
    /// </summary>
    public interface ILoadableContent : IDisposable
    {
        LoadableContentDescription Description { get; }
        LoadableContentDescription? NewDescription { get; }

        IEnumerable<string> ConcurrentEnumerateFiles();
        IEnumerable<string> ConcurrentEnumerateFiles(string searchPattern, SearchOption searchOption);
        IEnumerable<string> ConcurrentEnumerateFiles(string directoryName, string searchPattern, SearchOption searchOption);
        string ConcurrentFind(Func<string, bool> func);
        Stream ConcurrentGetInputStream(string relativePath);

        /// <summary>
        /// 获取到对应的 AssetBundle,若还未读取则返回异常;
        /// </summary>
        AssetBundle GetAssetBundle(AssetPath path);

        /// <summary>
        /// 获取到对应的 AssetBundle,若还未读取,则读取并返回,若不存在 AssetBundle 或读取失败则返回异常;(线程安全)
        /// </summary>
        AssetBundle GetOrLoadAssetBundle(AssetPath path);

        /// <summary>
        /// 获取到对应的 AssetBundle,若还未读取则返回异常;(线程安全)
        /// </summary>
        AssetBundle GetAssetBundle(string assetBundleName);

        /// <summary>
        /// 读取到指定的 AssetBundle,若未找到则返回异常;(线程安全)
        /// </summary>
        AssetBundle GetOrLoadAssetBundle(string assetBundleName);
    }
}
