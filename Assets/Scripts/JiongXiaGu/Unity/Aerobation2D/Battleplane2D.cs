using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Aerobation2D
{


    public sealed class Battleplane2D : Aerobat2D
    {
        private Battleplane2D()
        {
        }

        [SerializeField]
        private float horizontalSpeed = 1;
        [SerializeField]
        private float verticalSpeed = 1;

        private void Update()
        {
            float vertical = Input.GetAxis("Vertical");
            Rigidbody.AddForce(Vector3.forward * vertical * verticalSpeed, ForceMode.Acceleration);

            float horizontal = Input.GetAxis("Horizontal");
            Rigidbody.AddForce(Vector3.right * horizontal * horizontalSpeed, ForceMode.Acceleration);
        }

        [ContextMenu(nameof(Test))]
        private void Test()
        {
            Debug.Log(transform.forward);
            Debug.Log(transform.rotation);
            Debug.Log(transform.rotation.eulerAngles);
        }
    }
}
