using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 资源读取抽象类;
    /// </summary>
    public abstract class AssetReader<T>
        where T : class
    {
        /// <summary>
        /// 资源类型;
        /// </summary>
        public abstract AssetTypes AssetType { get; }

        /// <summary>
        /// 读取到资源,若无法读取则返回异常;
        /// </summary>
        public abstract T Load(LoadableContent content, AssetInfo assetInfo);

        /// <summary>
        /// 读取到资源,若无法读取则返回异常;
        /// </summary>
        public abstract WeakReferenceObject<T> AsWeakReferenceObject(T value);

        /// <summary>
        /// 不支持读取方式异常;
        /// </summary>
        protected Exception InvalidLoadModeException(string type, LoadMode mode)
        {
            throw new InvalidOperationException(string.Format("[{0}]不支持通过[{1}]读取;", type, mode));
        }
    }
}
