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
        [SerializeField]
        List<CubicHexCoord> offsets;
        public int Size { get; private set; }

        void Awake()
        {
            offsets = new List<CubicHexCoord>();
            Size = -1;
            sizeSlider.onValueChanged.AddListener(SetSize);
        }

        void OnEnable()
        {
            SetSize(sizeSlider.value);
        }

        public void SetSize(float value)
        {
            int size = Convert.ToInt32(value);
            SetSize(size);
        }

        public void SetSize(int size)
        {
            if (Size != size)
            {
                Size = size;
                offsets.Clear();
                offsets.AddRange(CubicHexCoord.Spiral_in(CubicHexCoord.Self, Size));
                SendChanged();
            }
        }

        void SendChanged()
        {
            if (enabled == true)
            {
                editSizer.SetSelecteOffsets(offsets);
            }
        }
    }
}
