using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using KouXiaGu.World2D.Map;
using UnityEngine;

namespace KouXiaGu.World2D
{


    /// <summary>
    /// 对世界坐标和其它类型进行转换;
    /// </summary>
    public static class WorldConvert
    {

        #region 六边形定义

        /// <summary>
        /// 六边形直径;
        /// </summary>
        public const float HexOuterDiameter = 2;

        /// <summary>
        /// 地图以外径为2的正六边形排列;
        /// </summary>
        static readonly Hexagon hexagon = new Hexagon() { OuterDiameter = HexOuterDiameter };

        /// <summary>
        /// 地图所使用的六边形;
        /// </summary>
        public static Hexagon MapHexagon
        {
            get { return hexagon; }
        }

        #endregion


        #region 世界位置转换蜂窝地图位置;

        /// <summary>
        /// 将浮点类型向量转换成地图点;
        /// </summary>
        public static PointPair PlaneToHexPair(Vector2 planePoint)
        {
            int x1, x2, y1, y2;
            var points = new PointPair[4];

            GetInterval(planePoint.x, hexagon.DistanceX, out x1, out x2);
            GetInterval(planePoint.y, hexagon.InnerDiameter, out y1, out y2);

            points[0] = new PointPair(hexagon, x1, y1);
            points[1] = new PointPair(hexagon, x1, y2);
            points[2] = new PointPair(hexagon, x2, y1);
            points[3] = new PointPair(hexagon, x2, y2);

            PointPair pointPair = Nearest(planePoint, points);
            return pointPair;
        }

        /// <summary>
        /// 获取到这个数所在的整数区间;
        /// intPoint1 为靠近0的点, intPoint2 为远离0的点;
        /// </summary>
        static void GetInterval(float point, float spacing, out int intPoint1, out int intPoint2)
        {
            intPoint1 = (int)(point / spacing);
            if (point < 0)
            {
                intPoint2 = intPoint1--;
            }
            else
            {
                intPoint2 = intPoint1++;
            }
        }

        /// <summary>
        /// 获取 坐标集 内世界坐标离目标最短的 坐标集;
        /// </summary>
        static PointPair Nearest(Vector2 target, params PointPair[] points)
        {
            float minDistance = Vector2.Distance(target, points[0].HexCenter);
            PointPair minPointPair = points[0];

            for (int i = 1; i < points.Length; i++)
            {
                float distance = Vector2.Distance(target, points[i].HexCenter);
                if (distance < minDistance)
                {
                    minPointPair = points[i];
                    minDistance = distance;
                }
            }

            return minPointPair;
        }

        /// <summary>
        /// 将 六边形编号 转换成 六边形中心点;
        /// </summary>
        public static Vector2 MapToHex(RectCoord mapPoint)
        {
            Vector2 position = new Vector2();
            position.x = hexagon.DistanceX * mapPoint.x;
            position.y = hexagon.InnerDiameter * mapPoint.y;

            if ((mapPoint.x & 1) == 1)
                position.y -= (hexagon.InnerDiameter / 2);

            return position;
        }

        /// <summary>
        /// 平面坐标 转换成 六边形中心点(任意的中心点);
        /// </summary>
        public static Vector2 PlaneToHex(Vector2 planePoint)
        {
            float distanceX2 = (hexagon.DistanceX * 2);
            planePoint.x = ((int)(planePoint.x / distanceX2)) * distanceX2;
            planePoint.y = ((int)(planePoint.y / hexagon.InnerDiameter)) * hexagon.InnerDiameter;
            return planePoint;
        }

        public struct PointPair
        {
            public PointPair(Hexagon hexagon, int mapX, int mapY)
            {
                MapPoint = new RectCoord(mapX, mapY);
                HexCenter = MapToHex(MapPoint);
            }

            public RectCoord MapPoint { get; private set; }
            public Vector2 HexCenter { get; private set; }

            public override string ToString()
            {
                string str = string.Concat("地图坐标:", MapPoint, "六边形坐标:", HexCenter);
                return str;
            }

            public static implicit operator RectCoord(PointPair item)
            {
                return item.MapPoint;
            }
            public static implicit operator Vector2(PointPair item)
            {
                return item.HexCenter;
            }
        }

        #endregion


        #region 鼠标

        /// <summary>
        /// 定义在Unity内触发器所在的层(重要)!
        /// </summary>
        static readonly int SceneAssistMask = LayerMask.GetMask("SceneAssist");

        /// <summary>
        /// 射线最大距离;
        /// </summary>
        const float RayMaxDistance = 5000;

