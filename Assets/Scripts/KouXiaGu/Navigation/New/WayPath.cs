using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Navigation
{


    public class WayPath<T> : LinkedList<T>
    {
        /// <summary>
        /// 起点;
        /// </summary>
        public T Starting
        {
            get {
                var first = First;
                if (first == null)
                {
                    return default(T);
                }
                else
                {
                    return first.Value;
                }
            }
        }

        /// <summary>
        /// 终点;
        /// </summary>
        public T Destination
        {
            get
            {
                var last = Last;
                if (last == null)
                {
                    return default(T);
                }
                else
                {
                    return last.Value;
                }
            }
        }
    }
}
