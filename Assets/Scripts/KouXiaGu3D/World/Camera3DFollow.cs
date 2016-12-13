using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.GameScene
{

    /// <summary>
    /// 跟随目标相机;
    /// 摄像机总是望向目标;
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(Camera)), ExecuteInEditMode]
    public class Camera3DFollow : MonoBehaviour
    {
        Camera3DFollow() { }

        /// <summary>
        /// 更随目标;
        /// </summary>
        [SerializeField]
        Transform target;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        float radius = 2;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        float angle = 27f;

        /// <summary>
        /// 本 GameObject 绑定的摄像机;
        /// </summary>
        Camera cameraComponent;

        Vector3 cameraPoint
        {
            get { return transform.position; }
        }

        void Awake()
        {
            cameraComponent = GetComponent<Camera>();
        }

        void Update()
        {
            cameraComponent.transform.LookAt(target);
            Vector3 position = GetCameraPos();
            position.y += Input.GetAxis("Mouse ScrollWheel") * 1f;

            cameraComponent.transform.position = position;
        }

        public double currentRadius;
        public double x;
        public double z;
        public double q;

        /// <summary>
        /// 获取到摄像机位置;
        /// </summary>
        Vector3 GetCameraPos()
        {
            q = -(Math.Atan2((target.position.z - transform.position.z), (target.position.x - transform.position.x)) *
                 (180 / Math.PI) + 90) * (Math.PI / 180);

            currentRadius = (radius + cameraPoint.y) * Math.Tan(angle * (Math.PI / 180));
            x = target.position.x + Math.Sin(q) * currentRadius;
            z = target.position.z + Math.Cos(q) * currentRadius;
            return new Vector3((float)x, cameraPoint.y, (float)z);
        }

        void LateUpdate()
        {
            cameraComponent.transform.LookAt(target);
            cameraComponent.transform.RotateAround(target.position, Vector3.up, 1 * Input.GetAxis("Mouse X"));
        }



    }

}
