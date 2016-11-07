using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XGame.Running.Map.Guidances
{

    public interface IObstacle
    {

        /// <summary>
        /// 障碍物属性;
        /// </summary>
        ObstacleMode ObstacleMode { get; }

        /// <summary>
        /// 查询是否允许通过这个障碍物;
        /// </summary>
        /// <param name="key"></param>
        /// <returns>允许通过为true,否则为false;</returns>
        bool IsOpen(IKey key);

        /// <summary>
        /// 请求隐藏该障碍物;
        /// </summary>
        void Concealment();

        /// <summary>
        /// 请求恢复隐藏的障碍物;
        /// </summary>
        void Show();

    }

}
