using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World
{

    /// <summary>
    /// 类似 <文明> 游戏的固定镜头;
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(Camera)), ExecuteInEditMode]
    public class LockCamera : MonoBehaviour
    {

        /// <summary>
        /// 更随目标;
        /// </summary>
        [SerializeField]
        Transform followTarget;


        void Update()
        {
           var item = Input.accelerationEvents[0];
        }




    }

}
