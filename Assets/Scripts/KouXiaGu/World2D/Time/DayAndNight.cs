using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World2D
{

    [DisallowMultipleComponent]
    public class DayAndNight : MonoBehaviour
    {
        [SerializeField]
        Camera mainCamera;
        
        [SerializeField]
        Light globalLight;

        [SerializeField, Range(0,1)]
        float value;

        void OnValidate()
        {
            Color color = new Color(value, value, value);
            mainCamera.backgroundColor = color;
            globalLight.color = color;
        }

    }

}
