//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//namespace KouXiaGu.Map
//{

//    /// <summary>
//    /// 地图平面坐标转换;
//    /// </summary>
//    public static class PositionConvert
//    {

//        /// <summary>
//        /// 获取视窗鼠标所在水平面上的坐标;
//        /// </summary>
//        public static Vector2 MouseToPlanePoint(this Camera camera)
//        {
//            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
//            RaycastHit raycastHit;
//            if (Physics.Raycast(ray, out raycastHit))
//            {
//                return raycastHit.point;
//            }
//            else
//            {
//                throw new Exception("坐标无法确定!检查摄像机之前地面是否存在3D碰撞模块!");
//            }
//        }

//        /// <summary>
//        /// 获取主摄像机视窗鼠标所在水平面上的坐标;
//        /// </summary>
//        public static Vector2 MouseToPlanePoint()
//        {
//            return MouseToPlanePoint(Camera.main);
//        }

//        /// <summary>
//        /// 书平面坐标转换到地图坐标;
//        /// </summary>
//        public static IntVector2 MouseToMapPoint(this GameHexMap buildMap)
//        {
//            Vector2 planePoint = MouseToPlanePoint();
//            return buildMap.PlaneToMapPoint(planePoint);
//        }

//    }

//}
