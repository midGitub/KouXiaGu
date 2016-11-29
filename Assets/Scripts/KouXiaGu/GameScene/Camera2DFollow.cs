using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.GameScene
{

    /// <summary>
    /// 跟随目标相机,根据标准资源改造;
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(Camera))]
    public class Camera2DFollow : MonoBehaviour
    {

        [SerializeField]
        Transform target;
        [SerializeField]
        Vector2 offset;
        [SerializeField]
        float damping = 1;
        [SerializeField]
        float lookAheadFactor = 3;
        [SerializeField]
        float lookAheadReturnSpeed = 0.5f;
        [SerializeField]
        float lookAheadMoveThreshold = 0.1f;
        [SerializeField]
        float cameraHeight;

        Vector3 lastTargetPosition;
        Vector3 currentVelocity;
        Vector3 lookAheadPos;

        /// <summary>
        /// 跟随的目标;
        /// </summary>
        public Transform Target
        {
            get { return target; }
            set { target = value; }
        }

        /// <summary>
        /// 设置Z轴高度;设置摄像机高度;
        /// </summary>
        public float CameraHeight
        {
            get { return cameraHeight; }
            set { cameraHeight = value; }
        }

        void Awake()
        {
            Vector3 cameraPoint = GetCameraPoint();
            lastTargetPosition = cameraPoint;
            cameraHeight = cameraHeight == default(float) ? (transform.position - cameraPoint).z : cameraHeight;
            transform.parent = null;
        }

        void Update()
        {
            Vector3 cameraPoint = GetCameraPoint();
            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (cameraPoint - lastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                lookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
            }
            else
            {
                lookAheadPos = Vector3.MoveTowards(lookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
            }

            Vector3 aheadTargetPos = cameraPoint + lookAheadPos + Vector3.forward * cameraHeight;
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref currentVelocity, damping);

            transform.position = newPos;
            lastTargetPosition = cameraPoint;
        }

        /// <summary>
        /// 获取到摄像机需要到达的位置;
        /// </summary>
        Vector3 GetCameraPoint()
        {
            Vector3 targetPosition, camerzEulerAngles;
            float camerzX, cameraY, cameraZ, radius;

            targetPosition = target.position;
            camerzEulerAngles = transform.rotation.eulerAngles;
            cameraZ = transform.position.z;

            radius = (float)Math.Abs(Math.Tan(camerzEulerAngles.x * (Math.PI / 180)) * cameraZ);
            camerzX = targetPosition.x;
            cameraY = targetPosition.y - radius;

            return new Vector2(camerzX + offset.x, cameraY + offset.y);
        }

    }

}
