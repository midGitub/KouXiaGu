using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.GameScene
{

    [DisallowMultipleComponent, ExecuteInEditMode]
    public class LookForward2D : MonoBehaviour
    {

        [SerializeField]
        private Transform target;

        private Transform current;

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
            LookForward(target.forward);
        }

        private void SetTarget(Transform target)
        {
            this.target = target;
            LookForward(target.forward);
        }

        private void LookForward(Vector3 targetForward)
        {
            current.forward = targetForward;
        }


    }

}
