using UnityEngine;

namespace KouXiaGu.Terrain3D.Navigation
{

    public interface INavigator
    {

        /// <summary>
        /// 是否正在跟随路径?
        /// </summary>
        bool IsFollowPath { get; }

        /// <summary>
        /// 所在位置;
        /// </summary>
        Vector3 Position { get; }

        /// <summary>
        /// 跟随路径;
        /// </summary>
        void Follow(INavigationPath path);

        /// <summary>
        /// 停止跟随路径;
        /// </summary>
        void Stop();

    }

}