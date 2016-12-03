using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{


    public static class MouseConvert
    {

        #region 鼠标

        /// <summary>
        /// 定义在Unity内触发器所在的层(重要)!
        /// </summary>
        static readonly int SceneAssistMask = LayerMask.GetMask("SceneAssist");

        /// <summary>
        /// 射线最大距离;
        /// </summary>
        const float RayMaxDistance = 5000;

        /// <summary>
        /// 获取视窗鼠标所在水平面上的坐标;
        /// </summary>
        public static Vector3 MouseToPlane(this Camera camera)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, RayMaxDistance, SceneAssistMask, QueryTriggerInteraction.Collide))
            {
                return raycastHit.point;
            }
            else
            {
                Debug.LogWarning("坐标无法确定!检查摄像机之前地面是否存在3D碰撞模块!");
                return default(Vector3);
            }
        }

        /// <summary>
        /// 获取主摄像机视窗鼠标所在水平面上的坐标;
        /// </summary>
        public static Vector3 MouseToPixel()
        {
            return MouseToPlane(Camera.main);
        }

        /// <summary>
        /// 获取到主相机到地图坐标;
        /// </summary>
        public static ShortVector2 MouseToOffset()
        {
            Vector3 mousePlanePoint = MouseToPixel();
            return HexGrids.PixelToOffset(mousePlanePoint);
        }

        #endregion

    }

}
