using System.Threading;
using System.Threading.Tasks;
using JiongXiaGu.Unity.Resources.Archives;

namespace JiongXiaGu.World.Archives
{

    /// <summary>
    /// 在进行存档时调用该接口;
    /// </summary>
    public interface ISceneArchiveHandle
    {
        /// <summary>
        /// 输出存档内容;
        /// </summary>
        Task WriteArchive(Archive archive, CancellationToken token);
    }
}
