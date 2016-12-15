using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Initialization
{

    public class ArchiveStages : IPeriod
    {
        ArchiveStages() { }

        const GameStages DEPUTY = GameStages.Saving;
        const bool INSTANT = true;

        static readonly ArchiveStages instance = new ArchiveStages();
        static readonly HashSet<IPreservable> observerSet = new HashSet<IPreservable>();
        static readonly Queue<IEnumerator> coroutines = new Queue<IEnumerator>();

        static Archiver archive;


        GameStages IPeriod.Deputy
        {
            get { return DEPUTY; }
        }

        bool IPeriod.Instant
        {
            get { return INSTANT; }
        }

        bool IPeriod.Premise()
        {
            return true;
            return Initializer.Stages == GameStages.Game;
        }

        IEnumerator IPeriod.OnEnter()
        {
            IEnumerator coroutine;
            Queue<IEnumerator> coroutineQueue = InitCoroutineQueue();

            while (coroutineQueue.Count != 0)
            {
                coroutine = coroutineQueue.Dequeue();

                while (coroutine.MoveNext())
                {
                    yield return null;
                }
            }

            archive.OnComplete();
            yield break;
        }

        Queue<IEnumerator> InitCoroutineQueue()
        {
            coroutines.Clear();

            IEnumerator item;
            foreach (var source in observerSet)
            {
                item = source.OnSave(archive);
                coroutines.Enqueue(item);
            }

            return coroutines;
        }

        /// <summary>
        /// 不需要实现
        /// </summary>
        IEnumerator IPeriod.OnLeave()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 订阅到这个状态;
        /// </summary>
        public static bool Subscribe(IPreservable observer)
        {
            return observerSet.Add(observer);
        }

        public static bool Unsubscribe(IPreservable observer)
        {
            return observerSet.Remove(observer);
        }


        /// <summary>
        /// 保存游戏为新的存档;
        /// </summary>
        public static Archiver Save()
        {
            ArchiveStages.archive = new Archiver();
            Initializer.Add(instance);
            return archive;
        }

        /// <summary>
        /// 保存游戏到存档;
        /// </summary>
        public static void Save(Archiver archive)
        {
            ArchiveStages.archive = archive;
            Initializer.Add(instance);
        }


    }

}
