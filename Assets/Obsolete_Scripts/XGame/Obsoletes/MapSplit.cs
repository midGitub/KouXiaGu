//using System.Collections;
//using System.Collections.Generic;

//namespace XGame.Running.MapStructures
//{

//    /// <summary>
//    /// 抽象类 拆分的地图结构;
//    /// </summary>
//    public abstract class MapSplit<T> : MapCollection<T>, IMapDictionary<T>, IEnumerable<T>
//    {
//        public MapSplit(MapInfo mapInfo) : base(mapInfo) { }

//        /// <summary>
//        /// 允许保存到的方向;
//        /// </summary>
//        public abstract Aspect CanSaveAspect2D { get; }

//        /// <summary>
//        /// 输入值是否在允许范围内,若不是则返回异常 ArgumentOutOfRangeException;
//        /// </summary>
//        /// <param name="position"></param>
//        /// <param name="aspect"></param>
//        protected override void OuterBoundary(IntVector2 position, Aspect aspect)
//        {
//            OuterBoundary(position);

//            AspectHelper.IsSingleAspect(aspect);
//            AspectHelper.ConfirmAspect(aspect, CanSaveAspect2D);
//        }

//        /// <summary>
//        /// 输入值是否在允许范围内,若不是则返回异常 ArgumentOutOfRangeException;
//        /// 若在允许范围内,则转换为保存的 点 和 方向;
//        /// </summary>
//        /// <param name="position"></param>
//        /// <param name="corners"></param>
//        protected abstract void IsInRange(ref IntVector2 position, ref Aspect aspect);

//        /// <summary>
//        /// 获取到这个值,若不存在则返回异常 KeyNotFoundException;
//        /// </summary>
//        /// <param name="position"></param>
//        /// <param name="aspect"></param>
//        /// <returns></returns>
//        public T GetValue(IntVector2 position, Aspect aspect)
//        {
//            IsInRange(ref position, ref aspect);
//            var dictionary = MapDictionary[position];
//            return dictionary[aspect];
//        }

//        /// <summary>
//        /// 设置到这个值,若不存在则返回异常 KeyNotFoundException;
//        /// </summary>
//        /// <param name="position"></param>
//        /// <param name="aspect"></param>
//        /// <param name="value"></param>
//        public void SetValue(IntVector2 position, Aspect aspect, T value)
//        {
//            IsInRange(ref position, ref aspect);
//            var dictionary = MapDictionary[position];
//            dictionary[aspect] = value;
//        }

//        /// <summary>
//        /// 加入到结构;若已存在,则返回异常ArgumentException;
//        /// </summary>
//        /// <param name="position"></param>
//        /// <param name="value"></param>
//        /// <param name="aspect"></param>
//        public abstract void Add(IntVector2 position, Aspect aspect, T value);
//        //{
//        //    Dictionary<Aspect, T> node;

//        //    IsInRange(ref position, ref aspect);

//        //    if (MapDictionary.TryGetValue(position, out node))
//        //    {
//        //        node.Add(aspect, value);
//        //    }
//        //    else
//        //    {
//        //        node = new Dictionary<Aspect, T>(AspectCount);
//        //        node.Add(aspect, value);
//        //        MapDictionary.Add(position, node);
//        //    }
//        //    Count++;
//        //}

//        /// <summary>
//        /// 从结构移除元素;成功移除返回true,若未找到则返回false;
//        /// </summary>
//        /// <param name="position"></param>
//        /// <param name="aspect"></param>
//        public bool Remove(IntVector2 position, Aspect aspect)
//        {
//            bool isRemove = false;
//            Dictionary<Aspect, T> node;

//            IsInRange(ref position, ref aspect);

//            if (MapDictionary.TryGetValue(position, out node))
//            {
//                isRemove = node.Remove(aspect);
//                if (isRemove && node.Count == 0)
//                {
//                    MapDictionary.Remove(position);
//                }
//            }

//            Count = isRemove ? --Count : Count;
//            return isRemove;
//        }

//        /// <summary>
//        /// 确认到这个位置是否存在;
//        /// </summary>
//        /// <param name="position"></param>
//        /// <param name="aspect"></param>
//        /// <returns></returns>
//        public bool Contains(IntVector2 position, Aspect aspect)
//        {
//            Dictionary<Aspect, T> node;

//            IsInRange(ref position, ref aspect);

//            if (MapDictionary.TryGetValue(position, out node))
//            {
//                return node.ContainsKey(aspect);
//            }
//            return false;
//        }

//        /// <summary>
//        /// 尝试获取到这个元素;
//        /// </summary>
//        /// <param name="position"></param>
//        /// <param name="aspect"></param>
//        /// <param name="value"></param>
//        /// <returns></returns>
//        public bool TryGetValue(IntVector2 position, Aspect aspect, out T value)
//        {
//            Dictionary<Aspect, T> node;

//            IsInRange(ref position, ref aspect);

//            if (MapDictionary.TryGetValue(position, out node))
//            {
//                if (node.TryGetValue(aspect, out value))
//                {
//                    return true;
//                }
//            }
//            value = default(T);
//            return false;
//        }

//        /// <summary>
//        /// 清除所有元素;
//        /// </summary>
//        public virtual void Clear()
//        {
//            MapDictionary.Clear();
//            Count = 0;
//        }

//        /// <summary>
//        /// 迭代获取所有的元素T;
//        /// </summary>
//        /// <returns></returns>
//        public IEnumerator<T> GetEnumerator()
//        {
//            foreach (var dictionary in MapDictionary.Values)
//            {
//                foreach (var value in dictionary.Values)
//                {
//                    yield return value;
//                }
//            }
//        }

//        /// <summary>
//        /// 迭代获取所有的元素T;
//        /// </summary>
//        /// <returns></returns>
//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return this.GetEnumerator();
//        }


//    }

//}
