using JiongXiaGu.Grids;

namespace JiongXiaGu.Unity.RectTerrain
{
    public interface IBakeContent
    {
        RectCoord Target { get; }
        BakeNodeInfo GetInfo(RectCoord pos);
    }
}