        /// <summary>
        /// 获取视窗鼠标所在水平面上的坐标;
        /// </summary>
        public static Vector2 MouseToPlane(this Camera camera)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, RayMaxDistance, SceneAssistMask, QueryTriggerInteraction.Collide))
            {
                return raycastHit.point;
            }
            else
            {
                throw new Exception("坐标无法确定!检查摄像机之前地面是否存在3D碰撞模块!");
            }
        }

        /// <summary>
        /// 获取主摄像机视窗鼠标所在水平面上的坐标;
        /// </summary>
        public static Vector2 MouseToPlane()
        {
            return MouseToPlane(Camera.main);
        }

        /// <summary>
        /// 获取到主相机到地图坐标;
        /// </summary>
        public static PointPair MouseToHexPair()
        {
            Vector2 mousePlanePoint = MouseToPlane();
            return PlaneToHexPair(mousePlanePoint);
        }

        #endregion


        #region 方向向量获取\转换;

        /// <summary>
        /// 存在方向数;
        /// </summary>
        public const int DirectionNumber = 6;

        /// <summary>
        /// 方向转换偏移量合集;
        /// </summary>
        static readonly Dictionary<int, DirectionVector> DirectionVectorSet = GetDirectionVector();

        static Dictionary<int, DirectionVector> GetDirectionVector()
        {
            var directionVectorSet = new Dictionary<int, DirectionVector>(DirectionNumber);

            directionVectorSet.AddIn(HexDirection.North, new RectCoord(0, 1), new RectCoord(0, 1));
            directionVectorSet.AddIn(HexDirection.Northeast, new RectCoord(1, 0), new RectCoord(1, 1));
            directionVectorSet.AddIn(HexDirection.Southeast, new RectCoord(1, -1), new RectCoord(1, 0));
            directionVectorSet.AddIn(HexDirection.South, new RectCoord(0, -1), new RectCoord(0, -1));
            directionVectorSet.AddIn(HexDirection.Southwest, new RectCoord(-1, -1), new RectCoord(-1, 0));
            directionVectorSet.AddIn(HexDirection.Northwest, new RectCoord(-1, 0), new RectCoord(-1, 1));
            directionVectorSet.AddIn(HexDirection.Self, new RectCoord(0, 0), new RectCoord(0, 0));

            return directionVectorSet;
        }

        static void AddIn(this Dictionary<int, DirectionVector> directionVectorDictionary,
            HexDirection direction, RectCoord oddVector, RectCoord evenVector)
        {
            DirectionVector directionVector = new DirectionVector(direction, oddVector, evenVector);
            directionVectorDictionary.Add((int)direction, directionVector);
        }

        /// <summary>
        /// 获取到这个地图坐标这个方向需要偏移的量;
        /// </summary>
        public static RectCoord GetVector(RectCoord target, HexDirection direction)
        {
            DirectionVector directionVector = DirectionVectorSet[(int)direction];
            if ((target.x & 1) == 1)
            {
                return directionVector.OddVector;
            }
            else
            {
                return directionVector.EvenVector;
            }
        }

        /// <summary>
        /// 六边形 x轴奇数位和偶数位 对应方向的偏移向量;
        /// </summary>
        struct DirectionVector
        {
            public DirectionVector(HexDirection direction, RectCoord oddVector, RectCoord evenVector)
            {
                this.Direction = direction;
                this.OddVector = oddVector;
                this.EvenVector = evenVector;
            }

            public HexDirection Direction { get; private set; }
            public RectCoord OddVector { get; private set; }
            public RectCoord EvenVector { get; private set; }
        }


        const int maxDirectionMark = (int)HexDirection.Self;
        const int minDirectionMark = (int)HexDirection.North;

        /// <summary>
        /// 按标记为从 高位到低位 循序排列的数组;不包含本身
        /// </summary>
        static readonly HexDirection[] HexDirectionsArray = new HexDirection[]
        {
            HexDirection.Northwest,
            HexDirection.Southwest,
            HexDirection.South,
            HexDirection.Southeast,
            HexDirection.Northeast,
            HexDirection.North,
        };

        /// <summary>
        /// 按标记为从 高位到低位 循序排列的数组;
        /// </summary>
        static readonly HexDirection[] HexDirectionsAndSelfArray = Enum.GetValues(typeof(HexDirection)).
            Cast<HexDirection>().Reverse().ToArray();

        /// <summary>
        /// 按标记为从 高位到低位 循序返回的迭代结构;不包含本身
        /// </summary>
        public static IEnumerable<HexDirection> HexDirections()
        {
            return HexDirectionsArray;
        }

        /// <summary>
        /// 获取到从 高位到低位 顺序返回的迭代结构;包括本身;
        /// </summary>
        public static IEnumerable<HexDirection> HexDirectionsAndSelf()
        {
            return HexDirectionsAndSelfArray;
        }

        /// <summary>
        /// 获取到方向集表示的所有方向;
        /// </summary>
        public static IEnumerable<HexDirection> HexDirections(HexDirection directions)
        {
            int mask = (int)directions;
            for (int intDirection = minDirectionMark; intDirection <= maxDirectionMark; intDirection <<= 1)
            {
                if ((intDirection & mask) == 1)
                {
                    yield return (HexDirection)intDirection;
                }
            }
        }

        #endregion


        #region 蜂窝地图拓展;

        /// <summary>
        /// 获取到这个地图结构周围的点 不存在的点返回返回默认值;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<T> GetNeighboursOrDefault<T>(this IHexMap<RectCoord, T> map, RectCoord target)
        {
            T item;
            IEnumerable<RectCoord> aroundPoints = GetNeighboursPoints(target);
            foreach (var point in aroundPoints)
            {
                if (!map.TryGetValue(point, out item))
                {
                    item = default(T);
                }
                yield return item;
            }
        }

        /// <summary>
        /// 获取到这个地图结构周围的点 不存在的点返回返回默认值;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<T> GetNeighboursAndSelfOrDefault<T>(this IHexMap<RectCoord, T> map, RectCoord target)
        {
            T item;
            IEnumerable<RectCoord> aroundPoints = GetNeighboursAndSelfPoints(target);
            foreach (var point in aroundPoints)
            {
                if (!map.TryGetValue(point, out item))
                {
                    item = default(T);
                }
                yield return item;
            }
        }

        /// <summary>
        /// 获取到这个地图结构周围的点 不存在的点返回返回默认值;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<T> GetNeighboursOrDefault<T>(this IHexMap<RectCoord, T> map, RectCoord target, HexDirection directions)
        {
            T item;
            IEnumerable<RectCoord> aroundPoints = GetNeighboursPoints(target, directions);
            foreach (var point in aroundPoints)
            {
                if (!map.TryGetValue(point, out item))
                {
                    item = default(T);
                }
                yield return item;
            }
        }

        /// <summary>
        /// 获取到这个地图结构周围的点,若不存在则不返回;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<KeyValuePair<RectCoord, T>> GetNeighbours<T>(this IHexMap<RectCoord, T> map, RectCoord target)
        {
            T item;
            IEnumerable<RectCoord> aroundPoints = GetNeighboursPoints(target);
            foreach (var point in aroundPoints)
            {
                if (map.TryGetValue(point, out item))
                {
                    yield return new KeyValuePair<RectCoord, T>(point, item);
                }
            }
        }

        /// <summary>
        /// 获取到这个地图结构周围的点,若不存在则不返回;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<KeyValuePair<RectCoord, T>> GetNeighboursAndSelf<T>(this IHexMap<RectCoord, T> map, RectCoord target)
        {
            T item;
            IEnumerable<RectCoord> aroundPoints = GetNeighboursAndSelfPoints(target);
            foreach (var point in aroundPoints)
            {
                if (map.TryGetValue(point, out item))
                {
                    yield return new KeyValuePair<RectCoord, T>(point, item);
                }
            }
        }

        /// <summary>
        /// 获取到这个地图结构周围的点,若不存在则不返回;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<KeyValuePair<RectCoord, T>> GetNeighbours<T>(this IHexMap<RectCoord, T> map, RectCoord target, HexDirection directions)
        {
            T item;
            IEnumerable<RectCoord> aroundPoints = GetNeighboursPoints(target, directions);
            foreach (var point in aroundPoints)
            {
                if (map.TryGetValue(point, out item))
                {
                    yield return new KeyValuePair<RectCoord, T>(point, item);
                }
            }
        }

        /// <summary>
        /// 获取到这个点周围的坐标;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<RectCoord> GetNeighboursPoints(RectCoord target)
        {
            foreach (var direction in HexDirections())
            {
                RectCoord point = GetVector(target, direction);
                yield return point + target;
            }
        }

        /// <summary>
        /// 获取到这个点本身和周围的坐标;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<RectCoord> GetNeighboursAndSelfPoints(RectCoord target)
        {
            foreach (var direction in HexDirectionsAndSelf())
            {
                RectCoord point = GetVector(target, direction);
                yield return point + target;
            }
        }

        /// <summary>
        /// 获取到这个点这些范围内的坐标;
        /// </summary>
        public static IEnumerable<RectCoord> GetNeighboursPoints(RectCoord target, HexDirection directions)
        {
            foreach (var direction in HexDirections(directions))
            {
                RectCoord point = GetVector(target, direction);
                yield return point + target;
            }
        }

        #endregion

        #region 方向标记拓展;

        /// <summary>
        /// 获取到满足条件的方向;若方向不存在节点则为不满足;
        /// </summary>
        public static HexDirection GetAroundAndSelfMask<T>(this IHexMap<RectCoord, T> map, RectCoord target, Func<T, bool> func)
        {
            HexDirection directions = 0;
            T item;
            IEnumerable<HexDirection> aroundDirection = HexDirectionsAndSelf();
            foreach (var direction in aroundDirection)
            {
                RectCoord vePoint = GetVector(target, direction) + target;
                if (map.TryGetValue(vePoint, out item))
                {
                    if (func(item))
                        directions |= direction;
                }
            }
            return directions;
        }

        #endregion

    }

}
