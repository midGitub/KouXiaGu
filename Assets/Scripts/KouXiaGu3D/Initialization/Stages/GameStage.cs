using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

namespace KouXiaGu.Initialization
{


    /// <summary>
    /// 游戏初始化,通过存档初始化游戏;
    /// </summary>
    public class GameStage : StageObservable<ArchiveFile>
    {

        const Stages DEPUTY = Stages.Game;
        const bool INSTANT = false;

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
            return (current & DEPUTY) == 0 &&
                archive != null &&
                (current & Stages.Initial) > 0;
        }


        static ArchiveFile archive;

        static readonly HashSet<IStageObserver<ArchiveFile>> observerSet = new HashSet<IStageObserver<ArchiveFile>>();

        static readonly GameStage instance = new GameStage();

        protected override ArchiveFile Resource
        {
            get { return archive; }
        }

        protected override IEnumerable<IStageObserver<ArchiveFile>> Observers
        {
            get { return observerSet; }
        }

        GameStage()
        {

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

        public static void Start(ArchiveFile archive)
        {
            GameStage.archive = archive;
            Initializer.Add(instance);
        }

        public static void End()
        {
            Initializer.Remove(instance);
        }

    }

}
