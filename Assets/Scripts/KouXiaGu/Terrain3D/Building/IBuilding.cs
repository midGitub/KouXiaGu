using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Terrain3D
{
    /// <summary>
    /// 场景建筑物实例;
    /// </summary>
    public interface IBuilding
    {
        /// <summary>
        /// 重新构建建筑(当地形发生变化时调用);
        /// </summary>
        void Rebuild();

        /// <summary>
        /// 销毁这个建筑物;
        /// </summary>
        void Destroy();
    }
}
