using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Terrain3D;
using KouXiaGu.World.Map;
using KouXiaGu.Resources;

namespace KouXiaGu.World
{

    /// <summary>
    /// 初始化游戏场景需要准备的数据;
    /// </summary>
    public interface IBasicData
    {
        IGameResource BasicResource { get; }
        WorldInfo WorldInfo { get; }
    }

    /// <summary>
    /// 游戏场景数据;
    /// </summary>
    public interface IWorld
    {
        IBasicData BasicData { get; }
        IWorldData WorldData { get; }
        IWorldComponents Components { get; }
    }

    public interface IWorldComplete : IWorld, IDisposable
    {
        IWorldUpdater Updater { get; }
    }

    /// <summary>
    /// 游戏场景的数据,存档时进行保存的部分;
    /// </summary>
    public interface IWorldData
    {
        GameMap MapData { get; }
    }

    /// <summary>
    /// 游戏场景的组件;
    /// </summary>
    public interface IWorldComponents
    {
        Landform Landform { get; }
        WorldTime Time { get; }
    }

    /// <summary>
    /// 场景更新组件;
    /// </summary>
    public interface IWorldUpdater : IDisposable
    {
        SceneUpdater LandformUpdater { get; }
        WorldTimeUpdater TimeUpdater { get; }
    }
}
