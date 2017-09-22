using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using JiongXiaGu.World;

namespace JiongXiaGu.RectTerrain
{

    /// <summary>
    /// 负责地形组件更新;
    /// </summary>
    [DisallowMultipleComponent]
    public class RectTerrainUpdater : MonoBehaviour, IUpdaterInitializeHandle
    {
        RectTerrainUpdater()
        {
        }

        [SerializeField]
        RectTerrainController terrainController;

        /// <summary>
        /// 更新间隔时间(毫秒);
        /// </summary>
        [SerializeField]
        int updateInterval;

        Task updateTask;
        CancellationTokenSource tokenSource;

        /// <summary>
        /// 更新次数;
        /// </summary>
        public int UpdateTimes { get; private set; }

        /// <summary>
        /// 是否正在更新;
        /// </summary>
        public bool IsUpdating { get; private set; }

        public LandformUpdater LandformUpdater
        {
            get { return terrainController.Landform.Updater; }
        }

        void OnDestroy()
        {
            CancelUpdate();
        }

        Task IUpdaterInitializeHandle.StartInitialize(CancellationToken token)
        {
            StartUpdate();
            return Task.Run(delegate ()
            {

                while (UpdateTimes < 1)
                {
                    token.ThrowIfCancellationRequested();
                }

                while (!terrainController.Landform.IsBakeComplete)
                {
                    token.ThrowIfCancellationRequested();
                }

                Debug.Log("[地形更新器]更新完成;");
            }, token);
        }

        /// <summary>
        /// 开始更新;
        /// </summary>
        public void StartUpdate()
        {
            if (!IsUpdating)
            {
                IsUpdating = true;
                tokenSource = new CancellationTokenSource();
                updateTask = Task.Run(() => TerrainUpdate(tokenSource.Token), tokenSource.Token);
            }
        }

        /// <summary>
        /// 取消更新;
        /// </summary>
        public void CancelUpdate()
        {
            if (IsUpdating)
            {
                IsUpdating = false;
                tokenSource.Cancel();
                tokenSource = null;
                updateTask = null;
            }
        }

        void TerrainUpdate(CancellationToken token)
        {
            while (true)
            {
                token.ThrowIfCancellationRequested();
                LandformUpdater.Update();
                UpdateTimes++;
                Thread.Sleep(updateInterval);
            }
        }
    }
}
