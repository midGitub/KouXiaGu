using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JiongXiaGu
{

    /// <summary>
    /// 让目标跟随自己,目标Y轴一直为0;
    /// </summary>
    internal class FollowY0 : MonoBehaviour
    {

        /// <summary>
        /// 跟随在其后的物体;
        /// </summary>
        [SerializeField]
        Transform follower;

        /// <summary>
        /// 跟随在其后的物体;
        /// </summary>
        public Transform Target
        {
            get { return follower; }
            set { follower = value; }
        }

        void Update()
        {
            if (follower != null)
            {
                Vector3 position = transform.position;
                follower.position = new Vector3(position.x, 0, position.z);
            }
        }

    }

}
