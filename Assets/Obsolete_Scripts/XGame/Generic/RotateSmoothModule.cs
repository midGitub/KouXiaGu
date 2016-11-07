using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XGame
{


    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public class RotateSmoothModule : RotateModule
    {

        protected RotateSmoothModule() { }

        [SerializeField, Tooltip("旋转速度,数值越大越快")]
        private float rotateSpeed = 1;

        /// <summary>
        /// 旋转到的目标;根据此值来控制旋转;
        /// </summary>
        private Quaternion targetQuaternion;

        /// <summary>
        /// 旋转速度,数值越大越快
        /// </summary>
        public float RotateSpeed
        {
            get { return rotateSpeed; }
            set { rotateSpeed = value; }
        }

        private void Update()
        {
            quaternion = Quaternion.RotateTowards(quaternion, targetQuaternion, rotateSpeed);
        }

        protected override void RotateTo(Aspect rotation)
        {
            targetQuaternion = AspectHelper.GetQuaternion(rotation);
        }

    }

}
