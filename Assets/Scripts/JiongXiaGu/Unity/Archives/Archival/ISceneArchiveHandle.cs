using System.Threading;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Archives
{

    /// <summary>
    /// 场景信息收集\读取处置接口;
    /// </summary>
    public interface ISceneArchiveHandle
    {
        /// <summary>
        /// 通知实例准备开始收集操作;
        /// </summary>
        /// <param name="cancellationToken">取消标记;</param>
        void Prepare(CancellationToken cancellationToken);

        /// <summary>
        /// 收集当前游戏的状态信息;
        /// </summary>
        /// <param name="archivalData">游戏状态信息;</param>
        /// <param name="cancellationToken">取消标记;</param>
        Task Collect(SceneArchivalData archivalData, CancellationToken cancellationToken);

        /// <summary>
        /// 在Unity线程调用,从存档读取状态信息;
        /// </summary>
        /// <param name="archive">需要读取的存档;</param>
        /// <param name="archivalData">需要保存到的游戏状态;</param>
        /// <param name="cancellationToken">取消标记;</param>
        Task Read(IArchiveFileInfo archive, SceneArchivalData archivalData, CancellationToken cancellationToken);
    }
}
