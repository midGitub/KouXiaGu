using System;
using JiongXiaGu.Collections;
using System.Threading;
using System.Collections.Generic;

namespace JiongXiaGu.Unity.Resources
{
    /// <summary>
    /// 数据整合处理接口;
    /// </summary>
    public interface IResourceIntegrateHandle
    {
        void Initialize(IReadOnlyList<ModificationContent> mods, CancellationToken token);

        ///// <summary>
        ///// 读取到对应内容;
        ///// </summary>
        //void Read(ModificationContent content, ITypeDictionary data, CancellationToken token);

        ///// <summary>
        ///// 输出对应内容;
        ///// </summary>
        ///// <exception cref="InvalidOperationException">未找到指定资源</exception>
        //void Write(ModificationContent content, ITypeDictionary data, CancellationToken token);

        ///// <summary>
        ///// 设置新的数据数据;
        ///// </summary>
        //void SetNew(LoadableData[] collection, CancellationToken token);

        ///// <summary>
        ///// 清空所有数据;
        ///// </summary>
        //void Clear();
    }
}
