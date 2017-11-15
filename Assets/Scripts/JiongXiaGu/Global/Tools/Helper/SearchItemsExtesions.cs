//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace JiongXiaGu
//{

//    /// <summary>
//    /// 在合集内寻找多个对象;
//    /// </summary>
//    public static class SearchItemsExtesions
//    {

//        /// <summary>
//        /// 在合集内寻找多个对象;
//        /// </summary>
//        /// <param name="collection">目标合集</param>
//        /// <param name="searchItems">需要寻找的对象,该合集会在寻找过程中更改,寻找结束后未找到的对象为该合集对象</param>
//        public static void Search<T>(this IEnumerable<T> collection, LinkedList<SearchItem<T>> searchItems)
//        {
//            if (collection == null)
//                throw new ArgumentNullException(nameof(collection));
//            if (searchItems == null)
//                throw new ArgumentNullException(nameof(searchItems));

//            foreach (var item in collection)
//            {
//                LinkedListNode<SearchItem<T>> node = searchItems.First;
//                while (node != null)
//                {
//                    SearchItem<T> searchItem = node.Value;
//                    if (node.Value.Compare(item))
//                    {
//                        searchItem.OnComplete(item);

//                        //一个对象对应多个结果;
//                        //var temp = node;
//                        //node = node.Next;
//                        //searchItems.Remove(temp);

//                        //一个对象对应一个结果;
//                        searchItems.Remove(node);
//                        break;
//                    }
//                    else
//                    {
//                        node = node.Next;
//                    }
//                }
//            }
//        }
//    }

//    /// <summary>
//    /// 需要寻找到的对象,和寻找到之后的操作;
//    /// </summary>
//    public class SearchItem<T>
//    {
//        public T Item { get; private set; }
//        private Func<T, bool> comparator;
//        private Action<T> onComplete;

//        public SearchItem(T item, Action<T> onComplete)
//        {
//            if (onComplete == null)
//                throw new ArgumentNullException(nameof(onComplete));

//            Item = item;
//            comparator = value => EqualityComparer<T>.Default.Equals(value, item);
//            this.onComplete = onComplete;
//        }

//        public SearchItem(T item, Func<T, bool> comparator, Action<T> onComplete)
//        {
//            if (comparator == null)
//                throw new ArgumentNullException(nameof(comparator));
//            if (onComplete == null)
//                throw new ArgumentNullException(nameof(onComplete));

//            Item = item;
//            this.comparator = comparator;
//            this.onComplete = onComplete;
//        }

//        public bool Compare(T item)
//        {
//            return comparator.Invoke(item);
//        }

//        public void OnComplete(T item)
//        {
//            onComplete.Invoke(item);
//        }
//    }
//}
