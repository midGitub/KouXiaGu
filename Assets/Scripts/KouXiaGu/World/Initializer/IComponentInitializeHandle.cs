using System.Threading;
using System.Threading.Tasks;

namespace KouXiaGu.World
{
    public interface IComponentInitializeHandle
    {
        Task StartInitialize(CancellationToken token);
    }
}
