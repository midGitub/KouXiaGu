using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 资源定义;实例非线程安全;
    /// </summary>
    public class LoadableResource
    {
        private static LoadableContent core;
        private static List<LoadableContent> dlc;
        private static List<LoadableContent> mod;
        private static List<LoadableContent> all;

        /// <summary>
        /// 核心资源;
        /// </summary>
        public static LoadableContent Core
        {
            get { return core; }
        }

        /// <summary>
        /// 所有拓展资源;
        /// </summary>
        public static IReadOnlyCollection<LoadableContent> Dlc
        {
            get { return dlc; }
        }

        /// <summary>
        /// 所有模组资源;
        /// </summary>
        public static IReadOnlyCollection<LoadableContent> Mod
        {
            get { return mod; }
        }

        /// <summary>
        /// 所有资源;
        /// </summary>
        public static IReadOnlyCollection<LoadableContent> All
        {
            get { return all; }
        }

        /// <summary>
        /// 初始化资源信息;
        /// </summary>
        internal static void Initialize()
        {
            LoadableContentSearcher contentSearcher = new LoadableContentSearcher();
            core = GetCore(contentSearcher.Factory);
            dlc = GetDlc(contentSearcher);
            mod = GetMod(contentSearcher);
            all = GetAll();
        }

        internal static void Quit()
        {
            foreach (var item in all)
            {
                try
                {
                    item.Dispose();
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError(ex);
                }
            }
        }

        public static LoadableContent GetCore(LoadableContentFactory factory)
        {
            return factory.Read(Resource.CoreDirectory);
        }

        public static List<LoadableContent> GetDlc(LoadableContentSearcher contentSearcher)
        {
            return contentSearcher.FindLoadableContent(Resource.DlcDirectory);
        }

        public static List<LoadableContent> GetMod(LoadableContentSearcher contentSearcher)
        {
            return contentSearcher.FindLoadableContent(Resource.ModDirectory);
        }

        private static List<LoadableContent> GetAll()
        {
            int capacity = 1 + dlc.Count + mod.Count;
            var all = new List<LoadableContent>(capacity);
            all.Add(Core);
            all.AddRange(dlc);
            all.AddRange(mod);
            return all;
        }
    }
}
