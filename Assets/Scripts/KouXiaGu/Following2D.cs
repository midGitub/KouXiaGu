using System;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 根据挂在物体位置跟随目标(非平滑);
    /// </summary>
    [DisallowMultipleComponent, ExecuteInEditMode]
    public class Following2D : MonoBehaviour
    {

        [SerializeField]
        private Transform target;

        [SerializeField]
        private Vector2 spans;

        private Transform current;
        private Vector3 previousPosition;

        public Transform Target
        {
            get { return target; }
            set { SetTarget(value); }
        }

        public Vector2 Spans
        {
            get { return spans; }
            set { spans = value; }
        }

        private void Awake()
        {
            current = transform;
        }

        private void Start()
        {
            if (this.target != null)
            {
                SetTarget(this.target);
            }
        }

        private void Update()
        {
            if (target != null && target.position != previousPosition)
            {
                CurrentUpdate(previousPosition);
                previousPosition = target.position;
            }
        }

        private void SetTarget(Transform target)
        {
            this.target = target;
            previousPosition = target.position;
            CurrentUpdate(previousPosition);
        }

        private void CurrentUpdate(Vector3 targetPosition)
        {
            Vector3 currentPosition = this.current.position;

            currentPosition.x = MoveTo(targetPosition.x, currentPosition.x, spans.x);
            currentPosition.y = MoveTo(targetPosition.y, currentPosition.y, spans.y);

            this.current.position = currentPosition;
        }

        /// <summary>
        /// 向目标移动,但是限制在跨度范围内;例如:
        /// target = 4 , current = 0, spans = 3,返回:3
        /// target = 5 , current = 3, spans = 3,返回:3
        /// </summary>
        /// <param name="target">目标数值</param>
        /// <param name="current">当前数值</param>
        /// <param name="spans">从0开始的跨步;</param>
        /// <returns></returns>
        private float MoveTo(float target, float current, float spans)
        {
            float distanceX = target - current;
            int multiple = (int)(distanceX / spans);

            if (multiple >= 1 || multiple <= -1)
            {
                return current + (spans * multiple);
            }

            return current;
        }

    }

}
