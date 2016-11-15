
////摄像机是不允许旋转(暂时完成不会旋转的脚本)
//#define CAMERA_NOT_ROTATE

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;
//using UniRx;


//namespace KouXiaGu
//{

//    [DisallowMultipleComponent, RequireComponent(typeof(Camera)), Obsolete]
//    public class CameraController : MonoBehaviour
//    {
//        private CameraController() { }

//        private Camera cameraObject;

//        private Vector2 dragVector2;

//        public Vector2 CenterScreenPoint
//        {
//            get { return new Vector2(cameraObject.pixelWidth / 2, cameraObject.pixelHeight / 2); }
//        }

//        public Vector3 MouseScreenPoint
//        {
//            get { return Input.mousePosition; }
//        }

//        public Vector3 MouseWorldPoint
//        {
//            get { return GetWorldPoint(MouseScreenPoint); }
//        }

//        public Vector2 CameraLookingPoint
//        {
//            get { return GetWorldPoint(CenterScreenPoint); }
//        }

//        private void Awake()
//        {
//            cameraObject = GetComponent<Camera>();
//        }

//        private void Start()
//        {
//            this.ObserveEveryValueChanged(_ => MouseScreenPoint).
//                Where(_ => Input.GetKey(KeyCode.Mouse2)).
//                Subscribe(_ => Drag(MouseWorldPoint));
//        }

//        private void Update()
//        {

//        }

//        public Vector3 GetWorldPoint(Vector2 screenPoint)
//        {
//            Ray ray = cameraObject.ScreenPointToRay(screenPoint);
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
//        /// 获取到摄像机望向地图的坐标;
//        /// </summary>
//        /// <returns></returns>
//        public Vector2 GetCameraLookingWorldPoint()
//        {
//            Vector3 position = transform.position;
//            Vector3 eulerAngles = transform.rotation.eulerAngles;
//#if CAMERA_NOT_ROTATE
//            float r = (float)Math.Abs(Math.Tan(eulerAngles.x * (Math.PI / 180)) * position.z);
//            float x = position.x;
//            float y = r + position.y;
//#else
//            float r = (float)Math.Abs(Math.Tan(eulerAngles.x * (Math.PI / 180)) * position.z);
//            float x = (float)(Math.Sin(eulerAngles.z) * r + position.x);
//            float y = (float)(Math.Cos(eulerAngles.z) * r + position.y);
//#endif
//            return new Vector2(x, y);
//        }

//        /// <summary>
//        /// 将地图坐标转换为摄像机位置;
//        /// </summary>
//        public Vector2 WorldToCamera(Vector2 worldPoint)
//        {
//            float x = worldPoint.x;
//            float y = (float)(Math.Tan(transform.rotation.eulerAngles.x) * Math.PI / 180) * transform.position.z;

//            return new Vector2(x, y);
//        }

//        public void Drag(Vector2 currentWolrdPoint)
//        {
//            transform.position -= (Vector3)((currentWolrdPoint - dragVector2));
//            dragVector2 = currentWolrdPoint;
//        }

//    }

//}
