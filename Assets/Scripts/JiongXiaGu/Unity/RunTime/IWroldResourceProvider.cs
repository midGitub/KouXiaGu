using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.RunTime
{

    /// <summary>
    /// 提供世界资源;
    /// </summary>
    public interface IWroldResourceProvider
    {
        /// <summary>
        /// 获取到世界资源;
        /// </summary>
        WroldResource GetWroldResource();
    }
}
