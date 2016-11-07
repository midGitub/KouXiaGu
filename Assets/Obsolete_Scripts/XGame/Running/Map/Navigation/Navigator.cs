//using System;
//using System.Collections;
//using UnityEngine;

//namespace KouXiaGu.Running.Map.Guidances
//{

//    /// <summary>
//    /// 导航;
//    /// </summary>
//    [DisallowMultipleComponent]
//    public class Navigator : Controller<Navigator>, IGameLoad
//    {

//        /// <summary>
//        /// 每个占用增量;
//        /// </summary>
//        private const short OccupiedObstacleIncrease = 1;

//        /// <summary>
//        /// 障碍物存在的方向数目;
//        /// </summary>
//        private const int ObstacleDirectionCount = AspectHelper.SidesCount + 1;

//        /// <summary>
//        /// 对导航形成阻碍的障碍物类型;
//        /// </summary>
//        private const ObstacleMode NavigationObstacle = ObstacleMode.Static | ObstacleMode.Unimportant;

//        /// <summary>
//        /// 对新加入障碍形成阻碍的障碍物类型,但是不对寻路进行阻碍;
//        /// </summary>
//        private const ObstacleMode OccupiedObstacle = ObstacleMode.Occupied;

//        /// <summary>
//        /// 不重要的障碍物,在新加入物体时,允许请求移除的障碍物;
//        /// </summary>
//        private const ObstacleMode UnimportantObstacle = ObstacleMode.Unimportant;

//        /// <summary>
//        /// 调用次序;
//        /// </summary>
//        [SerializeField]
//        private CallOrder callOrder = CallOrder.Instance;

//        /// <summary>
//        /// 导航地图;
//        /// </summary>
//        private MapGame<Obstacle> m_ObstacleMap;

//        protected override Navigator This { get { return this; } }
//        CallOrder ICallOrder.CallOrder { get { return callOrder; } }

//        IEnumerator IGameLoad.OnStart()
//        {
//            MapInfo mapInfo = MapController.GetInstance.MapInfo;
//            m_ObstacleMap = new MapGame<Obstacle>();
//            yield return AsyncHelper.WaitForFixedUpdate;
//        }

//        IEnumerator IGameLoad.OnClear()
//        {
//            m_ObstacleMap.Clear();
//            m_ObstacleMap = null;
//            yield return AsyncHelper.WaitForFixedUpdate;
//        }

//        /// <summary>
//        /// 向这个方向加入障碍物;
//        /// </summary>
//        /// <param name="position">位置</param>
//        /// <param name="aspect">东南西北 + 本身;</param>
//        /// <param name="obstacle">障碍物</param>
//        public void Add(IntVector2 position, Aspect aspect, IObstacle obstacle)
//        {
//            Obstacle Obstacle;
//            if (m_ObstacleMap.TryGetValue(position, aspect, out Obstacle))
//            {
//                Obstacle.Add(obstacle);
//            }
//            else
//            {
//                Obstacle = new Obstacle(obstacle);
//                m_ObstacleMap.Add(position, aspect, Obstacle);
//            }
//        }

//        /// <summary>
//        /// 移除这个方向的障碍物;若障碍物不与传入的相同,则不移除并返回false;
//        /// </summary>
//        /// <param name="position">位置</param>
//        /// <param name="aspect">东南西北 + 本身;</param>
//        public bool Remove(IntVector2 position, Aspect aspect, IObstacle obstacle)
//        {
//            Obstacle Obstacle;
//            if (m_ObstacleMap.TryGetValue(position, aspect, out Obstacle))
//            {
//                return Obstacle.Remove(obstacle);
//            }
//#if UNITY_EDITOR
//            Debug.LogWarning("移除不存在的障碍物!");
//#endif
//            return false;
//        }


//        #region 获取到可行走的方向;

