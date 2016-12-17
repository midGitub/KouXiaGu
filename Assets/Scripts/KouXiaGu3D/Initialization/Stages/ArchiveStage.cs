using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 保存游戏,在开始游戏后对游戏进行存档;
    /// </summary>
    public class ArchiveStage : StageObservable<ArchiveDirectory>, IStageObserver<ArchiveDirectory>
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


        static ArchiveDirectory archive;

        static readonly HashSet<IStageObserver<ArchiveDirectory>> observerSet = new HashSet<IStageObserver<ArchiveDirectory>>();

        static readonly ArchiveStage instance = new ArchiveStage();

        protected override ArchiveDirectory Resource
        {
            get { return archive; }
        }

        protected override IEnumerable<IStageObserver<ArchiveDirectory>> Observers
        {
            get { return observerSet; }
        }

        ArchiveStage()
        {
            Subscribe(this);
        }

        public static bool Subscribe(IStageObserver<ArchiveDirectory> observer)
        {
            return observerSet.Add(observer);
        }

        public static bool Unsubscribe(IStageObserver<ArchiveDirectory> observer)
        {
            return observerSet.Remove(observer);
        }

        public static bool Contains(IStageObserver<ArchiveDirectory> observer)
        {
            return observerSet.Contains(observer);
        }

        /// <summary>
        /// 保存游戏为新的存档;
        /// </summary>
        public static ArchiveDirectory Start()
        {
            ArchiveDirectory archive = new ArchiveDirectory();
            Start(archive);
            return archive;
        }

        /// <summary>
        /// 保存游戏到存档;
        /// </summary>
        public static void Start(ArchiveDirectory archive)
        {
            ArchiveStage.archive = archive;
            archive.Create();
            Initializer.Add(instance);
        }


        IEnumerator IStageObserver<ArchiveDirectory>.OnEnter(ArchiveDirectory item)
        {
            yield break;
        }

        IEnumerator IStageObserver<ArchiveDirectory>.OnLeave(ArchiveDirectory item)
        {
            yield break;
        }

        IEnumerator IStageObserver<ArchiveDirectory>.OnEnterRollBack(ArchiveDirectory item)
        {
            archive.Destroy();
            yield break;
        }

        IEnumerator IStageObserver<ArchiveDirectory>.OnLeaveRollBack(ArchiveDirectory item)
        {
            yield break;
        }

        void IStageObserver<ArchiveDirectory>.OnEnterCompleted()
        {
            archive.OnComplete();
        }

        void IStageObserver<ArchiveDirectory>.OnLeaveCompleted()
        {
            return;
        }

    }

}
