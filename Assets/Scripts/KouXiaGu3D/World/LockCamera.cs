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
    [DisallowMultipleComponent, RequireComponent(typeof(Camera))]
    public class LockCamera : MonoBehaviour
    {
        /// <summary>
        /// 在Update 内更新的值输入需要乘上的数值;
        /// </summary>
        static float InputOnUpdate
        {
            get { return Time.deltaTime; }
        }

        [SerializeField, Range(2f, 20)]
        float height = 5;

        /// <summary>
        /// 相机运动速度;
        /// </summary>
        [SerializeField, Range(0.1f, 10)]
        float movementSpeed = 5;

        /// <summary>
        /// 摄像机需要移动到的点;
        /// </summary>
        Vector3 cameraTagetPoint;

        /// <summary>
        /// 相机当前移动速度;
        /// </summary>
        Vector3 currentVelocity;

        /// <summary>
        /// 输入时的运动速度;
        /// </summary>
        public float MovementSpeed
        {
            get { return movementSpeed; }
            set { movementSpeed = value; }
        }

        float MouseScrollWheel
        {
            get { return Input.GetAxis("Mouse ScrollWheel"); }
        }

        void Start()
        {
            cameraTagetPoint = transform.position;
            cameraTagetPoint = new Vector3(cameraTagetPoint.x, height, cameraTagetPoint.z);
        }

        void Update()
        {
            cameraTagetPoint += MovementAxis();
            transform.position = Vector3.SmoothDamp(transform.position, cameraTagetPoint, ref currentVelocity, 0.1f);
        }

        void OnValidate()
        {
            cameraTagetPoint = new Vector3(cameraTagetPoint.x, height, cameraTagetPoint.z);
        }

        /// <summary>
        /// 根据输入返回摄像机的偏移量;
        /// </summary>
        public Vector3 MovementAxis()
        {
            Vector3 axis;
            MovementInput(out axis);
            return axis;
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

            axis += (transform.forward * MouseScrollWheel * movementSpeed);
            return isInput;
        }

    }

}
