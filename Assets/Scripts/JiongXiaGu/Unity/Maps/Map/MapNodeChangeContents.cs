using System;

namespace JiongXiaGu.Unity.Maps
{
    /// <summary>
    /// 地图节点变化内容;
    /// </summary>
    [Flags]
    public enum MapNodeChangeContents
    {
        None = 0,
        Landform = 1 >> 0,
        Building = 1 >> 1,
    }
}
