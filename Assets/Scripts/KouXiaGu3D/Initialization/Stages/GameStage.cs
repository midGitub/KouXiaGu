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
        const Stages DEPUTY = Stages.Game;

        protected override Stages Deputy
        {
            get { return DEPUTY; }
        }

        protected override Archiver Resource
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        protected override void LastEnter()
        {
            throw new NotImplementedException();
        }

        protected override bool Premise(Stages current)
        {
            return true;
        }
    }

}
