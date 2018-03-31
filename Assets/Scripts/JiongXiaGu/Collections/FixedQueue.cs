using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Collections
{

    [Obsolete]
    public class FixedQueue<T>
    {
        private readonly T[] array;

        /// <summary>
        /// 首个元素下标;
        /// </summary>
        private int start;

        /// <summary>
        /// 尾部元素下标 +1 ;
        /// </summary>
        private int end;
        private int addedCount;
        private int version;

        /// <summary>
        /// 最大元素数目;
        /// </summary>
        public int MaxCount => array.Length;

        /// <summary>
        /// 当前元素数目;
        /// </summary>
        public int Count => start <= end ? end - start : MaxCount - end + start + 1;

        /// <summary>
        /// 添加的次数;
        /// </summary>
        public int AddedCount => addedCount;

        public FixedQueue(int maxCount)
        {
            array = new T[maxCount];
            start = 0;
            end = maxCount - 1;
            addedCount = 0;
            version = 0;
        }

        /// <summary>
        /// 添加元素,若达到最高计数数目,则移除最先加入的元素;
        /// </summary>
        public void Add(T item)
        {
            throw new NotImplementedException();
        }
    }
}
