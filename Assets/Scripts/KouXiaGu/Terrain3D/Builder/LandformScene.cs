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
    public class LandformScene : IUnityThreadBehaviour<Action>
    {
        public LandformScene(LandformBuilder builder)
        {
            this.builder = builder;
            displayCoords = new HashSet<RectCoord>();
        }

        readonly LandformBuilder builder;
        readonly HashSet<RectCoord> displayCoords;

        IReadOnlyDictionary<RectCoord, IAsyncOperation<Chunk>> SceneDisplayedChunks
        {
            get { return builder.SceneDisplayedChunks; }
        }

        public object Sender
        {
            get { return "场景的地形创建销毁管理"; }
        }

        public Action Action
        {
            get { return OnLateUpdateSendDisplay; }
        }

        public void Display(IEnumerable<RectCoord> displayedCoord)
        {
            displayCoords.UnionWith(displayedCoord);
        }

        void OnLateUpdateSendDisplay()
        {
            foreach (var coord in displayCoords)
            {
                builder.Create(coord);
            }
        }

    }

}
