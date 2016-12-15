using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 保存游戏状态;
    /// </summary>
    public class ArchiveStage : InstantObservable<Archiver>
    {
        ArchiveStage() { }

        static readonly ArchiveStage instance = new ArchiveStage();

        public static ArchiveStage GetInstance
        {
            get { return instance; }
        }


        const Stages DEPUTY = Stages.Saving;

        static Archiver archive;

        protected override Stages Deputy
        {
            get { return DEPUTY; }
        }

        protected override Archiver Resource
        {
            get { return archive; }
        }

        protected override void LastEnter()
        {
            archive.OnComplete();
        }

        protected override bool Premise(Stages current)
        {
            return (current & Stages.Game) > 0;
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
