using System.Threading;
using System.Threading.Tasks;
using JiongXiaGu.Unity.Resources.Archives;

namespace JiongXiaGu.Unity.Archives
{

    /// <summary>
    /// 在进行存档时调用该接口;
    /// </summary>
    public interface ISceneArchiveHandle
    {
        /// <summary>
        /// 表示开始进行存档操作;
        /// </summary>
        void Prepare(CancellationToken cancellationToken);

        /// <summary>
        /// 准备存档内容;
        /// </summary>
        /// <param name="archive">存档路径;</param>
        /// <param name="cancellationToken">取消存档标记;</param>
        void Begin();

        /// <summary>
        /// 输出存档内容;
        /// </summary>
        /// <param name="archive">存档路径;</param>
        /// <param name="cancellationToken">取消存档标记;</param>
        Task Write(Archive archive);
    }
}
