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
        const Stages DEPUTY = Stages.Saving;

        static readonly ArchiveStage instance = new ArchiveStage();
        static Archiver archive;

        public static ArchiveStage GetInstance
        {
            get { return instance; }
        }

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
            return current == Stages.Game;
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


        //ArchiveStage() { }

        //const Stages DEPUTY = Stages.Saving;
        //const bool INSTANT = true;

        //static readonly ArchiveStage instance = new ArchiveStage();
        //static readonly HashSet<IPreservable> observerSet = new HashSet<IPreservable>();
        //static readonly Queue<IEnumerator> coroutines = new Queue<IEnumerator>();

        //static Archiver archive;


        //Stages IPeriod.Deputy
        //{
        //    get { return DEPUTY; }
        //}

        //bool IPeriod.Instant
        //{
        //    get { return INSTANT; }
        //}

        //bool IPeriod.Premise()
        //{
        //    return Initializer.Stages == Stages.Game;
        //}

        //IEnumerator IPeriod.OnEnter()
        //{
        //    IEnumerator coroutine;
        //    Queue<IEnumerator> coroutineQueue = InitCoroutineQueue();
        //    archive.OnSave();

        //    while (coroutineQueue.Count != 0)
        //    {
        //        coroutine = coroutineQueue.Dequeue();

        //        while (coroutine.MoveNext())
        //        {
        //            yield return null;
        //        }
        //    }

        //    archive.OnComplete();
        //    yield break;
        //}

        //Queue<IEnumerator> InitCoroutineQueue()
        //{
        //    coroutines.Clear();

        //    IEnumerator item;
        //    foreach (var source in observerSet)
        //    {
        //        item = source.OnSave(archive);
        //        coroutines.Enqueue(item);
        //    }

        //    return coroutines;
        //}

        ///// <summary>
        ///// 不需要实现
        ///// </summary>
        //IEnumerator IPeriod.OnLeave()
        //{
        //    throw new NotImplementedException();
        //}


        ///// <summary>
        ///// 订阅到这个状态;
        ///// </summary>
        //public static bool Subscribe(IPreservable observer)
        //{
        //    return observerSet.Add(observer);
        //}

        //public static bool Unsubscribe(IPreservable observer)
        //{
        //    return observerSet.Remove(observer);
        //}


    }

}
