using System;

namespace KouXiaGu
{

    public interface IBuildGameInCoroutine : ICoroutineInitialize<BuildGameData> { }
    public interface IBuildGameInThread : IThreadInitialize<BuildGameData> { }

    /// <summary>
    /// 加载游戏;
    /// </summary>
    [Serializable]
    internal class GameInitialize : InitializeComponent<IBuildGameInCoroutine, IBuildGameInThread, BuildGameData>
    {
        private GameInitialize() : base() { }
    }


    public interface IArchiveInCoroutine : ICoroutineInitialize<ArchivedGroup> { }
    public interface IArchiveInThread : IThreadInitialize<ArchivedGroup> { }

    /// <summary>
    /// 游戏归档方法;
    /// </summary>
    [Serializable]
    public sealed class ArchiveInitialize : InitializeComponent<IArchiveInCoroutine, IArchiveInThread, ArchivedGroup>
    {
        private ArchiveInitialize() : base() { }
    }


    public interface IQuitInCoroutine : ICoroutineInitialize<Unit> { }
    public interface IQuitInThread : IThreadInitialize<Unit> { }

    /// <summary>
    /// 退出游戏;
    /// </summary>
    [Serializable]
    internal class QuitInitialize : InitializeComponent<IQuitInCoroutine, IQuitInThread, Unit>
    {
        public QuitInitialize() : base() { }
    }

}
