using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XGame.Running
{


    /// <summary>
    /// 相机控制;
    /// </summary>
    [RequireComponent(typeof(RotateAbstract))]
    public sealed class CameraCotroller : MonoBehaviour
    {

        #region 变量;

        [SerializeField]
        private Camera thisCamera;

        [SerializeField, Tooltip("相机移动速度")]
        private float dragSleep = 1;

        [SerializeField, Space(8), Tooltip("最小缩放值")]
        private float minScaling = 5f;

        [SerializeField, Tooltip("最大缩放值")]
        private float maxScaling = 20f;

        [SerializeField, Tooltip("相机缩放速度")]
        private float scalingSleep = 1f;

        private int m_hashCode;

        private static readonly WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        private RotateAbstract m_rotation;

        #endregion


        #region 属性;

        public float DragSleep
        {
            get { return dragSleep; }
            set { dragSleep = value; }
        }

        public Camera Camera
        {
            get { return thisCamera; }
            set { thisCamera = value; }
        }

        public float MinScaling
        {
            get { return minScaling; }
            set { minScaling = value; }
        }

        public float MaxScaling
        {
            get { return maxScaling; }
            set { maxScaling = value; }
        }

        public float ScalingSleep
        {
            get { return scalingSleep; }
            set { scalingSleep = value; }
        }

        #endregion

        private void Awake()
        {
            thisCamera = thisCamera == null ? GetComponent<Camera>() : thisCamera;
            m_rotation = GetComponent<RotateAbstract>();
        }

        private void Start()
        {
            m_hashCode = this.GetHashCode();
            Input.LogIn(FunctionKey.CameraDrag, m_hashCode);
            Input.LogIn(FunctionKey.CameraRotateLeft, m_hashCode);
            Input.LogIn(FunctionKey.CameraRotateRigth, m_hashCode);
        }

        private void Update()
        {
            //拖拽;
            if (Input.GetKeyDown(FunctionKey.CameraDrag, m_hashCode))
            {
                StartCoroutine(MouseMiddleDrag());
            }
            //旋转;
            if (Input.GetKeyDown(FunctionKey.CameraRotateLeft, m_hashCode))
            {
                m_rotation.RotateLeft();
            }
            if (Input.GetKeyDown(FunctionKey.CameraRotateRigth, m_hashCode))
            {
                m_rotation.RotateRigth();
            }

            CameraScaling();
        }

        private void OnDisable()
        {
            Input.LogOut(FunctionKey.CameraDrag, m_hashCode);
            Input.LogOut(FunctionKey.CameraRotateLeft, m_hashCode);
            Input.LogOut(FunctionKey.CameraRotateRigth, m_hashCode);
        }


        /// <summary>
        /// 鼠标中间拖拽;
        /// </summary>
        private IEnumerator MouseMiddleDrag()
        {
            Vector2 m_tempVector = thisCamera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);

            while (Input.GetKey(FunctionKey.CameraDrag, m_hashCode))
            {
                Vector2 point = thisCamera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
                transform.position -= (Vector3)((point - m_tempVector) * dragSleep);
                m_tempVector = thisCamera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
                yield return waitForEndOfFrame;
            }
        }


        /// <summary>
        /// 相机滚轴缩放;
        /// </summary>
        private void CameraScaling()
        {
            float movement = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
            float scaling = thisCamera.orthographicSize + (movement * -scalingSleep * thisCamera.orthographicSize);
            thisCamera.orthographicSize = Mathf.Clamp(scaling, minScaling, maxScaling);
        }

    }

}
