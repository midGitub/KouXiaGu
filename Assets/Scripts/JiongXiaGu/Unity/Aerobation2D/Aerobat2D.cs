using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Aerobation2D
{

    /// <summary>
    /// 飞行器控制;
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    public sealed class Aerobat2D : MonoBehaviour
    {
        private Aerobat2D()
        {
        }

        [SerializeField]
        private AerobatModelInfo modelInfo;
        [SerializeField]
        private AerobatInfo aerobatInfo;
        public Rigidbody Rigidbody { get; private set; }
        public WeaponsSystem WeaponsSystem { get; private set; }

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            float vertical = Input.GetAxis("Vertical");
            Rigidbody.AddForce(Vector3.forward * vertical * aerobatInfo.VerticalSpeed, ForceMode.Acceleration);

            float horizontal = Input.GetAxis("Horizontal");
            Rigidbody.AddForce(Vector3.right * horizontal * aerobatInfo.HorizontalSpeed, ForceMode.Acceleration);

            Vector3 position = transform.localPosition;
            position.x = Mathf.Clamp(position.x, modelInfo.ActivitySouthwest.x, modelInfo.ActivityNortheast.x);
            position.z = Mathf.Clamp(position.z, modelInfo.ActivitySouthwest.y, modelInfo.ActivityNortheast.y);
            transform.localPosition = position;
        }

        private void Fire()
        {
            bool fireAll = Input.GetButton("FireAll");
            if (fireAll)
            {
                WeaponsSystem.FireAll();
                return;
            }

            bool fire1 = Input.GetButton("Fire1");
            if (fire1)
            {
                WeaponsSystem.Fire(1);
                return;
            }

            bool fire2 = Input.GetButton("Fire2");
            if (fire2)
            {
                WeaponsSystem.Fire(2);
                return;
            }

            bool fire3 = Input.GetButton("Fire3");
            if (fire3)
            {
                WeaponsSystem.Fire(3);
                return;
            }
        }
    }
}
