using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Aerobation2D
{

    /// <summary>
    /// 飞行器抽象类;
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Aerobat2D : MonoBehaviour
    {
        protected Aerobat2D()
        {
        }

        protected Rigidbody Rigidbody { get; private set; }

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }
    }
}
