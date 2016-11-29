using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.GameScene
{

    /// <summary>
    /// 摄像机脚本的输入控制;
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(Camera2DFollow))]
    public class Camera2DFollowInput : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 5)]
        float speed = 2.22f;
        [SerializeField]
        float minHeight = -10f;
        [SerializeField]
        float maxHeight = -3.7f;

        Camera2DFollow camera2DFollow;

        void Awake()
        {
            camera2DFollow = GetComponent<Camera2DFollow>();
        }

        void Update()
        {
            float result = camera2DFollow.CameraHeight + Input.GetAxis("Mouse ScrollWheel") * speed;
            camera2DFollow.CameraHeight = Mathf.Clamp(result, minHeight, maxHeight);
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            if (minHeight > maxHeight)
                throw new IndexOutOfRangeException("最小高度大于最大高度;");
        }
#endif

    }

}
