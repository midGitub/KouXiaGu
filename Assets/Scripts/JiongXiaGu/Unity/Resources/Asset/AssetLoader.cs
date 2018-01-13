using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 资源读取抽象类;
    /// </summary>
    public abstract class AssetLoader<T>
        where T : class
    {
        /// <summary>
        /// 返回支持读取的方式;
        /// </summary>
        public abstract IReadOnlyList<AssetLoadModes> SupportedLoadModes { get; }

        /// <summary>
        /// 确认是否支持这种读取方式;
        /// </summary>
        public bool IsSupported(AssetLoadModes modes)
        {
            return SupportedLoadModes.Contains(modes);
        }

        /// <summary>
        /// 若不支持此方式读取则抛出异常;
        /// </summary>
        public void ThrowIfNotSupportedLoadMode(AssetLoadModes mode)
        {
            if (!IsSupported(mode))
            {
                throw NotSupportedLoadModeException(mode);
            }
        }

        /// <summary>
        /// 不支持读取方式异常;
        /// </summary>
        protected Exception NotSupportedLoadModeException(AssetLoadModes mode)
        {
            throw new NotSupportedException(string.Format("类型[{0}]不支持通过[{1}]方式读取;", typeof(T).Name, mode));
        }

        /// <summary>
        /// 读取到资源;
        /// </summary>
        public abstract T Load(ModificationContent content, AssetInfo assetInfo);

        /// <summary>
        /// 异步读取到资源;
        /// </summary>
        public abstract Task<T> LoadAsync(ModificationContent content, AssetInfo assetInfo, CancellationToken token);


        /// <summary>
        /// 从 AssetBundle 读取到资源;(仅Unity线程执行)
        /// </summary>
        protected TU LoadFromAssetBundle<TU>(ModificationContent content, AssetInfo assetInfo)
            where TU : UnityEngine.Object
        {
            AssetBundle assetBundle = LoadableResource.SharedContent.GetAssetBundle(content, assetInfo.From);
            var texture = assetBundle.LoadAsset<TU>(assetInfo.Name);
            if (texture == null)
            {
                throw new ArgumentException(string.Format("在AssetBundle[{0}]内未找到资源[{1}]", assetInfo.From, assetInfo.Name));
            }
            else
            {
                return texture;
            }
        }

        /// <summary>
        /// 从 AssetBundle 读取到资源;(仅Unity线程执行)
        /// </summary>
        protected TU LoadFromAssetBundle<TU>(AssetBundle assetBundle, string assetName)
            where TU : UnityEngine.Object
        {
            var item = assetBundle.LoadAsset<TU>(assetName);
            if (item == null)
            {
                throw new ArgumentException(string.Format("在AssetBundle[{0}]内未找到资源[{1}]", assetBundle, assetName));
            }
            else
            {
                return item;
            }
        }

        //protected Task<TU> LoadFromAssetBundleAsync<TU>(AssetBundle assetBundle, string assetName)
        //    where TU : UnityEngine.Object
        //{
        //    AssetBundleRequest request = assetBundle.LoadAssetAsync<TU>(assetName);
        //    TaskCompletionSource<TU> taskCompletionSource = new TaskCompletionSource<TU>();
        //}
    }
}
