using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using KouXiaGu.World;

namespace KouXiaGu.RectTerrain
{

    /// <summary>
    /// 负责地形组件更新;
    /// </summary>
    [DisallowMultipleComponent]
    public class RectTerrainUpdater : MonoBehaviour, IUpdaterInitializer
    {
        RectTerrainUpdater()
        {
        }

        [SerializeField]
        RectTerrainController terrainController;
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
        public bool IsUpdating
        {
            get { return updateTask != null; }
        }

        public LandformUpdater LandformUpdater
        {
            get { return terrainController.Landform.Updater; }
        }

        void OnDestroy()
        {
            tokenSource.Cancel();
        }

        Task IUpdaterInitializer.StartInitialize(CancellationToken token)
        {
            StartUpdate();
            return Task.Run(delegate ()
            {

                while (UpdateTimes < 1)
                {
                }

                while (!terrainController.Landform.IsBakeComplete)
                {
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
                tokenSource = new CancellationTokenSource();
                updateTask = Task.Run((Action)TerrainUpdate, tokenSource.Token);
            }
        }

        /// <summary>
        /// 取消更新;
        /// </summary>
        public void CancelUpdate()
        {
            if (IsUpdating)
            {
                tokenSource.Cancel();
                tokenSource = null;
                updateTask = null;
            }
        }

        void TerrainUpdate()
        {
            while (true)
            {
                LandformUpdater.Update();
                UpdateTimes++;
                Thread.Sleep(updateInterval);
            }
        }
    }
}
