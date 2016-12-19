using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.GameScene
{

    /// <summary>
    /// 当存在目标时跟随目标,
    /// 若 跟随目标时按方向控制键 或 不存在目标,则转换为 类似 文明系列 的固定方向摄像机;
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(Camera)), ExecuteInEditMode]
    public class FollowTargetCamera : MonoBehaviour
    {
        FollowTargetCamera() { }

        /// <summary>
        /// 在Update 内更新的值输入需要乘上的数值;
        /// </summary>
        static float InputOnUpdate
        {
            get { return Time.deltaTime; }
        }

        /// <summary>
        /// 更随目标;
        /// </summary>
        [SerializeField]
        Transform target;

        /// <summary>
        /// 是否为跟随模式;
        /// </summary>
        [SerializeField]
        bool isFollowMode = false;

        [SerializeField]
        TargetAroundCamera targetCamera;

        [SerializeField]
        CameraMovement cameraMovement;

        /// <summary>
        /// 本 GameObject 绑定的摄像机;
        /// </summary>
        Camera cameraComponent;

        /// <summary>
        /// 摄像机需要移动到的点;
        /// </summary>
        Vector3 cameraTagetPoint;

        /// <summary>
        /// 相机当前移动速度;
        /// </summary>
        Vector3 currentVelocity;

        /// <summary>
        /// 摄像机位置;
        /// </summary>
        Vector3 cameraPoint
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        /// <summary>
        /// 目标位置;
        /// </summary>
        Vector3 targetPoint
        {
            get { return target.position; }
            set { target.position = value; }
        }

        /// <summary>
        /// 更随的目标,若不存在则设置为null;;
        /// </summary>
        public Transform Target
        {
            get { return target; }
            set
            {
                if (value == null)
                    UnFollowTarget();
                else
                    FollowTarget(value);
            }
        }

        void Awake()
        {
            cameraComponent = GetComponent<Camera>();
        }

        void Start()
        {

        }

        void Update()
        {
            Vector3 temp_cameraPoint = cameraPoint;
            //Vector3 temp_targetPoint = targetPoint;

            //if (targetCamera.RotationInput())
            //{
            //    //transform.rotation = Quaternion.LookRotation(target.position - transform.position);
            //}
            targetCamera.RotateAround(transform, target.position);
            //transform.LookAt(target.position);
            //cameraTagetPoint = targetCamera.GetCameranPosition(temp_targetPoint);
            ////transform.rotation = Quaternion.LookRotation(target.position - transform.position);

            if (cameraMovement.Movement(transform))
            {
                //UnFollowTarget();
            }
            else
            {
                //transform.rotation = cameraTagetRotation;
            }

            //cameraTagetPoint = targetCamera.GetCameranPosition(temp_targetPoint);
            //transform.rotation = Quaternion.LookRotation(target.position - transform.position);

            //targetPoint = temp_targetPoint;
            //cameraTagetPoint = temp_cameraPoint;
            //cameraPoint = Vector3.SmoothDamp(cameraPoint, cameraTagetPoint, ref currentVelocity, 0.5f);
        }

        /// <summary>
        /// 开始更随目标;
        /// </summary>
        /// <param name="target"></param>
        void FollowTarget(Transform newTarget)
        {
            this.target = newTarget;
            isFollowMode = true;
        }

        /// <summary>
        /// 停止更随目标;
        /// </summary>
        void UnFollowTarget()
        {
            isFollowMode = false;
        }

        /// <summary>
        /// 围绕目标旋转的摄像机方法集;
        /// </summary>
        [Serializable]
        public class TargetAroundCamera
        {

            /// <summary>
            /// 摄像机旋转时,每个周期的旋转量;
            /// </summary>
            [SerializeField, Range((float)(Math.PI / 180), (float)(Math.PI))]
            float rotationAngle = (float)(Math.PI / 180);

            /// <summary>
            /// 设置旋转速度;
            /// </summary>
            public float RotationAngle
            {
                get { return rotationAngle; }
                set { rotationAngle = value; }
            }

            /// <summary>
            /// 摄像机本身旋转;
            /// </summary>
            public bool RotateAround(Transform camera)
            {
                Vector3 axis;
                bool isInput = RotationInput(out axis);
                if (isInput)
                {
                    camera.Rotate(axis, RotationAngle, Space.World);
                }
                return isInput;
            }

            /// <summary>
            /// 摄像机围绕着目标点旋转;
            /// </summary>
            public bool RotateAround(Transform camera, Vector3 targetPoint)
            {
                Vector3 axis;
                bool isInput = RotationInput(out axis);
                if (isInput)
                {
                    camera.RotateAround(targetPoint, axis, RotationAngle);
                }
                return isInput;
            }

            /// <summary>
            /// 输入更新,若存在输入则返回 true;
            /// </summary>
            public bool RotationInput(out Vector3 axis)
            {
                bool isInput = false;
                axis = new Vector3();

                if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_Rotate_Left))
                {
                    axis.y -= rotationAngle * InputOnUpdate;
                    isInput = true;
                }
                if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_Rotate_Right))
                {
                    axis.y += rotationAngle * InputOnUpdate;
                    isInput = true;
                }
                return isInput;
            }

            /// <summary>
            /// 获取到摄像机应该放置的位置;
            /// </summary>
            public Vector3 CameraPosition(Vector3 target, float angle, float cameraHeight, float withHorizonAngle)
            {
                double currentRadius = (cameraHeight) / Math.Tan(withHorizonAngle * (Math.PI / 180));
                double x = target.x + Math.Sin(angle) * currentRadius;
                double z = target.z + Math.Cos(angle) * currentRadius;
                return new Vector3((float)x, cameraHeight, (float)z);
            }

            /// <summary>
            /// 获取到相机望向目标的旋转量,类似 LookAt();
            /// </summary>
            public Quaternion LookRotation(Vector3 camera, Vector3 target)
            {
                return Quaternion.LookRotation(target - camera);
            }

            /// <summary>
            /// 获取到两个点的角度,基准在正y轴上;
            /// </summary>
            public double AngleY(Vector3 target, Vector3 current)
            {
                double angle = -(Math.Atan2((target.z - current.z), (target.x - current.x)) *
                   (180 / Math.PI) + 90) * (Math.PI / 180);
                return angle;
            }

        }

        /// <summary>
        /// 摄像机移动;
        /// </summary>
        [Serializable]
        public class CameraMovement
        {
            /// <summary>
            /// 相机运动速度;
            /// </summary>
            [SerializeField, Range(0.1f, 10)]
            float movementSpeed = 1;

            /// <summary>
            /// 输入时的运动速度;
            /// </summary>
            public float MovementSpeed
            {
                get { return movementSpeed; }
                set { movementSpeed = value; }
            }

            /// <summary>
            /// 对摄像机进行移动;
            /// </summary>
            public bool Movement(Transform camera)
            {
                Vector3 axis;
                bool isInput = MovementInput(out axis);

                if (isInput)
                {
                    float y = camera.rotation.eulerAngles.y;
                    axis = Quaternion.Euler(0, y, 0) * axis;

                    camera.Translate(axis, Space.World);
                }

                return isInput;
            }


            /// <summary>
            /// 获取到输入的偏移量,若存在输入则返回 true;
            /// </summary>
            bool MovementInput(out Vector3 axis)
            {
                bool isInput = false;
                axis = new Vector3();

                if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_Movement_Up))
                {
                    axis.z += movementSpeed * InputOnUpdate;
                    isInput = true;
                }
                if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_Movement_Down))
                {
                    axis.z -= movementSpeed * InputOnUpdate;
                    isInput = true;
                }
                if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_Movement_Right))
                {
                    axis.x += movementSpeed * InputOnUpdate;
                    isInput = true;
                }
                if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_Movement_Left))
                {
                    axis.x -= movementSpeed * InputOnUpdate;
                    isInput = true;
                }

                return isInput;
            }

        }

    }

}
