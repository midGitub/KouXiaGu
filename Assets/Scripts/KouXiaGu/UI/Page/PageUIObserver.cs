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

        [SerializeField]
        bool defaultPage;

        Toggle toggle;

        void Awake()
        {
            toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(OnValueChanged);
        }

        void OnDisable()
        {
            SetDefaultPage();
        }

        void Start()
        {
            SetDefaultPage();
            OnValueChanged(toggle.isOn);
        }

        void OnValueChanged(bool isOn)
        {
            if (isOn)
                target.Display();
            else
                target.Conceal();
        }

        void SetDefaultPage()
        {
            if (defaultPage)
                toggle.isOn = true;
            else
                toggle.isOn = false;
        }

    }

}
