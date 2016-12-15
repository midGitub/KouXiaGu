using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 保存游戏状态;
    /// </summary>
    public class ArchiveStage : StageObservable<Archiver>, IStageObserver<Archiver>
    {
        static readonly ArchiveStage instance = new ArchiveStage();

        public static ArchiveStage GetInstance
        {
            get { return instance; }
        }


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
            return (current & Stages.Game) > 0;
        }


        static Archiver archive;

        protected override Archiver Resource
        {
            get { return archive; }
        }

        ArchiveStage()
        {
            this.Subscribe(this);
        }

        IEnumerator IStageObserver<Archiver>.OnEnter(Archiver item)
        {
            yield break;
        }

        IEnumerator IStageObserver<Archiver>.OnLeave(Archiver item)
        {
            yield break;
        }

        IEnumerator IStageObserver<Archiver>.OnEnterRollBack(Archiver item)
        {
            yield break;
        }

        IEnumerator IStageObserver<Archiver>.OnLeaveRollBack(Archiver item)
        {
            yield break;
        }

        void IStageObserver<Archiver>.OnEnterCompleted()
        {
            archive.OnComplete();
        }

        void IStageObserver<Archiver>.OnLeaveCompleted()
        {
            return;
        }


        /// <summary>
        /// 保存游戏为新的存档;
        /// </summary>
        public static Archiver Save()
        {
            Archiver archive = new Archiver();
            Save(archive);
            return archive;
        }

        /// <summary>
        /// 保存游戏到存档;
        /// </summary>
        public static void Save(Archiver archive)
        {
            ArchiveStage.archive = archive;
            archive.OnSave();
            Initializer.Add(instance);
        }

    }

}
