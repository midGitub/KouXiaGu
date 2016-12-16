using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 保存游戏;
    /// </summary>
    public class ArchiveStage : StageObservable<ArchiveFile>, IStageObserver<ArchiveFile>
    {

        const Stages DEPUTY = Stages.Saving;
        const bool INSTANT = true;

        public override Stages Deputy
        {
            get { return DEPUTY; }
        }

        public override bool Instant
        {
            get { return INSTANT; }
        }

        public override bool Premise(Stages current)
        {
            return (current & Stages.Game) > 0 &&
                archive != null;
        }


        static ArchiveFile archive;

        static readonly HashSet<IStageObserver<ArchiveFile>> observerSet = new HashSet<IStageObserver<ArchiveFile>>();

        static readonly ArchiveStage instance = new ArchiveStage();

        protected override ArchiveFile Resource
        {
            get { return archive; }
        }

        protected override IEnumerable<IStageObserver<ArchiveFile>> Observers
        {
            get { return observerSet; }
        }

        ArchiveStage()
        {
            Subscribe(this);
        }

        public static bool Subscribe(IStageObserver<ArchiveFile> observer)
        {
            return observerSet.Add(observer);
        }

        public static bool Unsubscribe(IStageObserver<ArchiveFile> observer)
        {
            return observerSet.Remove(observer);
        }

        public static bool Contains(IStageObserver<ArchiveFile> observer)
        {
            return observerSet.Contains(observer);
        }

        /// <summary>
        /// 保存游戏为新的存档;
        /// </summary>
        public static ArchiveFile Start()
        {
            ArchiveFile archive = new ArchiveFile();
            Start(archive);
            return archive;
        }

        /// <summary>
        /// 保存游戏到存档;
        /// </summary>
        public static void Start(ArchiveFile archive)
        {
            ArchiveStage.archive = archive;
            archive.Create();
            Initializer.Add(instance);
        }


        IEnumerator IStageObserver<ArchiveFile>.OnEnter(ArchiveFile item)
        {
            yield break;
        }

        IEnumerator IStageObserver<ArchiveFile>.OnLeave(ArchiveFile item)
        {
            yield break;
        }

        IEnumerator IStageObserver<ArchiveFile>.OnEnterRollBack(ArchiveFile item)
        {
            archive.Destroy();
            yield break;
        }

        IEnumerator IStageObserver<ArchiveFile>.OnLeaveRollBack(ArchiveFile item)
        {
            yield break;
        }

        void IStageObserver<ArchiveFile>.OnEnterCompleted()
        {
            archive.OnComplete();
        }

        void IStageObserver<ArchiveFile>.OnLeaveCompleted()
        {
            return;
        }

    }

}
