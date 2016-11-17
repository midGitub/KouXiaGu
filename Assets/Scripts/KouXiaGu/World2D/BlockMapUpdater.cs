using System;
using UnityEngine;
using UniRx;
using System.Threading;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 多线程的协调读取保存;
    /// </summary>
    [Serializable]
    public class BlockMapUpdater
    {
        private BlockMapUpdater() { }

        [SerializeField]
        private MapState state = MapState.None;
        [SerializeField, Tooltip("更新的目标")]
        private Transform followTarget;

        private object stateLock = new object();
        private Vector2 targetPlanePosition;
        private IDisposable disposble;
        private wnBlockMap worldMap;


        public MapState StateForNow
        {
            get { return state; }
        }
        public bool isRunning
        {
            get { return disposble != null; }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Awake(wnBlockMap worldMap)
        {
            if (worldMap == null)
                throw new NullReferenceException();
            if (isRunning)
                throw new Exception("重复调用");

            this.worldMap = worldMap;
            targetPlanePosition = new Vector2(float.MaxValue, float.MaxValue);
        }

        /// <summary>
        /// 开始周期性读取地图;
        /// </summary>
        public void StartLoad()
        {
            disposble = Observable.EveryUpdate().Subscribe(_ => AsynUpdate());
        }

        /// <summary>
        /// 停止下次更新地图的请求;
        /// </summary>
        public void StopLoad()
        {
            disposble.Dispose();
            disposble = null;
            targetPlanePosition = new Vector2(float.MaxValue, float.MaxValue);
        }


        /// <summary>
        /// 保存,若正在读取,则无响应,直到允许保存;
        /// </summary>
        public void Save()
        {
            while (SetState(MapState.Saving))
            {
            }
            worldMap.SaveAllBlock();
            SetState(MapState.None);
        }

        private bool SetState(MapState stateChange)
        {
            lock (stateLock)
            {
                if (this.state == MapState.None || stateChange == MapState.None)
                {
                    this.state = stateChange;
                    return true;
                }
                return false;
            }
        }

        private void AsynUpdate()
        {
            if (followTarget != null && targetPlanePosition != (Vector2)followTarget.position && SetState(MapState.Loading))
            {
                targetPlanePosition = followTarget.position;
                ThreadPool.QueueUserWorkItem(Update);
            }
        }

        private void Update(object state)
        {
            IntVector2 mapPoint = WorldConvert.PlaneToHexPair(targetPlanePosition);
            worldMap.UpdateBlock(mapPoint);
            SetState(MapState.None);
        }

    }

}
