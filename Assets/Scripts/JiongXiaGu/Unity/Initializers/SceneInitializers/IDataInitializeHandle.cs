using JiongXiaGu.Unity.Resources.Archives;
using System.Threading;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity
{

    /// <summary>
    /// 场景数据初始化处置器;
    /// </summary>
    public interface IDataInitializeHandle
    {
        /// <summary>
        /// 进行初始化,
        /// </summary>
        /// <param name="archive">若不存在存档,则为null</param>
        /// <param name="token">取消标记</param>
        Task StartInitialize(Archive archive, CancellationToken token);
    }
}
