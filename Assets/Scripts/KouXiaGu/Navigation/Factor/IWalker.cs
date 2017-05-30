using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Navigation
{

    public interface IWalkableConfirmer<T>
    {
        /// <summary>
        /// 该位置是否允许行走?
        /// </summary>
        bool IsWalkable(T position);
    }

    public interface ICostCalculator<T>
    {
        /// <summary>
        /// 获取走入到 target 位置的代价值;
        /// </summary>
        /// <param name="position">当前位置;</param>
        /// <param name="target">走入到的位置;</param>
        /// <param name="destination">终点位置;</param>
        /// <returns></returns>
        int GetCost(T position, T target, T destination);
    }

    public interface IWalker<T> : IWalkableConfirmer<T>, ICostCalculator<T>
    {
    }
}
