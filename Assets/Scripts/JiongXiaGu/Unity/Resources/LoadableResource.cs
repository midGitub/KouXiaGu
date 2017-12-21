using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 可读资源定义;
    /// </summary>
    public static class LoadableResource
    {
        private static LoadableContentSearcher contentSearcher = new LoadableContentSearcher();

        /// <summary>
        /// 资源共享合集;
        /// </summary>
        public static SharedContent SharedContent { get; private set; } = new SharedContent();

        /// <summary>
        /// 核心资源;
        /// </summary>
        public static Lazy<LoadableContent> Core { get; private set; } = new Lazy<LoadableContent>(() => contentSearcher.Factory.Read(Resource.CoreDirectory, true), true);

        /// <summary>
        /// 用户配置文件;
        /// </summary>
        public static Lazy<Content> UserConfig { get; private set; } = new Lazy<Content>(() => new ContentDirectory(Resource.UserConfigDirectory), true);

        private static List<LoadableContent> contents;

        /// <summary>
        /// 所有自定义的资源;
        /// </summary>
        public static IReadOnlyCollection<LoadableContent> All => contents;

        /// <summary>
        /// 初始化资源信息;
        /// </summary>
        internal static void Initialize()
        {
            var dlc = contentSearcher.Find(Resource.DlcDirectory);
            var mod = contentSearcher.Find(Resource.ModDirectory);
            contents = new List<LoadableContent>();
            contents.AddRange(dlc);
            contents.AddRange(mod);
        }
    }
}