//        /// <summary>
//        /// 获取到可行走的方向;
//        /// </summary>
//        /// <param name="position">位置;</param>
//        /// <param name="key"></param>
//        /// <returns></returns>
//        public Aspect GetOpenAspect(IntVector2 position, IKey key)
//        {
//            //若超出地图范围,则不存在可行走的方向;
//            if (m_ObstacleMap.MapInfo.IsOuterBoundary(position))
//            {
//                return AspectHelper.Empty;
//            }

//            Aspect CloseAspects = AspectHelper.Empty;

//            foreach (var aspect in AspectHelper.GetAspect(AspectHelper.FourDirections))
//            {
//                if (m_ObstacleMap.IsOuterBoundary(position, aspect) || !IsOpenSide(position, aspect, key))
//                {
//                    CloseAspects |= AspectHelper.GetSide(aspect);
//                }
//            }

//            //查询角方向是否存在墙;
//            foreach (var aspect in AspectHelper.GetAspect(~CloseAspects & AspectHelper.Corners))
//            {
//                if (m_ObstacleMap.IsOuterBoundary(position, aspect) || !IsOpenCorner(position, aspect, key))
//                {
//                    CloseAspects |= aspect;
//                }
//            }

//            return ~CloseAspects & AspectHelper.AroundExceptSelf;
//        }

//        /// <summary>
//        /// 这个障碍物是否开放,开放返回true,否则返回false;
//        /// </summary>
//        /// <param name="obstacle"></param>
//        /// <param name="key"></param>
//        /// <returns></returns>
//        private bool IsOpen(Obstacle obstacle, IKey key)
//        {
//            if (obstacle.EffectiveObstacle != null)
//            {
//                return obstacle.EffectiveObstacle.IsOpen(key);
//            }
//            return true;
//        }

//        /// <summary>
//        /// 这个方向的障碍物是否开放,若不存在障碍物,则返回true;
//        /// </summary>
//        /// <param name="position"></param>
//        /// <param name="aspect"></param>
//        /// <param name="key"></param>
//        /// <returns></returns>
//        private bool IsOpen(IntVector2 position, Aspect aspect, IKey key)
//        {
//            Obstacle obstacle;
//            if (m_ObstacleMap.TryGetValue(position, aspect, out obstacle))
//            {
//                return IsOpen(obstacle, key);
//            }
//            return true;
//        }

//        /// <summary>
//        /// 这个正方向是否开放;
//        /// </summary>
//        /// <param name="position"></param>
//        /// <param name="FourDirection"></param>
//        /// <param name="key"></param>
//        /// <returns></returns>
//        private bool IsOpenSide(IntVector2 position, Aspect FourDirection, IKey key)
//        {
//            bool isOpen = true;

//            isOpen &= IsOpen(position, FourDirection, key);

//            position += AspectHelper.GetVector(FourDirection);
//            if (isOpen)
//            {
//                isOpen &= IsOpen(position, Aspect.Itself, key);
//            }

//            return isOpen;
//        }

//        /// <summary>
//        /// 检查这个角方向是否开放;
//        /// </summary>
//        /// <param name="position"></param>
//        /// <param name="corner"></param>
//        /// <param name="key"></param>
//        /// <returns></returns>
//        private bool IsOpenCorner(IntVector2 position, Aspect corner, IKey key)
//        {
//            position += AspectHelper.GetVector(corner);    //转换到对角的位置;

//            if (!IsOpen(position, Aspect.Itself, key))
//                return false;

//            switch (corner)
//            {
//                case Aspect.NorthWestern:
//                    return IsOpen(position, Aspect.East, key) &&
//                         IsOpen(position, Aspect.South, key);

//                case Aspect.NorthEast:
//                    return IsOpen(position, Aspect.Western, key) &&
//                         IsOpen(position, Aspect.South, key);

//                case Aspect.SouthWestern:

//                    return IsOpen(position, Aspect.East, key) &&
//                         IsOpen(position, Aspect.North, key);

