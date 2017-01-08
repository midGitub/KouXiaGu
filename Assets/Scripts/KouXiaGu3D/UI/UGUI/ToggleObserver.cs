using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.UI
{

    [DisallowMultipleComponent]
    public class ToggleObserver : MonoBehaviour
    {
        ToggleObserver() { }

        [SerializeField]
        Toggle toggle;

        void Awake()
        {
            toggle.onValueChanged.AddListener(OnValueChanged);
        }

        void Start()
        {
            OnValueChanged(toggle.isOn);
        }

        void OnValueChanged(bool isOn)
        {
            gameObject.SetActive(isOn);
        }

    }

}
