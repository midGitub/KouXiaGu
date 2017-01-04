using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.KeyInput
{

    /// <summary>
    /// 特殊按键监视;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class SpecialKey : UnitySington<SpecialKey>
    {

        [SerializeField, HideInInspector]
        ResponseKeyStack escape = new ResponseKeyStack(KeyCode.Escape);

        [SerializeField, HideInInspector]
        ResponseKeyStack enter = new ResponseKeyStack(KeyCode.Return);


        public static ResponseKeyStack Escape
        {
            get { return GetInstance.escape; }
        }

        public static ResponseKeyStack Enter
        {
            get { return GetInstance.enter; }
        }

        protected override void Awake()
        {
            base.Awake();

            escape.OnAwake();
            enter.OnAwake();
        }

        void Update()
        {
            escape.OnUpdate();
            enter.OnUpdate();
        }

    }

}
