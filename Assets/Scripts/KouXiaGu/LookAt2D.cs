using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 挂在物体面向目标物体;
    /// </summary>
    [DisallowMultipleComponent, ExecuteInEditMode]
    public class LookAt2D : MonoBehaviour
    {

        [SerializeField]
        private Transform target;

        private Transform current;
        private Vector3 previousPosition;

        public Transform Target
        {
            get { return target; }
            set { SetTarget(value); }
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
                LookAt(previousPosition);
                previousPosition = target.position;
            }
        }

        private void SetTarget(Transform target)
        {
            this.target = target;
            previousPosition = target.position;
            LookAt(previousPosition);
        }

        private void LookAt(Vector3 targetPosition)
        {
            current.LookAt(targetPosition, Vector3.back);
        }

    }

}
