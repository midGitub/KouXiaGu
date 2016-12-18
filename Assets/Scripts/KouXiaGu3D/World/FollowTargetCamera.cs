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
        /// 更随目标;
        /// </summary>
        [SerializeField]
        Transform target;

        [SerializeField]
        bool isFollow = false;

        /// <summary>
        /// 摄像机旋转角度;
        /// </summary>
        [SerializeField, Range(0f, (float)Math.PI)]
        float rotation = (float)Math.PI;

        /// <summary>
        /// 旋转速度;
        /// </summary>
        [SerializeField, Range(0.1f, 5)]
        float rotationSpeed = 1;

        /// <summary>
        /// 摄像机旋转时,每个周期的旋转量;
        /// </summary>
        float rotationValue;

        /// <summary>
        /// 相机运动速度;
        /// </summary>
        [SerializeField]
        float movementSpeed = 1;

        /// <summary>
        /// 地平线角度;
        /// </summary>
        [SerializeField, Range(0.1f, 90)]
        float withHorizonAngle = 50f;

        /// <summary>
        /// 高度;
        /// </summary>
        [SerializeField, Range(2f, 20)]
        float height = 7f;

        /// <summary>
        /// 本 GameObject 绑定的摄像机;
        /// </summary>
        Camera cameraComponent;

        /// <summary>
        /// 摄像机需要移动到的点;
        /// </summary>
        Vector3 cameraTagetPoint;
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
        }

        public Transform Target
        {
            get { return target; }
            set
            {
                Transform target = value;
                if (value == null)
                {
                    GameObject newTarget = new GameObject("CameraTarget");
                    target = newTarget.transform;
                    target.position = target.position;
                }
                else
                {
                    if (this.target.name == "CameraTarget")
                    {
                        Destroy(this.target.gameObject);
                    }
                }
                this.target = target;
            }
        }

        /// <summary>
        /// 设置旋转速度;
        /// </summary>
        public float RotationSpeed
        {
            get { return rotationSpeed; }
            set {
                rotationSpeed = value;
                UpdateRotationValue();
            }
        }

        /// <summary>
        /// 输入时的运动速度;
        /// </summary>
        public float MovementSpeed
        {
            get { return movementSpeed; }
            set { movementSpeed = value; }
        }

        void Awake()
        {
            cameraComponent = GetComponent<Camera>();
            UpdateRotationValue();
        }

        void Update()
        {
            Vector3 temp_cameraPoint = cameraPoint;

            RotationInput();
            MovementInput(ref temp_cameraPoint);

            cameraTagetPoint = GetCameranPosition(targetPoint, height, rotation, withHorizonAngle);
            cameraComponent.transform.LookAt(target);

            cameraPoint = Vector3.SmoothDamp(temp_cameraPoint, cameraTagetPoint, ref currentVelocity, 0.1f);
        }

        void OnValidate()
        {
            cameraPoint = new Vector3(cameraPoint.x, height, cameraPoint.z);
            UpdateRotationValue();
        }

        void RotationInput()
        {
            if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_Rotate_Left))
            {
                rotation -= rotationValue;
            }
            if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_Rotate_Right))
            {
                rotation += rotationValue;
            }
        }

        void MovementInput(ref Vector3 movement)
        {
            if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_Movement_Up))
            {
                movement.z += movementSpeed;
            }
            if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_Movement_Down))
            {
                movement.z -= movementSpeed;
            }
            if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_Movement_Right))
            {
                movement.x += movementSpeed;
            }
            if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_Movement_Left))
            {
                movement.x -= movementSpeed;
            }
        }

        void UpdateRotationValue()
        {
            rotationValue = (float)(rotationSpeed * (Math.PI / 180));
        }

        /// <summary>
        /// 根据信息获取到摄像机应该放置的位置;
        /// </summary>
        static Vector3 GetCameranPosition(Vector3 targetPoint, float cameraHeight, double rotation, float withHorizonAngle)
        {
            double currentRadius = (cameraHeight) / Math.Tan(withHorizonAngle * (Math.PI / 180));
            double x = targetPoint.x + Math.Sin(rotation) * currentRadius;
            double z = targetPoint.z + Math.Cos(rotation) * currentRadius;
            return new Vector3((float)x, cameraHeight, (float)z);
        }

        /// <summary>
        /// 获取两个点的角度;
        /// </summary>
        public static double Angle(Vector3 target, Vector3 current)
        {
            double angle = -(Math.Atan2((target.z - current.z), (target.x - current.x)) *
               (180 / Math.PI) + 90) * (Math.PI / 180);

            return angle;
        }

    }

}
