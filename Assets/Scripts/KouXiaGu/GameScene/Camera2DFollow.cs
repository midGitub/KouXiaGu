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

        public Transform target;
        public Vector2 offset;

        public float damping = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;

        private float m_OffsetZ;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;


        private void Start()
        {
            Vector3 cameraPoint = GetCameraPoint();
            m_LastTargetPosition = cameraPoint;
            m_OffsetZ = (transform.position - cameraPoint).z;
            transform.parent = null;
        }

        private void Update()
        {
            Vector3 cameraPoint = GetCameraPoint();
            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (cameraPoint - m_LastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
            }
            else
            {
                m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
            }

            Vector3 aheadTargetPos = cameraPoint + m_LookAheadPos + Vector3.forward * m_OffsetZ;
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

            transform.position = newPos;
            m_LastTargetPosition = cameraPoint;
        }

        /// <summary>
        /// 获取到摄像机需要到达的位置;
        /// </summary>
        public Vector3 GetCameraPoint()
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
