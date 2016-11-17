using System;
using UnityEngine;
using UniRx;

namespace KouXiaGu.World2D
{


    [Serializable]
    public class FollowTargetPosition
    {
        private FollowTargetPosition() { }

        [SerializeField, Tooltip("当目标位置变化时更新")]
        private bool onMoveUpdate;

        [SerializeField, Tooltip("更新的目标")]
        private Transform target;

        public Transform Target
        {
            get { return target; }
            set { target = value; }
        }

        public void Start(Action<Vector2> onUpdate)
        {
            if (onMoveUpdate)
            {
                OnMoveUpdate(onUpdate);
            }
            else
            {
                AlwaysUpdate(onUpdate);
            }
        }

        private void OnMoveUpdate(Action<Vector2> onUpdate)
        {
            Observable.EveryUpdate().
                Where(_ => target != null).
                ObserveEveryValueChanged(_ => target.position).
                Subscribe(_ => onUpdate(target.position), err => Debug.Log(err));
        }

        private void AlwaysUpdate(Action<Vector2> onUpdate)
        {
            Observable.EveryUpdate().
                Where(_ => target != null).
                Subscribe(_ => onUpdate(target.position), err => Debug.Log(err));
        }

    }

}
