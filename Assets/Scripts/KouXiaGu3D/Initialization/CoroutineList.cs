using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 记录执行到位置的的链表;
    /// </summary>
    public class CoroutineList<T>
    {

        readonly List<KeyValuePair<T, IEnumerator>> coroutineList;

        /// <summary>
        /// 指向运行到的协程;
        /// </summary>
        public int Pointer { get; private set; }

        /// <summary>
        /// 总共的任务数目;
        /// </summary>
        public int Count
        {
            get { return coroutineList.Count; }
        }

        /// <summary>
        /// 已经完成的任务数目;
        /// </summary>
        public int CompleteCount
        {
            get
            {
                int completeCount = Pointer;
                return completeCount > Count ? Count : completeCount;
            }
        }

        /// <summary>
        /// 等待中的任务数目;
        /// </summary>
        public int WaitCount
        {
            get
            {
                int waitCount = coroutineList.Count - Pointer;
                return waitCount < 0 ? 0 : waitCount;
            }
        }

        /// <summary>
        /// 当前指向的协程,若不存在则为NULL;
        /// </summary>
        public IEnumerator Coroutine
        {
            get { return coroutineList[Pointer].Value; }
        }

        /// <summary>
        /// 当前指向的元素,若不存在则为NULL;
        /// </summary>
        public T Item
        {
            get { return coroutineList[Pointer].Key; }
        }

        public CoroutineList()
        {
            coroutineList = new List<KeyValuePair<T, IEnumerator>>();
            ResetPointer();
        }

        public void ResetPointer()
        {
            Pointer = 0;
        }

        public bool MoveNext()
        {
            Pointer++;
            return Pointer > Count;
        }

        public bool MovePreviou()
        {
            Pointer--;
            return Pointer < 0;
        }

        /// <summary>
        /// 重新设置协程链表;
        /// </summary>
        public void SetCoroutines(IEnumerable<T> observerSet, Func<T, IEnumerator> func)
        {
            IEnumerator coroutine;
            coroutineList.Clear();

            foreach (var observer in observerSet)
            {
                coroutine = func(observer);
                coroutineList.Add(new KeyValuePair<T, IEnumerator>(observer, coroutine));
            }

            ResetPointer();
        }

        /// <summary>
        /// 重新设置协程链表;
        /// </summary>
        public void SetCoroutines(IEnumerable<KeyValuePair<T, IEnumerator>> observerSet, Func<T, IEnumerator> func)
        {
            IEnumerable<T> item = observerSet.Select(observer => observer.Key);
            SetCoroutines(item, func);
        }

        /// <summary>
        /// 获取到所有正在等待中的任务和当前正在执行的任务;
        /// </summary>
        public KeyValuePair<T, IEnumerator>[] GetWaitsAndCurrent()
        {
            int waitCount = WaitCount;
            KeyValuePair<T, IEnumerator>[] waits = new KeyValuePair<T, IEnumerator>[waitCount];
            coroutineList.CopyTo(Pointer, waits, 0, waitCount);
            return waits;
        }

        /// <summary>
        /// 获取到所有已经完成的任务;
        /// </summary>
        public KeyValuePair<T, IEnumerator>[] GetCompletes()
        {
            int completeCount = CompleteCount;
            KeyValuePair<T, IEnumerator>[] completes = new KeyValuePair<T, IEnumerator>[completeCount];
            coroutineList.CopyTo(0, completes, 0, completeCount);
            return completes;
        }

        /// <summary>
        /// 获取到所有已经完成的任务和当前正在执行的任务;
        /// </summary>
        public KeyValuePair<T, IEnumerator>[] GetCompletesAndCurrent()
        {
            int completeCount = CompleteCount + 1;
            KeyValuePair<T, IEnumerator>[] completes = new KeyValuePair<T, IEnumerator>[completeCount];
            coroutineList.CopyTo(0, completes, 0, completeCount);
            return completes;
        }

        public void Clear()
        {
            coroutineList.Clear();
        }

    }


}
