using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;
using UnityEngine.UI;
using KouXiaGu.UI;

namespace KouXiaGu.World.Map.MapEdit
{

    [DisallowMultipleComponent]
    public sealed class UIMapEditSpiralSizer : MonoBehaviour
    {
        UIMapEditSpiralSizer()
        {
        }

        [SerializeField]
        UIMapEditSizer editSizer;
        [SerializeField]
        Slider sizeSlider;
        HexSpiralSizer hexSpiralSizer;

        public PointSizer PointSizer
        {
            get { return hexSpiralSizer; }
        }

        void Awake()
        {
            hexSpiralSizer = new HexSpiralSizer();
            sizeSlider.onValueChanged.AddListener(OnValueChanged);
        }

        void OnEnable()
        {
            editSizer.SetPointSizer(PointSizer);
        }

        void OnValueChanged(float value)
        {
            int size = Convert.ToInt32(value);
            hexSpiralSizer.SetSize(size);
        }
    }
}
