
//#define DEBUG_Navigator

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;
//using XGame.Collections;

//namespace XGame.GameRunning
//{


//    /// <summary>
//    /// 障碍物状态;
//    /// </summary>
//    [Flags]
//    public enum ObstacleMode
//    {

//        /// <summary>
//        /// 静态的障碍物;仅允许自己移除;
//        /// </summary>
//        Static = 1,

//        ///// <summary>
//        ///// 动态障碍物;
//        ///// </summary>
//        //Dynamic,

//        /// <summary>
//        /// 不重要的障碍物,允许覆盖\隐藏;
//        /// </summary>
//        Unimportant = 2,

//        /// <summary>
//        /// 占用这个障碍物点,但是不对寻路形成障碍;
//        /// </summary>
//        Occupied = 4,

//    }

//    public interface IKey
//    {

//    }

//    public interface IObstacle
//    {

//        /// <summary>
//        /// 障碍物属性;
//        /// </summary>
//        ObstacleMode ObstacleMode { get; }

//        /// <summary>
//        /// 查询是否允许通过这个障碍物;
//        /// </summary>
//        /// <param name="key"></param>
//        /// <returns>允许通过为true,否则为false;</returns>
//        bool IsOpen(IKey key);

//        /// <summary>
//        /// 请求隐藏该障碍物;
//        /// </summary>
//        void Concealment();

//        /// <summary>
//        /// 请求恢复隐藏的障碍物;
//        /// </summary>
//        void Show();

//    }


//    /// <summary>
//    /// 导航数据类;
//    /// </summary>
//    public class Navigator : Manager<Navigator>, IGameO
//    {

//        [SerializeField]
//        private CallOrder moduleType = CallOrder.Instance;

//        /// <summary>
//        /// 地图信息;
//        /// </summary>
//        private MapInfo m_MapInfo;

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
//        /// 有效障碍物地图;五个方向,本身方向 + 周围四个;
//        /// </summary>
//        private GameMap2D<Obstacle> m_ObstacleMap;

//        protected override Navigator This
//        {
//            get { return this; }
//        }

//        CallOrder ICallOrder.CallOrder
//        {
//            get { return moduleType; }
//        }

//        void IGameO.GameStart()
//        {
//            return;
//        }

//        IEnumerator IGameO.OnStart()
//        {
//            m_MapInfo = MapManager.GetInstance.MapInfo;
//            m_ObstacleMap = new GameMap2D<Obstacle>(m_MapInfo);
//            yield return AsyncHelper.WaitForFixedUpdate;
//        }

//        #region 障碍物;

//        /// <summary>
//        /// 向这个方向加入障碍物;
//        /// </summary>
//        /// <param name="position">位置</param>
//        /// <param name="fiveDirection">东南西北 + 本身;</param>
//        /// <param name="obstacle">障碍物</param>
//        public void Add(IntVector2 position, Aspect2D fiveDirection, IObstacle obstacle)
//        {
//            Obstacle Obstacle;
//            if (m_ObstacleMap.TryGetValue(position, fiveDirection, out Obstacle))
//            {
//                Obstacle.Add(Obstacle, obstacle);
//            }
//            else
//            {
//                Obstacle = new Obstacle(obstacle);
//                m_ObstacleMap.Add(position, fiveDirection, Obstacle);
//            }
//        }

//        /// <summary>
//        /// 移除这个方向的障碍物;若障碍物不与传入的相同,则不移除并返回false;
//        /// </summary>
//        /// <param name="position">位置</param>
//        /// <param name="fiveDirection">东南西北 + 本身;</param>
//        public void Remove(IntVector2 position, Aspect2D fiveDirection, IObstacle obstacle)
//        {
//            Obstacle Obstacle;
//            if (m_ObstacleMap.TryGetValue(position, fiveDirection, out Obstacle))
//            {
//                Obstacle.Remove(Obstacle, obstacle);
//            }
//            Debug.LogWarning("移除不存在的障碍物!");
//        }

//        #endregion


//        #region 获取到可行走的方向;

//        /// <summary>
//        /// 获取到可行走的方向;
//        /// </summary>
//        /// <param name="position">位置;</param>
//        /// <param name="key"></param>
//        /// <returns></returns>
//        public Aspect2D GetOpenAspect(IntVector2 position, IKey key)
//        {
//            Aspect2D CloseAspects = AspectHelper.Empty;

//            foreach (var sideAspect in AspectHelper.GetAspect(AspectHelper.FourDirections))
//            {
//                if (!IsOpenSide(position, sideAspect, key))   //若不开放则不允许通过;
//                {
//                    CloseAspects |= AspectHelper.GetSide(sideAspect);
//                }
//            }

