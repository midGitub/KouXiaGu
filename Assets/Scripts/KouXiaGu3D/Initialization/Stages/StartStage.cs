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

        static readonly StartStage instance = new StartStage();

        public static StartStage GetInstance
        {
            get { return instance; }
        }


        const Stages DEPUTY = Stages.Start;
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
            return (current & DEPUTY) == 0;
        }


        protected override object Resource
        {
            get { return null; }
        }

        StartStage()
        {

        }

    }

}
