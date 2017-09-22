using System.Threading;
using System.Threading.Tasks;

namespace KouXiaGu.World
{

    /// <summary>
    /// 场景更新器初始化处置器;
    /// </summary>
    public interface IUpdaterInitializeHandle
    {
        Task StartInitialize(CancellationToken token);
    }
}