//            //查询角方向是否存在墙;
//            foreach (var cornerAspect in AspectHelper.GetAspect(~CloseAspects & AspectHelper.Corners))
//            {
//                if (!IsOpenCorner(position, cornerAspect, key))
//                {
//                    CloseAspects |= cornerAspect;
//                }
//            }

//            return ~CloseAspects & AspectHelper.AroundExceptSelf;
//        }

//        /// <summary>
//        /// 这个节点是否开放;
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
//        /// 这个正方向是否开放;
//        /// </summary>
//        /// <param name="position"></param>
//        /// <param name="fourDirections"></param>
//        /// <param name="key"></param>
//        /// <returns></returns>
//        private bool IsOpenSide(IntVector2 position, Aspect2D fourDirections, IKey key)
//        {
//            bool isOpen = false;
//            Obstacle obstacle;
//            if (m_ObstacleMap.TryGetValue(position, fourDirections, out obstacle))
//            {
//                isOpen |= IsOpen(obstacle, key);
//            }

//            position += AspectHelper.GetVector(fourDirections);
//            if (isOpen && m_ObstacleMap.TryGetValue(position, Aspect2D.Itself, out obstacle))
//            {
//                isOpen |= IsOpen(obstacle, key);
//            }

//            return isOpen;
//        }

//        /// <summary>
//        /// 检查这个角方向是否开放;
//        /// </summary>
//        /// <param name="position"></param>
//        /// <param name="corners"></param>
//        /// <param name="key"></param>
//        /// <returns></returns>
//        private bool IsOpenCorner(IntVector2 position, Aspect2D corners, IKey key)
//        {
//            Obstacle obstacle;
//            if (m_ObstacleMap.TryGetValue(position, corners, out obstacle))
//            {
//                return IsOpen(obstacle, key);
//            }
//            return true;
//        }

//        #endregion

//        private class Obstacle
//        {

//            public Obstacle(IObstacle obstacle)
//            {
//                Add(this, obstacle);
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
//            public IObstacle EffectiveObstacle
//            {
//                get { return m_EffectiveObstacle != null ? m_EffectiveObstacle : m_UnimportantObstacle; }
//            }

//            /// <summary>
//            /// 障碍物数量;
//            /// </summary>
//            public short OccupiedCount
//            {
//                get { return m_OccupiedCount; }
//                private set
//                {
//                    m_OccupiedCount = value;
//#if DEBUG_Navigator
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
//            public static Obstacle Add(Obstacle node, IObstacle obstacle)
//            {

//                //加入障碍物仅为占位;
//                if ((obstacle.ObstacleMode & OccupiedObstacle) > 0)
//                {

//                }
//                //不存在有效障碍物,,且加入障碍物为寻路障碍物;
//                else if (node.m_EffectiveObstacle == null && (obstacle.ObstacleMode & NavigationObstacle) > 0)
//                {
//                    node.m_EffectiveObstacle = obstacle;
//                    if (node.m_UnimportantObstacle != null)
//                        node.m_UnimportantObstacle.Concealment();
//                }
//                //不存在可隐藏障碍物,且加入障碍物为不重要的;
//                else if (node.m_UnimportantObstacle == null && (obstacle.ObstacleMode & UnimportantObstacle) > 0)
//                {
//                    node.m_UnimportantObstacle = obstacle;
//                }
//                else
//                {
//                    throw new ArgumentException("已存在障碍物!");
//                }

//                node.OccupiedCount += OccupiedObstacleIncrease;
//                return node;
//            }

//            /// <summary>
//            /// 移除障碍物;
//            /// </summary>
//            /// <param name="node"></param>
//            /// <param name="obstacle"></param>
//            public static Obstacle Remove(Obstacle node, IObstacle obstacle)
//            {
//                //加入障碍物仅为占位;
//                if ((obstacle.ObstacleMode & OccupiedObstacle) > 0)
//                {

//                }
//                //移除的为有效障碍物;
//                else if (node.m_EffectiveObstacle == obstacle)
//                {
//                    node.m_EffectiveObstacle = null;
//                    if (node.m_UnimportantObstacle != null)
//                        node.m_UnimportantObstacle.Show();
//                }
//                //移除的为可隐藏障碍物;
//                else if (node.m_UnimportantObstacle == obstacle)
//                {
//                    node.m_UnimportantObstacle = null;
//                }
//#if DEBUG_Navigator
//                else
//                {
//                    throw new ArgumentException("准备移除不存在的障碍物!" + obstacle);
//                }
//#endif
//                node.OccupiedCount -= OccupiedObstacleIncrease;
//                return node;
//            }

//        }

//    }

//}
