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

        const string ImaginaryTargetName = "CameraImaginaryTarget";

        /// <summary>
        /// 在Update 内更新的值输入需要乘上的数值;
        /// </summary>
        static float InputOnUpdate
        {
            get { return Time.deltaTime; }
        }

        /// <summary>
        /// 在当前的目标位置放置一个假象的目标,若传入的当前目标也为假象的,则直接返回;
        /// </summary>
        static Transform GetImaginaryTarget()
        {
            GameObject obj = new GameObject(ImaginaryTargetName);
            Transform imaginaryTarget = obj.transform;
            return imaginaryTarget;
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
        FreeMovementCamera movementCamera;

        /// <summary>
        /// 本 GameObject 绑定的摄像机;
        /// </summary>
        Camera cameraComponent;

        /// <summary>
        /// 摄像机需要移动到的点;
        /// </summary>
        Vector3 cameraTagetPoint;
        Vector3 currentVelocity;

        Transform imaginaryTarget;

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

        Transform ImaginaryTarget
        {
            get { return imaginaryTarget ?? (imaginaryTarget = GetImaginaryTarget()); }
        }

        /// <summary>
        /// 更随的目标,若不存在则设置为null;;
        /// </summary>
        public Transform Target
        {
            get { return target; }
            set
            {
                Transform newTarget;
                if (value == null)
                {
                    newTarget = ImaginaryTarget;
                    isFollowMode = false;
                }
                else
                {
                    newTarget = value;
                    isFollowMode = true;
                }
                this.target = newTarget;
            }
        }

        void Awake()
        {
            cameraComponent = GetComponent<Camera>();
        }

        void Start()
        {
            if (Target == null)
                Target = null;
        }

        void Update()
        {
            Vector3 temp_cameraPoint = cameraPoint;
            Vector3 temp_targetPoint = targetPoint;

            targetCamera.RotationInput();
            movementCamera.MovementInput(ref temp_targetPoint);

            cameraTagetPoint = targetCamera.GetCameranPosition(temp_targetPoint);
            cameraComponent.transform.LookAt(temp_targetPoint);

            targetPoint = temp_targetPoint;
            cameraPoint = Vector3.SmoothDamp(temp_cameraPoint, cameraTagetPoint, ref currentVelocity, 0.1f);
        }

        /// <summary>
        /// 围绕目标旋转的摄像机;
        /// </summary>
        [Serializable]
        public class TargetAroundCamera
        {

            /// <summary>
            /// 摄像机旋转角度;
            /// </summary>
            [SerializeField, Range(0f, (float)Math.PI)]
            float rotation = (float)Math.PI;

            /// <summary>
            /// 摄像机旋转时,每个周期的旋转量;
            /// </summary>
            [SerializeField, Range((float)(Math.PI / 180), (float)(Math.PI))]
            float rotationSpeed = (float)(Math.PI / 180);

            /// <summary>
            /// 地平线角度;
            /// </summary>
            [SerializeField, Range(0.1f, 90)]
            float withHorizonAngle = 50f;

            /// <summary>
            /// 高度;
            /// </summary>
            [SerializeField, Range(2f, 20)]
            float cameraHeight = 7f;

            /// <summary>
            /// 设置旋转速度;
            /// </summary>
            public float RotationSpeed
            {
                get { return rotationSpeed; }
                set { rotationSpeed = value; }
            }

            /// <summary>
            /// 根据信息获取到摄像机应该放置的位置;
            /// </summary>
            public Vector3 GetCameranPosition(Vector3 targetPoint)
            {
                double currentRadius = (cameraHeight) / Math.Tan(withHorizonAngle * (Math.PI / 180));
                double x = targetPoint.x + Math.Sin(rotation) * currentRadius;
                double z = targetPoint.z + Math.Cos(rotation) * currentRadius;
                return new Vector3((float)x, cameraHeight, (float)z);
            }

            /// <summary>
            /// 输入更新,若存在输入则返回 true;
            /// </summary>
            public bool RotationInput()
            {
                bool isInput = false;

                if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_Rotate_Left))
                {
                    rotation -= rotationSpeed * InputOnUpdate;
                    isInput = true;
                }
                if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_Rotate_Right))
                {
                    rotation += rotationSpeed * InputOnUpdate;
                    isInput = true;
                }
                return isInput;
            }

        }

        [Serializable]
        public class FreeMovementCamera
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
            /// 输入更新,若存在输入则返回true;
            /// </summary>
            public bool MovementInput(ref Vector3 movement)
            {
                bool isInput = false;

                if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_Movement_Up))
                {
                    movement.z += movementSpeed * InputOnUpdate;
                    isInput = true;
                }
                if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_Movement_Down))
                {
                    movement.z -= movementSpeed * InputOnUpdate;
                    isInput = true;
                }
                if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_Movement_Right))
                {
                    movement.x += movementSpeed * InputOnUpdate;
                    isInput = true;
                }
                if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_Movement_Left))
                {
                    movement.x -= movementSpeed * InputOnUpdate;
                    isInput = true;
                }
                return isInput;
            }

        }

    }

}
