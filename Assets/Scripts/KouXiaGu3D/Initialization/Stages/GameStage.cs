using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Initialization
{


    /// <summary>
    /// 游戏初始化,通过存档初始化游戏;
    /// </summary>
    public class GameStage : StageObservable<Archiver>
    {
        static readonly GameStage instance = new GameStage();

        public static GameStage GetInstance
        {
            get { return instance; }
        }


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
            return (current & DEPUTY) == 0;
        }


        static Archiver archive;

        protected override Archiver Resource
        {
            get { return archive; }
        }

        GameStage()
        {

        }


    }

}
