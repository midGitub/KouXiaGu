using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 更随目标摄像机;
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(Camera))]
    public class LockTargetCamera : MonoBehaviour
    {
        /// <summary>
        /// 在Update 内更新的值输入需要乘上的数值;
        /// </summary>
        static float InputOnUpdate
        {
            get { return Time.deltaTime; }
        }

        /// <summary>
        /// 更随的目标;
        /// </summary>
        [SerializeField]
        Transform target;

        /// <summary>
        /// 摄像机旋转时,每个周期的旋转量;
        /// </summary>
        [SerializeField, Range((float)(Math.PI / 180), (float)(Math.PI))]
        float rotationAngle = (float)(Math.PI / 2);

        /// <summary>
        /// 当前与目标形成的角度;
        /// </summary>
        [SerializeField]
        float currentAngle = (float)Math.PI;

        /// <summary>
        /// 与地平线的角度;
        /// </summary>
        [SerializeField, Range(0f, 90f)]
        float horizonAngle = 53f;

        /// <summary>
        /// 摄像机离人物的高度;
        /// </summary>
        [SerializeField]
        float height = 5f;

        /// <summary>
        /// 摄像机离人物的高度限制;
        /// </summary>
        [SerializeField]
        Vector2 heightRange = new Vector2(4,6);

        /// <summary>
        /// 摄像机需要移动到的点;
        /// </summary>
        Vector3 cameraTagetPoint;

        /// <summary>
        /// 摄像机需要旋转到的角度;
        /// </summary>
        Quaternion cameraTagetQuaternion;

        /// <summary>
        /// 相机当前移动速度;
        /// </summary>
        Vector3 currentVelocity;

        public float Height
        {
            get { return height + target.position.y; }
        }

        void Start()
        {
            cameraTagetPoint = transform.position;
        }

        void Update()
        {
            RotationInput();
            HeightInput();

            cameraTagetPoint = CameraPosition();
            cameraTagetQuaternion = LookRotation(transform.position, target.position);

            transform.rotation = Quaternion.Slerp(transform.rotation, cameraTagetQuaternion, 0.25f);
            transform.position = Vector3.SmoothDamp(transform.position, cameraTagetPoint, ref currentVelocity, 0.1f);
        }

        /// <summary>
        /// 获取到当前摄像机应该放置的位置;
        /// </summary>
        Vector3 CameraPosition()
        {
            Vector3 cameraPoint = transform.position;

            float angle = this.currentAngle;
            float cameraHeight = this.height;
            float withHorizonAngle = this.horizonAngle;

            Vector3 position = CameraPosition(target.position, angle, cameraHeight, withHorizonAngle);
            position.y = this.Height;
            return position;
        }

        /// <summary>
        /// 根据摄像机与地平线的角度获取到摄像机应该放置的位置;
        /// </summary>
        Vector3 CameraPosition(Vector3 target, float angle, float cameraHeight, float withHorizonAngle)
        {
            double currentRadius = (cameraHeight) / Math.Tan(withHorizonAngle * (Math.PI / 180));
            double x = target.x + Math.Sin(angle) * currentRadius;
            double z = target.z + Math.Cos(angle) * currentRadius;
            return new Vector3((float)x, cameraHeight, (float)z);
        }

        /// <summary>
        /// 旋转量输入更新;
        /// </summary>
        void RotationInput()
        {
            if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_Rotate_Left))
            {
                currentAngle -= rotationAngle * InputOnUpdate;
            }
            if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_Rotate_Right))
            {
                currentAngle += rotationAngle * InputOnUpdate;
            }
        }

        /// <summary>
        /// 高度输入更新;
        /// </summary>
        void HeightInput()
        {
            this.height -= Input.GetAxis("Mouse ScrollWheel");
            this.height = Mathf.Clamp(this.height, heightRange.x, heightRange.y);
        }

        /// <summary>
        /// 获取到相机望向目标的旋转量,类似 LookAt();
        /// </summary>
        public Quaternion LookRotation(Vector3 camera, Vector3 target)
        {
            return Quaternion.LookRotation(target - camera);
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
            float rotationAngle = (float)(Math.PI / 2);

            /// <summary>
            /// 相机运动速度;
            /// </summary>
            [SerializeField, Range(0.1f, 10)]
            float movementSpeed = 3;

            /// <summary>
            /// 输入时的运动速度;
            /// </summary>
            public float MovementSpeed
            {
                get { return movementSpeed; }
                set { movementSpeed = value; }
            }

            /// <summary>
            /// 设置旋转速度;
            /// </summary>
            public float RotationAngle
            {
                get { return rotationAngle; }
                set { rotationAngle = value; }
            }


            #region 旋转


            public Quaternion RotateAround(Quaternion camera)
            {
                Vector3 axis;
                RotationInput(out axis);
                return Quaternion.Euler(axis * 100 * RotationAngle) * camera;
            }

            /// <summary>
            /// 摄像机本身旋转,若存在旋转则返回 true;
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
            /// 摄像机围绕着目标点旋转,若存在旋转则返回 true;
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
            /// 根据摄像机与地平线的角度获取到摄像机应该放置的位置;
            /// </summary>
            public Vector3 CameraPosition(Transform camera, Vector3 target)
            {
                Vector3 cameraPoint = camera.position;

                float angle = (float)AngleY(target, cameraPoint);
                float cameraHeight = cameraPoint.y;
                float withHorizonAngle = camera.rotation.eulerAngles.x;

                return CameraPosition(target, angle, cameraHeight, withHorizonAngle);
            }

            /// <summary>
            /// 根据摄像机与地平线的角度获取到摄像机应该放置的位置;
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

            #endregion

            //#region 移动

            ///// <summary>
            ///// 对摄像机进行移动;
            ///// </summary>
            //public bool Movement(Transform camera)
            //{
            //    Vector3 axis;
            //    bool isInput = MovementInput(out axis);

            //    if (isInput)
            //    {
            //        float angleY = camera.rotation.eulerAngles.y;
            //        axis = Quaternion.Euler(0, angleY, 0) * axis;

            //        //camera.Translate(axis, Space.World);
            //        camera.position += axis;
            //    }

            //    return isInput;
            //}

            ///// <summary>
            ///// 根据输入返回摄像机的偏移量;
            ///// </summary>
            //public Vector3 MovementAxis(Vector3 cameraEulerAngles)
            //{
            //    Vector3 axis;
            //    MovementInput(out axis);

            //    float angleY = cameraEulerAngles.y;
            //    axis = Quaternion.Euler(0, angleY, 0) * axis;
            //    return axis;
            //}

            ///// <summary>
            ///// 是否存在输入?
            ///// </summary>
            //public bool IsMovementInput()
            //{
            //    return CustomInput.GetKeyHoldDown(KeyFunction.Camera_Movement_Up) ||
            //        CustomInput.GetKeyHoldDown(KeyFunction.Camera_Movement_Down) ||
            //        CustomInput.GetKeyHoldDown(KeyFunction.Camera_Movement_Right) ||
            //        CustomInput.GetKeyHoldDown(KeyFunction.Camera_Movement_Left);
            //}

            ///// <summary>
            ///// 获取到输入的偏移量,若存在输入则返回 true;
            ///// </summary>
            //public bool MovementInput(out Vector3 axis)
            //{
            //    bool isInput = false;
            //    axis = new Vector3();

            //    if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_Movement_Up))
            //    {
            //        axis.z += movementSpeed * InputOnUpdate;
            //        isInput = true;
            //    }
            //    if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_Movement_Down))
            //    {
            //        axis.z -= movementSpeed * InputOnUpdate;
            //        isInput = true;
            //    }
            //    if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_Movement_Right))
            //    {
            //        axis.x += movementSpeed * InputOnUpdate;
            //        isInput = true;
            //    }
            //    if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_Movement_Left))
            //    {
            //        axis.x -= movementSpeed * InputOnUpdate;
            //        isInput = true;
            //    }

            //    return isInput;
            //}

            //#endregion

        }

    }

}
