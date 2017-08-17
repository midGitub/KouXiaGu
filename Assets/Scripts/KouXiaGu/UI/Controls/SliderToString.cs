using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.UI
{

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Slider))]
    public sealed class SliderToString : MonoBehaviour
    {
        SliderToString()
        {
        }

        [SerializeField]
        InputField.OnChangeEvent onValueChanged;
        Slider sliderObject;

        void Awake()
        {
            sliderObject = GetComponent<Slider>();
            sliderObject.onValueChanged.AddListener(OnSliderValueChanged);
        }

        void Start()
        {
            OnSliderValueChanged(sliderObject.value);
        }

        void OnSliderValueChanged(float value)
        {
            onValueChanged.Invoke(value.ToString());
        }
    }
}
