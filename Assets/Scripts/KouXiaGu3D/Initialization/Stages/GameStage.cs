using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

namespace KouXiaGu.Initialization
{


    /// <summary>
    /// 游戏初始化,通过存档初始化游戏,等于开始一个新的游戏;
    /// </summary>
    public class GameStage : StageObservable<ArchiveDirectory>
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


        static ArchiveDirectory archive;

        static readonly HashSet<IStageObserver<ArchiveDirectory>> observerSet = new HashSet<IStageObserver<ArchiveDirectory>>();

        static readonly GameStage instance = new GameStage();

        protected override ArchiveDirectory Resource
        {
            get { return archive; }
        }

        protected override IEnumerable<IStageObserver<ArchiveDirectory>> Observers
        {
            get { return observerSet; }
        }

        GameStage()
        {

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

        public static void Start(ArchiveDirectory archive)
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
