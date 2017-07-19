using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Cameras
{

    /// <summary>
    /// 相机参数监视;
    /// </summary>
    [RequireComponent(typeof(Camera))]
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public sealed class CanmeraMonitoring : MonoBehaviour
    {
        CanmeraMonitoring()
        {
        }

        public Camera monitoredCamera;
        Camera _thisCamera;

        Camera thisCamera
        {
            get { return _thisCamera ?? (_thisCamera = GetComponent<Camera>()); }
        }

        [ContextMenu("Set")]
        void OnPreRender()
        {
            thisCamera.fieldOfView = monitoredCamera.fieldOfView;
            thisCamera.orthographic = monitoredCamera.orthographic;
            thisCamera.nearClipPlane = monitoredCamera.nearClipPlane;
            thisCamera.farClipPlane = monitoredCamera.farClipPlane;
        }
    }
}
