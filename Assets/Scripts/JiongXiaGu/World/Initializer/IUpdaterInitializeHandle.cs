using System.Threading;
using System.Threading.Tasks;

namespace JiongXiaGu.World
{

    /// <summary>
    /// 场景更新器初始化处置器;
    /// </summary>
    public interface IUpdaterInitializeHandle
    {
        Task StartInitialize(CancellationToken token);
    }
}
