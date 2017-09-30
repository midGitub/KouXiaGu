using JiongXiaGu.Unity.Archives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using JiongXiaGu.Unity.Resources.Archives;
using System.Threading;

namespace JiongXiaGu.Unity.RectMaps
{


    [DisallowMultipleComponent]
    public sealed class RectMapSceneController : SceneSington<RectMapSceneController>, IDataInitializeHandle, ISceneArchiveHandle
    {
        RectMapSceneController()
        {
        }

        /// <summary>
        /// 是否正在进行存档操作?
        /// </summary>
        public bool IsArchiving { get; private set; }

        /// <summary>
        /// 当前场景使用的地图;
        /// </summary>
        public WorldMap Map { get; private set; }

        Task IDataInitializeHandle.StartInitialize(Archive archive, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        void ISceneArchiveHandle.Begin()
        {
            IsArchiving = true;
        }

        void ISceneArchiveHandle.Prepare(CancellationToken cancellationToken)
        {
            IsArchiving = false;
        }

        Task ISceneArchiveHandle.Write(Archive archive)
        {
            throw new NotImplementedException();
        }
    }
}
