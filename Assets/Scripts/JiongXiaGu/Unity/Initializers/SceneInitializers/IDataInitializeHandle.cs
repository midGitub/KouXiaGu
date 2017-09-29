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
        Task StartInitialize(Archive archive, CancellationToken token);
    }
}
