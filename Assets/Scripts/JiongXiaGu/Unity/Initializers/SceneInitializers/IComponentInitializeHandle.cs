using System.Threading;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity
{
    public interface IComponentInitializeHandle
    {
        Task StartInitialize(CancellationToken token);
    }
}
