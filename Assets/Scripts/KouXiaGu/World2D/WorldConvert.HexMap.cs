using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World2D.Map;

namespace KouXiaGu.World2D
{


    public static partial class WorldConvert
    {

        #region 方向向量获取\转换;

        /// <summary>
        /// 存在方向数;
        /// </summary>
        public const int DirectionNumber = 6;

        static readonly Dictionary<int, DirectionVector> DirectionVectorSet = GetDirectionVector();

        /// <summary>
        /// 获取到这个地图坐标这个方向需要偏移的量;
        /// </summary>
        public static ShortVector2 GetVector(ShortVector2 target, HexDirection direction)
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

        static Dictionary<int, DirectionVector> GetDirectionVector()
        {
            var directionVectorSet = new Dictionary<int, DirectionVector>(DirectionNumber);

            directionVectorSet.Add(HexDirection.North, new ShortVector2(0, 1), new ShortVector2(0, 1));
            directionVectorSet.Add(HexDirection.Northeast, new ShortVector2(1, 0), new ShortVector2(1, 1));
            directionVectorSet.Add(HexDirection.Southeast, new ShortVector2(1, -1), new ShortVector2(1, 0));
            directionVectorSet.Add(HexDirection.South, new ShortVector2(0, -1), new ShortVector2(0, -1));
            directionVectorSet.Add(HexDirection.Southwest, new ShortVector2(-1, -1), new ShortVector2(-1, 0));
            directionVectorSet.Add(HexDirection.Northwest, new ShortVector2(-1, 0), new ShortVector2(-1, 1));

            return directionVectorSet;
        }

        static void Add(this IDictionary<int, DirectionVector> directionVectorDictionary,
            HexDirection direction, ShortVector2 oddVector, ShortVector2 evenVector)
        {
            DirectionVector directionVector = new DirectionVector(direction, oddVector, evenVector);
            directionVectorDictionary.Add((int)direction, directionVector);
        }

        /// <summary>
        /// 六边形 x轴奇数位和偶数位 对应方向的偏移向量;
        /// </summary>
        struct DirectionVector
        {
            public DirectionVector(HexDirection direction, ShortVector2 oddVector, ShortVector2 evenVector)
            {
                this.Direction = direction;
                this.OddVector = oddVector;
                this.EvenVector = evenVector;
            }

            public HexDirection Direction { get; private set; }
            public ShortVector2 OddVector { get; private set; }
            public ShortVector2 EvenVector { get; private set; }
        }

        #endregion


        #region 

        /// <summary>
        /// 获取到这个地图结构周围的点 不存在的点返回返回默认值;从 HexDirection 内的最小值开始返回;
        /// </summary>
        public static IEnumerable<T> GetAroundOrDefault<T>(this IMap<ShortVector2, T> map, ShortVector2 target)
        {
            throw new NotImplementedException();
        }

        #endregion

    }

}
