using System;
using UnityEngine;
using UniRx;
using System.Threading;

namespace KouXiaGu.World2D
{


    [Serializable]
    public class FollowTargetPosition
    {
        private FollowTargetPosition() { }

        /// <summary>
        /// 正在读取;
        /// </summary>
        [SerializeField]
        private bool loading = false;
        [SerializeField, Tooltip("更新的目标")]
        private Transform followTarget;
        [SerializeField, Tooltip("当目标位置变化时更新")]
        private bool onMoveUpdate;

        private Vector2 targetPosition;
        private IDisposable disposble;
        Action<Vector2> onUpdate;

        public void StartAsyn(Action<Vector2> onUpdate)
        {
            this.onUpdate = onUpdate;

            if (onMoveUpdate)
                OnMoveUpdate();
            else
                AlwaysUpdate();
        }

        public void Stop()
        {
            disposble.Dispose();
        }

        private void OnMoveUpdate()
        {
            disposble = Observable.EveryUpdate().
                Where(_ => followTarget != null && !loading).
                 ObserveEveryValueChanged(_ => (Vector2)followTarget.position).
                 Subscribe(_ => UpdateInThreah());
        }

        private void AlwaysUpdate()
        {
            disposble = Observable.EveryUpdate().
                Where(_ => followTarget != null && !loading).
                Subscribe(_ => UpdateInThreah());
        }

        private void UpdateInThreah()
        {
            targetPosition = followTarget.position;
            loading = true;
            ThreadPool.QueueUserWorkItem(Update);
        }

        private void Update(object state)
        {
            onUpdate(targetPosition);
            loading = false;
        }

    }

}
