using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    [ExecuteInEditMode]
    public class LookAt : MonoBehaviour
    {

        [SerializeField]
        private Camera camera;

        private void Awake()
        {
            transform.forward = camera.transform.forward;

        }

        private void Update()
        {
            transform.forward = camera.transform.forward;
        }

    }

}
