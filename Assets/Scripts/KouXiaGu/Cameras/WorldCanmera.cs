using KouXiaGu.KeyInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Terrain3D;

namespace KouXiaGu.Cameras
{

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    public sealed class WorldCanmera : MonoBehaviour
    {

        [SerializeField]
        Transform target;
        [SerializeField]
        Vector3 defaultTargetOffset;

        [SerializeField, Range(0, 1)]
        float smoothTime = 0.3f;
        [SerializeField, Range(0, 1)]
        float zoom = 0;
        [SerializeField, Range(0, 1)]
        float maxZoom = 0.6f;
        [SerializeField, Range(1, 40)]
        float zoomRatio = 16;

        [SerializeField]
        Vector3 currentOffset;
        [SerializeField, Range(3, 10)]
        float movementRatio = 6;
        [SerializeField]
        bool isEdgeMovement = true;

        public Vector3 finalPosition;
        public Vector3 currentVelocity;

        public float ZoomValue
        {
            get { return zoom; }
            set { zoom = Mathf.Clamp(value, 0, maxZoom); }
        }

        /// <summary>
        /// 是否允许光标在窗口边缘时,摄像机进行移动;
        /// </summary>
        public bool IsEdgeMovement
        {
            get { return isEdgeMovement; }
        }

        void Update()
        {
            ZoomRespond();
            CanmeraMovementRespond();

            finalPosition = GetTargetPosition();
            Vector3 pos = Vector3.LerpUnclamped(finalPosition, target.position, zoom);
            transform.position = Vector3.SmoothDamp(transform.position, pos, ref currentVelocity, 0.3f);
        }

        void OnValidate()
        {
            ZoomValue = zoom;
        }

        Vector3 GetTargetPosition()
        {
            Vector3 targetPos = target.position;
            return new Vector3(targetPos.x + defaultTargetOffset.x + currentOffset.x, defaultTargetOffset.y, targetPos.z + defaultTargetOffset.z + currentOffset.z);
        }

        void ZoomRespond()
        {
            float mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");
            zoom += mouseScrollWheel * zoomRatio * Time.deltaTime;
            ZoomValue = zoom;
        }

        void CanmeraMovementRespond()
        {
            if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_movement_up))
            {
                currentOffset.z += movementRatio * Time.deltaTime;
            }
            if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_movement_down))
            {
                currentOffset.z -= movementRatio * Time.deltaTime;
            }
            if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_movement_left))
            {
                currentOffset.x -= movementRatio * Time.deltaTime;
            }
            if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_movement_right))
            {
                currentOffset.x += movementRatio * Time.deltaTime;
            }
            if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_return))
            {
                currentOffset = default(Vector3);
            }
        }

        [ContextMenu("设置当前坐标到默认偏移")]
        void SetDefaultTargetOffset()
        {
            defaultTargetOffset = transform.position;
        }
    }
}
