using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 游戏起始阶段;
    /// </summary>
    public class InitialStage : StageObservable<object>
    {

        const Stages DEPUTY = Stages.Initial;
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


        static readonly HashSet<IStageObserver<object>> observerSet = new HashSet<IStageObserver<object>>();

        static readonly InitialStage instance = new InitialStage();

        protected override object Resource
        {
            get { return null; }
        }

        protected override IEnumerable<IStageObserver<object>> Observers
        {
            get { return observerSet; }
        }

        InitialStage()
        {

        }

        public static bool Subscribe(IStageObserver<object> observer)
        {
            return observerSet.Add(observer);
        }

        public static bool Unsubscribe(IStageObserver<object> observer)
        {
            return observerSet.Remove(observer);
        }

        public static bool Contains(IStageObserver<object> observer)
        {
            return observerSet.Contains(observer);
        }

        public static void Start()
        {
            Initializer.Add(instance);
        }

    }

}
