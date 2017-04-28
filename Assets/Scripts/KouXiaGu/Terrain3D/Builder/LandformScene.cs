using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 场景管理,负责对场景地形块的创建和销毁进行管理;
    /// </summary>
    class LandformScene
    {
        public LandformScene(LandformBuilder builder)
        {
            this.builder = builder;
        }

        readonly LandformBuilder builder;
        readonly HashSet<RectCoord> display;

        IReadOnlyDictionary<RectCoord, IAsyncOperation<Chunk>> SceneDisplayedChunks
        {
            get { return builder.SceneDisplayedChunks; }
        }

        public void Display(IEnumerable<RectCoord> displayedCoord)
        {
            display.UnionWith(displayedCoord);
        }

    }

}
