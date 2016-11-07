using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XGame
{

    [DisallowMultipleComponent]
    public class RotateModule : RotateAbstract
    {
        protected RotateModule() { }

        /// <summary>
        /// 旋转;根据此值来控制旋转;
        /// </summary>
        protected Quaternion quaternion
        {
            get { return transform.rotation; }
            set { transform.rotation = value; }
        }

        /// <summary>
        /// 设置旋转到的方向;
        /// </summary>
        /// <param name="rotation"></param>
        protected override void RotateTo(Aspect rotation)
        {
            this.quaternion = AspectHelper.GetQuaternion(rotation);
        }


    }

}
