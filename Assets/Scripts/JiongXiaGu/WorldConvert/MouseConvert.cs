using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JiongXiaGu
{

    /// <summary>
    /// 鼠标到地图坐标的转换;
    /// </summary>
    public static class MouseConvert
    {

        /// <summary>
        /// 碰撞层名称;
        /// </summary>
        const string layerName = "SceneAssist";

        /// <summary>
        /// 定义在Unity内触发器所在的层(重要)!
        /// </summary>
        static readonly int layerMask = LayerMask.GetMask(layerName);

        static readonly int layer = LayerMask.NameToLayer(layerName);

        /// <summary>
        /// 射线最大距离;
        /// </summary>
        const float RayMaxDistance = 800000f;

        /// <summary>
        /// 从屏幕坐标获取到 y 轴 为 0 的平面坐标;
        /// </summary>
        public static Vector3 ScreenToPixel(this Camera camera, Vector3 screenPoint)
        {
            Start:
            Ray ray = camera.ScreenPointToRay(screenPoint);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, RayMaxDistance, layerMask, QueryTriggerInteraction.Collide))
            {
                return raycastHit.point;
            }
            else
            {
                if (camera.GetComponent<FollowY0>() == null)
                {
                    CreateCollider(camera.transform);
                    goto Start;
                }

                Debug.LogWarning("坐标无法确定!检查摄像机之前地面是否存在3D碰撞模块!");
                return default(Vector3);
            }
        }

        /// <summary>
        /// 获取视窗鼠标所在水平面上的坐标;
        /// </summary>
        public static Vector3 MouseToPixel(this Camera camera)
        {
            return camera.ScreenToPixel(Input.mousePosition);
        }

        /// <summary>
        /// 获取主摄像机视窗鼠标所在水平面上的坐标;
        /// </summary>
        public static Vector3 MouseToPixel()
        {
            return MouseToPixel(Camera.main);
        }

        /// <summary>
        /// 创建一个碰撞器跟随相机;
        /// </summary>
        static void CreateCollider(Transform camera)
        {
            GameObject collider = new GameObject("MouseCollider");
            var boxCollider = collider.AddComponent<BoxCollider>();

            boxCollider.isTrigger = true;
            collider.transform.localScale = new Vector3(50, 0, 50);
            collider.layer = layer;

            var component = camera.gameObject.AddComponent<FollowY0>();
            component.Target = collider.transform;
        }

    }

}
