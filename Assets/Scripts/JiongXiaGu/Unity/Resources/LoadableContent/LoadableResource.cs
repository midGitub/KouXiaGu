using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 可读资源定义(非线程安全);
    /// </summary>
    public static class LoadableResource
    {
        /// <summary>
        /// 资源共享合集;
        /// </summary>
        public static SharedContent SharedContent { get; private set; } = new SharedContent();

        /// <summary>
        /// 核心资源;
        /// </summary>
        public static LoadableContent Core { get; private set; } 

        /// <summary>
        /// 所有资源;
        /// </summary>
        private static readonly List<LoadableContent> all = new List<LoadableContent>();

        /// <summary>
        /// 所有资源;
        /// </summary>
        public static IReadOnlyCollection<LoadableContent> All => all;

        /// <summary>
        /// 资源读取顺序;
        /// </summary>
        public static LoadOrder Order { get; private set; }

        /// <summary>
        /// 获取到所有可读取的资源;
        /// </summary>
        internal static async Task Initialize()
        {
            await Task.Run(delegate ()
            {
                LoadableContentSearcher contentSearcher = new LoadableContentSearcher();
                Core = GetCore(contentSearcher.Factory);
                all.Add(Core);

                var mods = contentSearcher.Find(Resource.ModDirectory);
                all.AddRange(mods);

                var userMods = contentSearcher.Find(Resource.UserModDirectory);
                all.AddRange(userMods);
            });

            await Core.LoadAllAssetBundlesAsync();
        }

        /// <summary>
        /// 获取到核心资源;
        /// </summary>
        private static LoadableContent GetCore(LoadableContentFactory factory)
        {
            string directory = Path.Combine(Resource.StreamingAssetsPath, "Data");
            var core = factory.Read(directory);
            return core;
        }

        /// <summary>
        /// 设置资源读取顺序;
        /// </summary>
        internal static void SetOrder(LoadOrder order)
        {
            throw new NotImplementedException();
        }
    }
}
