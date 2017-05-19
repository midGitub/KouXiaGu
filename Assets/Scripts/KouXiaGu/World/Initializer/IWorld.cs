using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Terrain3D;
using KouXiaGu.World.Map;

namespace KouXiaGu.World
{

    public interface IWorld
    {
        IWorldData Data { get; }
        IWorldScene Component { get; }
    }

    public interface IWorldData
    {
        IGameData GameData { get; }
        WorldInfo Info { get; }
        TimeManager Time { get; }
        GameMap Map { get; }
    }

    public interface IWorldScene
    {
        Landform Landform { get; }
    }

}
