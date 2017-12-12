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
        /// 读取到资源,若无法读取则返回异常;
        /// </summary>
        public abstract T Load(LoadableContent content, AssetInfo assetInfo);

        /// <summary>
        /// 不支持读取方式异常;
        /// </summary>
        protected Exception NotSupportedLoadModeException(AssetLoadModes mode)
        {
            throw new NotSupportedException(string.Format("类型[{0}]不支持通过[{1}]方式读取;", typeof(T).Name, mode));
        }
    }
}
