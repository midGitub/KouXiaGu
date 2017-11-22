//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace JiongXiaGu
//{

//    public interface IAction<T>
//    {
//        void Invoke(T value);
//    }

//    public static class ForEachExtesions
//    {

//        public static void ForEach<TAction, T>(this IEnumerable<T> collection, TAction action)
//            where TAction : IAction<T>
//        {
//            if (collection == null)
//                throw new ArgumentNullException(nameof(collection));
//            if (action == null)
//                throw new ArgumentNullException(nameof(action));

//            foreach (var item in collection)
//            {
//                action.Invoke(item);
//            }
//        }

//        public static void ForEach<TAction, T>(this IEnumerable<T> collection, ICollection<TAction> actions)
//            where TAction: IAction<T>
//        {
//            if (collection == null)
//                throw new ArgumentNullException(nameof(collection));
//            if (actions == null)
//                throw new ArgumentNullException(nameof(actions));

//            foreach (var item in collection)
//            {
//                foreach (var action in actions)
//                {
//                    action.Invoke(item);
//                }
//            }
//        }
//    }

//    /// <summary>
//    /// 搜索项目,需要寻找到的对象;
//    /// </summary>
//    public struct SearchAction<T> : IAction<T>
//    {
//        /// <summary>
//        /// 搜索元素对比器;
//        /// </summary>
//        private Func<T, bool> comparator;

//        /// <summary>
//        /// 找到时进行的操作;
//        /// </summary>
//        private Action<T> onComplete;

//        /// <summary>
//        /// 是否已经寻找到?
//        /// </summary>
//        public bool IsFind { get; private set; }
         
//        public SearchAction(Func<T, bool> comparator, Action<T> onComplete) : this()
//        {
//            if (comparator == null)
//                throw new ArgumentNullException(nameof(comparator));
//            if (onComplete == null)
//                throw new ArgumentNullException(nameof(onComplete));

//            this.comparator = comparator;
//            this.onComplete = onComplete;
//        }

//        public void Reset()
//        {
//            IsFind = false;
//        }

//        public void Invoke(T value)
//        {
//            if (!IsFind && comparator.Invoke(value))
//            {
//                onComplete.Invoke(value);
//            }
//        }
//    }
//}
