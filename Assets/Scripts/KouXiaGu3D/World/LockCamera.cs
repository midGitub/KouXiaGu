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
        /// 鼠标滚轮滑动 Axis 名;
        /// </summary>
        const string MOUSE_SCROLL_WHEEL = "Mouse ScrollWheel";

        /// <summary>
        /// 更随目标,需要一直存在的;
        /// </summary>
        [SerializeField]
        Transform followTarget;

        [SerializeField]
        float minHeight = 0;
        [SerializeField]
        float maxHeight = 500;

        Vector3 currentPos;
        Vector3 newPos;

        void Update()
        {
            newPos = new Vector3();
            currentPos = transform.position;

            newPos.y = currentPos.y - Input.GetAxis(MOUSE_SCROLL_WHEEL);
            newPos.y = Mathf.Clamp(newPos.y, minHeight, maxHeight);

            transform.position = newPos;
        }


    }

}
