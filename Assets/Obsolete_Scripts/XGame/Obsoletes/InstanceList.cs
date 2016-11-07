//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace XGame.Running.MapStructures
//{

//    /// <summary>
//    /// 实例链表;
//    /// </summary>
//    public class InstanceList<TBase> : IEnumerable<TBase>
//    {

//        public InstanceList()
//        {
//            m_List = new List<TBase>();
//        }

//        public InstanceList(IEnumerable<TBase> collection)
//        {
//            m_List = new List<TBase>(collection);
//        }

//        public InstanceList(int capacity)
//        {
//            m_List = new List<TBase>(capacity);
//        }

//        /// <summary>
//        /// 数据结构;
//        /// </summary>
//        protected List<TBase> m_List;

//        /// <summary>
//        /// 存在的元素个数 0 <= Count;
//        /// </summary>
//        public int Count
//        {
//            get { return m_List.Count; }
//        }

//        /// <summary>
//        /// 加入此元素;
//        /// </summary>
//        /// <param name="item"></param>
//        public virtual void Add(TBase item)
//        {
//            m_List.Add(item);
//        }

//        /// <summary>
//        /// 移除此元素;
//        /// </summary>
//        /// <param name="item"></param>
//        /// <returns></returns>
//        public virtual bool Remove(TBase item)
//        {
//            return m_List.Remove(item);
//        }

//        /// <summary>
//        /// 获取到结构中的该类型 元素;
//        /// </summary>
//        /// <typeparam name="T">继承与 TBase 的类型</typeparam>
//        /// <returns></returns>
//        public T Get<T>()
//            where T : TBase
//        {
//            return (T)m_List.Find(item => item is T);
//        }

//        /// <summary>
//        /// 获取到结构中所有该类型的 元素;
//        /// </summary>
//        /// <typeparam name="T">继承与 TBase 的类型</typeparam>
//        /// <returns></returns>
//        public IEnumerable<T> GetAll<T>()
//            where T : TBase
//        {
//            return m_List.FindAll(item => item is T).Cast<T>();
//        }

//        /// <summary>
//        /// 确认是否存在此 元素;
//        /// </summary>
//        /// <param name="item"></param>
//        /// <returns></returns>
//        public bool Contains(TBase item)
//        {
//            return m_List.Contains(item);
//        }

//        /// <summary>
//        /// 清除所有元素;
//        /// </summary>
//        public virtual void Clear()
//        {
//            m_List.Clear();
//        }

//        public IEnumerator<TBase> GetEnumerator()
//        {
//            return m_List.GetEnumerator();
//        }

//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return m_List.GetEnumerator();
//        }

//    }

//}
