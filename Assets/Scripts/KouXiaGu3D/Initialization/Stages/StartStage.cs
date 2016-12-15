using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 游戏起始界面;
    /// </summary>
    public class StartStage : StageObservable<object>
    {
        StartStage() { }

        static readonly StartStage instance = new StartStage();

        public static StartStage GetInstance
        {
            get { return instance; }
        }


        const Stages DEPUTY = Stages.Start;

        protected override Stages Deputy
        {
            get { return DEPUTY; }
        }

        protected override object Resource
        {
            get { return null; }
        }

        protected override void LastEnter()
        {
            return;
        }

        protected override bool Premise(Stages current)
        {
            return (current & DEPUTY) == 0;
        }

    }

}
