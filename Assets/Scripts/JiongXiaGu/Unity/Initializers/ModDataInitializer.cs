using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Initializers
{

    /// <summary>
    /// 模组数据初始化处置;
    /// </summary>
    public interface IModDataInitializeHandle
    {
        /// <summary>
        /// 进行初始化;
        /// </summary>
        Task Initialize(IEnumerable<ModInfo> datanfos, CancellationToken token);
    }

    /// <summary>
    /// 模组数据初始化;
    /// </summary>
    public class ModDataInitializer : InitializerBase
    {
        protected override string InitializerName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        protected override Task Initialize_internal(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
