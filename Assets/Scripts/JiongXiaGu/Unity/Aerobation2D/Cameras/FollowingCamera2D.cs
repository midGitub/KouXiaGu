using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Aerobation2D.Cameras
{

    /// <summary>
    /// 跟随目标移动的相机;
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    public sealed class FollowingCamera2D : MonoBehaviour
    {
        private FollowingCamera2D()
        {
        }

        [SerializeField]
        private Vector3 cameraOffset;
        [SerializeField]
        private Vector3 offset;
        private Camera thisCamera;

        private void Awake()
        {
            thisCamera = GetComponent<Camera>();
        }

        [ContextMenu(nameof(Update))]
        private void Update()
        {
            transform.localPosition = cameraOffset;
            transform.LookAt(transform.parent);
            transform.localPosition += offset;
        }

        private void OnValidate()
        {
            Update();
        }
    }
}
