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
        IWorldComponent Component { get; }
        IWorldScene Scene { get; }
    }

    public interface IWorldData
    {
        IGameData GameData { get; }
        WorldInfo Info { get; }
        TimeManager Time { get; }
        MapResource Map { get; }
    }

    public interface IWorldComponent
    {
        Landform Landform { get; }
    }

    public interface IWorldScene
    {

    }


}
