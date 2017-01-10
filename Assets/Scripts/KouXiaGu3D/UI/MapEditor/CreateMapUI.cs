using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using KouXiaGu.Terrain3D;

namespace KouXiaGu.UI
{

    [DisallowMultipleComponent]
    public class CreateMapUI : MonoBehaviour
    {
        CreateMapUI() { }

        [SerializeField]
        Button btnCreate;

        [SerializeField]
        InputField fieldId;

        [SerializeField]
        InputField fieldName;

        [SerializeField]
        InputField fieldVersion;

        [SerializeField]
        InputField fieldSummary;

        void Awake()
        {

        }

    }

}
