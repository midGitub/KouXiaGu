using KouXiaGu.Collections;
using KouXiaGu.Grids;
using UniRx;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 监视地图变化;
    /// </summary>
    public interface ITerrainMapObserver : IObserver<DictionaryChange<CubicHexCoord, TerrainNode>>
    {

    }

}