//                case Aspect.SouthEast:
//                    return IsOpen(position, Aspect.Western, key) &&
//                         IsOpen(position, Aspect.North, key);

//                default:
//#if UNITY_EDITOR
//                    Debug.LogError("传入方向并非角方向!");
//#endif
//                    return false;
//            }
//        }

//        #endregion

//        /// <summary>
//        /// 障碍物节点;
//        /// </summary>
//        internal class Obstacle
//        {

//            internal Obstacle(IObstacle obstacle)
//            {
//                Add(obstacle);
//            }

//            /// <summary>
//            /// 有效的障碍物;
//            /// </summary>
//            private IObstacle m_EffectiveObstacle;

//            /// <summary>
//            /// 被覆盖的障碍物;
//            /// </summary>
//            private IObstacle m_UnimportantObstacle;

//            /// <summary>
//            /// 障碍物数量;
//            /// </summary>
//            private short m_OccupiedCount;

//            /// <summary>
//            /// 有效的障碍物;若不存在则返回null;
//            /// </summary>
//            public IObstacle EffectiveObstacle{ get { return m_EffectiveObstacle != null ? m_EffectiveObstacle : m_UnimportantObstacle; } }

//            /// <summary>
//            /// 障碍物数量;
//            /// </summary>
//            public short OccupiedCount
//            {
//                get { return m_OccupiedCount; }
//                private set
//                {
//                    m_OccupiedCount = value;
//#if UNITY_EDITOR
//                    if (m_OccupiedCount < 0)
//                    {
//                        Debug.LogError("加入移除的次数出错!Count < 0");
//                    }
//#endif
//                }
//            }

//            /// <summary>
//            /// 加入障碍物;
//            /// </summary>
//            /// <param name="node"></param>
//            /// <param name="obstacle"></param>
//            public void Add(IObstacle obstacle)
//            {

//                //加入障碍物仅为占位;
//                if ((obstacle.ObstacleMode & OccupiedObstacle) > 0)
//                {

//                }
//                //不存在有效障碍物,,且加入障碍物为寻路障碍物;
//                else if (m_EffectiveObstacle == null && (obstacle.ObstacleMode & NavigationObstacle) > 0)
//                {
//                    m_EffectiveObstacle = obstacle;
//                    if (m_UnimportantObstacle != null)
//                        m_UnimportantObstacle.Concealment();
//                }
//                //不存在可隐藏障碍物,且加入障碍物为不重要的;
//                else if (m_UnimportantObstacle == null && (obstacle.ObstacleMode & UnimportantObstacle) > 0)
//                {
//                    m_UnimportantObstacle = obstacle;
//                }
//                else
//                {
//                    throw new ArgumentException("已存在障碍物!");
//                }

//                OccupiedCount += OccupiedObstacleIncrease;
//            }

//            /// <summary>
//            /// 移除障碍物;
//            /// </summary>
//            /// <param name="node"></param>
//            /// <param name="obstacle"></param>
//            public bool Remove(IObstacle obstacle)
//            {
//                //加入障碍物仅为占位;
//                if ((obstacle.ObstacleMode & OccupiedObstacle) > 0)
//                {

//                }
//                //移除的为有效障碍物;
//                else if (m_EffectiveObstacle == obstacle)
//                {
//                    m_EffectiveObstacle = null;
//                    if (m_UnimportantObstacle != null)
//                        m_UnimportantObstacle.Show();
//                }
//                //移除的为可隐藏障碍物;
//                else if (m_UnimportantObstacle == obstacle)
//                {
//                    m_UnimportantObstacle = null;
//                }
//                else
//                {
//                    return false;
//                    //throw new ArgumentException("准备移除不存在的障碍物!" + obstacle);
//                }
//                OccupiedCount -= OccupiedObstacleIncrease;
//                return true;
//            }

//        }

//    }

//}
