using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 关联的资源,处理资源共享的 AssetBundle 和 其它资源;(线程安全);
    /// </summary>
    public class SharedContent
    {
        /// <summary>
        /// 所有关联的资源;
        /// </summary>
        private BlockingCollection<Content> contentCollection = new BlockingCollection<Content>();

        /// <summary>
        /// 添加新的资源;
        /// </summary>
        internal Content Add(LoadableContent loadableContent)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 移除资源,若移除成功则返回true;
        /// </summary>
        internal bool Remove(LoadableContent loadableContent)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 获取到对应的 AssetBundle,若还未读取或不存在则返回null;
        /// </summary>
        public AssetBundle GetAssetBundle(string key)
        {
            throw new NotImplementedException();
        }

        private const char AssetBundleKeyFristChar = '@';
        private const char AssetBundleKeySeparator = ':';


        public class Content
        {
            public LoadableContent LoadableContent { get; private set; }

            /// <summary>
            /// 延迟实例化,若不存在AssetBundle则为Null;
            /// </summary>
            public List<KeyValuePair<string, AssetBundle>> AssetBundles { get; private set; }

            public Content(LoadableContent loadableContent)
            {
                LoadableContent = loadableContent;
            }

            public void Add(string name, AssetBundle assetBundle)
            {
                throw new NotImplementedException();
            }
        }
    }
}
