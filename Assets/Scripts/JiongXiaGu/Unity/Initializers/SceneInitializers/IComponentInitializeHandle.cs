using System.Threading;
using System.Threading.Tasks;

namespace JiongXiaGu.World
{
    public interface IComponentInitializeHandle
    {
        Task StartInitialize(CancellationToken token);
    }
}
