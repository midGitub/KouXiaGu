using KouXiaGu.KeyInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Terrain3D;

namespace KouXiaGu.Cameras
{
    /// <summary>
    /// 场景相机;
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    public sealed class WorldCanmera : MonoBehaviour
    {
        const string MouseScrollWheelAxis = "Mouse ScrollWheel";
        const string Mouse_x_axis = "Mouse X";
        const string Mouse_y_axis = "Mouse Y";

        [SerializeField]
        Transform target;
        [SerializeField]
        bool isLockTarget = false;
        [SerializeField]
        Vector3 defaultTargetOffset;
        [SerializeField, Range(0, 1)]
        float smoothTime = 0.3f;

        [SerializeField, Range(0, 1)]
        float zoom = 0;
        [SerializeField, Range(0, 1)]
        float maxZoom = 0.6f;
        [SerializeField, Range(0.1f, 5)]
        float zoomRatio = 0.1f;

        [SerializeField, Range(3, 10)]
        float movementRatio = 6;
        [SerializeField]
        bool canEdgeMovement = true;
        [SerializeField, Range(0.1f, 5)]
        float edgeMovementRatio = 1;

        /// <summary>
        /// x, y 对应横轴距离, z, w 对应竖轴距离;
        /// </summary>
        [SerializeField]
        Vector4 cameraClamp = new Vector4(-5, 5, -5, 5);

        Camera _camera;
        Vector3 phonyTargetPosition;
        Vector3 currentOffset;
        Vector3 finalPosition;
        Vector3 currentVelocity;

        /// <summary>
        /// 摄像机望向的目标;
        /// </summary>
        public Transform Target
        {
            get { return target; }
        }

        /// <summary>
        /// 是否锁定相机望向目标?
        /// </summary>
        public bool IsLockTarget
        {
            get { return isLockTarget; }
            set { isLockTarget = value; }
        }

        /// <summary>
        /// 相机缩放值,数值越大越靠近目标,0 ~ maxZoom
        /// </summary>
        public float Zoom
        {
            get { return zoom; }
            set { zoom = Mathf.Clamp(value, 0, maxZoom); }
        }

        /// <summary>
        /// 缩放灵敏度,数值越大越灵敏;
        /// </summary>
        public float ZoomRatio
        {
            get { return zoomRatio; }
            set { zoomRatio = value; }
        }

        /// <summary>
        /// 相机移动灵敏度,数值越大越灵敏;
        /// </summary>
        public float MovementRatio
        {
            get { return movementRatio; }
            set { movementRatio = value; }
        }

        /// <summary>
        /// 是否允许光标在窗口边缘时,摄像机进行移动;
        /// </summary>
        public bool CanEdgeMovement
        {
            get { return canEdgeMovement; }
            set { canEdgeMovement = value; }
        }

        /// <summary>
        /// 边缘移动灵敏度,数值越大越灵敏;
        /// </summary>
        public float EdgeMovementRatio
        {
            get { return edgeMovementRatio; }
            set { edgeMovementRatio = value; }
        }

        void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        /// <summary>
        /// 摄像机望向的目标,若不存在则置为Null;
        /// </summary>
        public void SetTarget(Transform target)
        {
            if (target == null)
            {
                if (this.target != null)
                {
                    phonyTargetPosition = this.target.position + currentOffset;
                    currentOffset = Vector3.zero;
                    this.target = null;
                }
            }
            else
            {
                CameraRetrun();
                this.target = target;
            }
        }

        void Update()
        {
            Vector3 targetPosition;

            if (target == null)
            {
                ZoomRespond();
                MovementRespond(ref phonyTargetPosition);
                EdgeMovementRespond(ref phonyTargetPosition);
                targetPosition = phonyTargetPosition;
            }
            else
            {
                targetPosition = target.position;
                if (!isLockTarget)
                {
                    ZoomRespond();
                    CameraOffsetClamp(ref currentOffset);
                    MovementRespond(ref currentOffset);
                    EdgeMovementRespond(ref currentOffset);
                    CameraRetrunRespond();
                }
                else
                {
                    currentOffset = Vector3.zero;
                    zoom = 0;
                }
            }

            finalPosition = GetCameraPosition(targetPosition);
            transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref currentVelocity, smoothTime);
        }

        void OnValidate()
        {
            Zoom = zoom;
        }

