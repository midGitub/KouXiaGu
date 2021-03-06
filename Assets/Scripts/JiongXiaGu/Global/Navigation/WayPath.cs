﻿using JiongXiaGu.Grids;
using System.Collections.Generic;

namespace JiongXiaGu.Navigation
{
    /// <summary>
    /// 路径,链表首节点为起点,末节点为终点;
    /// </summary>
    public class WayPath<T> : LinkedList<T>
    {
        public WayPath(IWalker<T> map, IRange<T> searchRange)
        {
            Map = map;
            SearchRange = searchRange;
        }

        public IWalker<T> Map { get; private set; }
        public IRange<T> SearchRange { get; private set; }

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
