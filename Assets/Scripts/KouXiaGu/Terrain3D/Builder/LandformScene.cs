﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    public interface ILandformWatcher
    {
        void UpdateDispaly(LandformScene scene);
    }

    /// <summary>
    /// 场景管理,负责对场景地形块的创建和销毁进行管理;
    /// </summary>
    public class LandformScene : IUnityThreadBehaviour<Action>
    {
        public LandformScene(LandformBuilder builder)
        {
            this.builder = builder;
            createCoords = new HashSet<RectCoord>();
            destroyCoords = new List<RectCoord>();
            watcherList = new List<ILandformWatcher>();
            OnUpdateSendDisplay();
            this.SubscribeUpdate();
        }

        readonly LandformBuilder builder;
        readonly HashSet<RectCoord> createCoords;
        readonly List<RectCoord> destroyCoords;
        readonly List<ILandformWatcher> watcherList;

        IReadOnlyDictionary<RectCoord, IAsyncOperation<Chunk>> sceneDisplayedChunks
        {
            get { return builder.SceneDisplayedChunks; }
        }

        IEnumerable<RectCoord> sceneCoords
        {
            get { return sceneDisplayedChunks.Keys; }
        }

        object IUnityThreadBehaviour<Action>.Sender
        {
            get { return "场景的地形块创建销毁管理"; }
        }

        Action IUnityThreadBehaviour<Action>.Action
        {
            get { return OnUpdateSendDisplay; }
        }

        public ICollection<ILandformWatcher> WatcherCollection
        {
            get { return watcherList; }
        }

        public void Display(IEnumerable<RectCoord> coords)
        {
            createCoords.UnionWith(coords);
        }

        void OnUpdateSendDisplay()
        {
            UpdateDispalyCoords();

            ICollection<RectCoord> needDestroyCoords = GetNeedDestroyCoords();
            foreach (var coord in needDestroyCoords)
            {
                this.builder.Destroy(coord);
            }

            ICollection<RectCoord> needCreateCoords = GetNeedCreateCoords();
            foreach (var coord in needCreateCoords)
            {
                this.builder.Create(coord);
            }

            createCoords.Clear();
            destroyCoords.Clear();
        }

        void UpdateDispalyCoords()
        {
            foreach (var item in watcherList)
            {
                item.UpdateDispaly(this);
            }
        }

        ICollection<RectCoord> GetNeedDestroyCoords()
        {
            foreach (var coord in sceneCoords)
            {
                if (!createCoords.Contains(coord))
                    destroyCoords.Add(coord);
            }
            return destroyCoords;
        }

        ICollection<RectCoord> GetNeedCreateCoords()
        {
            createCoords.ExceptWith(sceneCoords);
            return createCoords;
        }

    }

}
