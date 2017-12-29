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
        /// <summary>
        /// 资源共享合集;
        /// </summary>
        public static SharedContent SharedContent { get; private set; } = new SharedContent();

        /// <summary>
        /// 核心资源;
        /// </summary>
        public static Lazy<LoadableContent> Core { get; private set; } = new Lazy<LoadableContent>(delegate()
        {
            LoadableContentFactory factory = new LoadableContentFactory();
            var core = factory.Read(Resource.CoreDirectory);
            SharedContent.Add(core);
            return core;
        }, true);

        /// <summary>
        /// 用户配置文件;
        /// </summary>
        public static Lazy<Content> UserConfig { get; private set; } = new Lazy<Content>(() => new ContentDirectory(Resource.UserConfigDirectory), true);

        private static List<LoadableContent> all;

        /// <summary>
        /// 所有自定义的资源;
        /// </summary>
        public static IReadOnlyCollection<LoadableContent> All => all;

        internal static void Initialize()
        {

        }

        /// <summary>
        /// 找到所有可以读取的资源;
        /// </summary>
        internal static void FindResource()
        {
            LoadableContentSearcher contentSearcher = new LoadableContentSearcher();
            var dlc = contentSearcher.Find(Resource.DlcDirectory);
            var mod = contentSearcher.Find(Resource.ModDirectory);
            all = new List<LoadableContent>();
            all.Add(Core.Value);
            all.AddRange(dlc);
            all.AddRange(mod);
        }
    }
}
