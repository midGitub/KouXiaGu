//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//namespace KouXiaGu.Terrain3D.Navigation
//{

//    /// <summary>
//    /// 导航路径;
//    /// </summary>
//    public interface INavigationPath
//    {

//        /// <summary>
//        /// 移动到的最大速度;
//        /// </summary>
//        float MaxSpeed { get; }

//        /// <summary>
//        /// 移动到的位置;
//        /// </summary>
//        Vector3 Position { get; }

//        /// <summary>
//        /// 移动到下一个位置,若已经到终点则返回false,否则返回true;
//        /// </summary>
//        bool MoveNext();

//        /// <summary>
//        /// 在不使用这路线时调用;
//        /// </summary>
//        void Complete();

//    }

//}
