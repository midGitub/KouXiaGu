//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace XGame.Running.MapStructures
//{

//    /// <summary>
//    /// 在游戏地图的基础上每个点添加一个HashSet合集;
//    /// </summary>
//    /// <typeparam name="T"></typeparam>
//    public class SetMap2D<T> : WallSet<HashSet<T>>
//    {
//        public SetMap2D() { }
//        public SetMap2D(MapInfo mapInfo) : base(mapInfo) { }


//        /// <summary>
//        /// 加入到位置,若已存在相同的则返回false,加入成功返回true;
//        /// </summary>
//        /// <param name="position"></param>
//        /// <param name="fiveDirections"></param>
//        /// <param name="value"></param>
//        public bool Add(IntVector2 position, Aspect2D fiveDirections, T value)
//        {
//            HashSet<T> hashSet;
//            OuterBoundary(position);
//            if (TryGetValue(position, fiveDirections, out hashSet))
//            {
//                return hashSet.Add(value);
//            }
//            else
//            {
//                hashSet = new HashSet<T>() { value };
//                Add(position, fiveDirections, hashSet);
//                return true;
//            }
//        }

//        /// <summary>
//        /// 移除;若移除成功返回true,否则返回false;
//        /// </summary>
//        /// <param name="position"></param>
//        /// <param name="fiveDirections"></param>
//        /// <param name="value"></param>
//        /// <returns></returns>
//        public bool Remove(IntVector2 position, Aspect2D fiveDirections, T value)
//        {
//            HashSet<T> hashSet;
//            if (TryGetValue(position, fiveDirections, out hashSet))
//            {
//                return hashSet.Remove(value);
//            }
//            return false;
//        }

//        /// <summary>
//        /// 确认是否存在此元素;存在返回true,否则返回false;
//        /// </summary>
//        /// <param name="position"></param>
//        /// <param name="fiveDirections"></param>
//        /// <param name="value"></param>
//        /// <returns></returns>
//        public bool Contains(IntVector2 position, Aspect2D fiveDirections, T value)
//        {
//            HashSet<T> hashSet;
//            if (TryGetValue(position, fiveDirections, out hashSet))
//            {
//                return hashSet.Contains(value);
//            }
//            return false;
//        }



//    }

//}
