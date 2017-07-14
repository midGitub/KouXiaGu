using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
namespace KouXiaGu.UI
{

    /// <summary>
    /// 对 Toggle 进行拓展,加入了相反参数的事件;
    /// </summary>
    [RequireComponent(typeof(Toggle))]
    [DisallowMultipleComponent]
    public sealed class ToggleExpand : MonoBehaviour
    {
        ToggleExpand()
        {
        }

        Toggle toggleObject;

        /// <summary>
        /// 传入相反的值;
        /// </summary>
        [SerializeField]
        Toggle.ToggleEvent onValueChanged_opposite;

        void Awake()
        {
            toggleObject = GetComponent<Toggle>();
            toggleObject.onValueChanged.Invoke(toggleObject.isOn);
            toggleObject.onValueChanged.AddListener(OnToggleValueChanged);
            OnToggleValueChanged(toggleObject.isOn);
        }

        void OnToggleValueChanged(bool isOn)
        {
            onValueChanged_opposite.Invoke(!isOn);
        }
    }
}
