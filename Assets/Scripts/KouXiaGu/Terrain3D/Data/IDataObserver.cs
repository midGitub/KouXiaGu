using KouXiaGu.Collections;
using KouXiaGu.Grids;
using UniRx;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 监视地形数据变化;
    /// </summary>
    public interface IDataObserver : IObserver<DictionaryChange<CubicHexCoord, TerrainNode>>
    {

    }

}
