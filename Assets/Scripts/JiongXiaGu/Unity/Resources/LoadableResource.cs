using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 资源定义;
    /// </summary>
    public static class LoadableResource
    {
        private static List<LoadableContent> contents;

        /// <summary>
        /// 核心资源;
        /// </summary>
        public static LoadableContent Core
        {
            get { return contents[0]; }
        }

        /// <summary>
        /// 所有资源;
        /// </summary>
        public static IReadOnlyCollection<LoadableContent> All
        {
            get { return contents; }
        }

        /// <summary>
        /// 初始化资源信息;
        /// </summary>
        internal static void Initialize()
        {
            LoadableContentSearcher contentSearcher = new LoadableContentSearcher();
            var core = contentSearcher.Factory.Read(Resource.CoreDirectory, true);
            var dlc = contentSearcher.Find(Resource.DlcDirectory);
            var mod = contentSearcher.Find(Resource.ModDirectory);
            contents = new List<LoadableContent>();
            contents.Add(core);
            contents.AddRange(dlc);
            contents.AddRange(mod);
        }

        /// <summary>
        /// 获取到核心资源;
        /// </summary>
        public static LoadableContent GetCore(LoadableContentFactory factory)
        {
            var core = factory.Read(Resource.CoreDirectory, true);
            return core;
        }
    }
}