        /// <summary>
        /// 缩放响应;
        /// </summary>
        void ZoomRespond()
        {
            float mouseScrollWheel = Input.GetAxis(MouseScrollWheelAxis);
            zoom += mouseScrollWheel * zoomRatio;
            Zoom = zoom;
        }

        /// <summary>
        /// 摄像机移动响应;
        /// </summary>
        void MovementRespond(ref Vector3 position)
        {
            if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_movement_up))
            {
                position.z += movementRatio * Time.deltaTime;
            }
            if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_movement_down))
            {
                position.z -= movementRatio * Time.deltaTime;
            }
            if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_movement_left))
            {
                position.x -= movementRatio * Time.deltaTime;
            }
            if (CustomInput.GetKeyHoldDown(KeyFunction.Camera_movement_right))
            {
                position.x += movementRatio * Time.deltaTime;
            }
        }

        /// <summary>
        /// 窗口边缘移动;
        /// </summary>
        void EdgeMovementRespond(ref Vector3 position)
        {
            const float edgeWidth = 5;
            if (canEdgeMovement)
            {
                float height = _camera.pixelHeight;
                float width = _camera.pixelWidth;
                float mouseHeight = Input.mousePosition.y;
                float mouseWidth = Input.mousePosition.x;

                float heightResult = height - mouseHeight;
                if (heightResult < edgeWidth)
                {
                    float mouseY = Input.GetAxis(Mouse_y_axis);
                    if (mouseY > 0)
                    {
                        position.z += mouseY * edgeMovementRatio;
                    }
                }
                else if (heightResult > (height - edgeWidth))
                {
                    float mouseY = Input.GetAxis(Mouse_y_axis);
                    if (mouseY < 0)
                    {
                        position.z += mouseY * edgeMovementRatio;
                    }
                }

                float widthResult = width - mouseWidth;
                if (widthResult < edgeWidth)
                {
                    float mouseX = Input.GetAxis(Mouse_x_axis);
                    if (mouseX > 0)
                    {
                        position.x += mouseX * edgeMovementRatio;
                    }
                }
                else if (widthResult > (width - edgeWidth))
                {
                    float mouseX = Input.GetAxis(Mouse_x_axis);
                    if (mouseX < 0)
                    {
                        position.x += mouseX * edgeMovementRatio;
                    }
                }
            }
        }

        /// <summary>
        /// 相机归正响应;
        /// </summary>
        void CameraRetrunRespond()
        {
            if (CustomInput.GetKeyDown(KeyFunction.Camera_return))
            {
                CameraRetrun();
            }
        }

        /// <summary>
        /// 将摄像机望向目标;
        /// </summary>
        void CameraRetrun()
        {
            currentOffset = Vector3.zero;
        }

        /// <summary>
        /// 摄像机与对象偏移量限制;
        /// </summary>
        void CameraOffsetClamp(ref Vector3 position)
        {
            position.x = Mathf.Clamp(position.x, cameraClamp.x, cameraClamp.y);
            position.z = Mathf.Clamp(position.z, cameraClamp.z, cameraClamp.w);
        }

        /// <summary>
        /// 获取到摄像机应该在的位置;
        /// </summary>
        Vector3 GetCameraPosition(Vector3 targetPos)
        {
            Vector3 pos = new Vector3(targetPos.x + defaultTargetOffset.x + currentOffset.x, defaultTargetOffset.y, targetPos.z + defaultTargetOffset.z + currentOffset.z);
            pos = Vector3.LerpUnclamped(pos, targetPos, zoom);
            return pos;
        }

        public WorldCanmeraConfig ToConfig()
        {
            WorldCanmeraConfig config = new WorldCanmeraConfig()
            {
                IsLockTarget = isLockTarget,
                CanEdgeMovement = canEdgeMovement,
                EdgeMovementRatio = edgeMovementRatio,
                MovementRatio = movementRatio,
                Zoom = zoom,
                ZoomRatio = zoomRatio,
            };
            return config;
        }

        public void SetConfig(WorldCanmeraConfig config)
        {
            IsLockTarget = config.IsLockTarget;
            CanEdgeMovement = config.CanEdgeMovement;
            EdgeMovementRatio = config.EdgeMovementRatio;
            MovementRatio = config.MovementRatio;
            Zoom = zoom;
            ZoomRatio = config.ZoomRatio;
        }

        [ContextMenu("设置当前坐标到默认偏移")]
        void SetDefaultTargetOffset()
        {
            defaultTargetOffset = transform.position;
        }
    }
}
