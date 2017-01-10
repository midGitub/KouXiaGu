using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.UI
{

    [DisallowMultipleComponent, RequireComponent(typeof(Toggle))]
    public class PageUIObserver : MonoBehaviour
    {
        PageUIObserver() { }


        [SerializeField]
        PageUI target;

        Toggle toggle;

        void Awake()
        {
            toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(OnValueChanged);
        }

        void Start()
        {
            OnValueChanged(toggle.isOn);
        }

        void OnValueChanged(bool isOn)
        {
            if (isOn)
                target.Display();
            else
                target.Conceal();
        }

    }

}
