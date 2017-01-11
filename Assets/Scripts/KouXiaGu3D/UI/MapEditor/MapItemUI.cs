using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using KouXiaGu.Terrain3D;
using KouXiaGu.Globalization;

namespace KouXiaGu.UI
{

    /// <summary>
    /// 单个地图显示窗口;
    /// </summary>
    [DisallowMultipleComponent]
    public class MapItemUI : MonoBehaviour
    {
        MapItemUI() { }

        [SerializeField]
        Text mapMame;

        [SerializeField]
        Text mapData;

        [SerializeField]
        Toggle toggle;


        /// <summary>
        /// 当 toggle 为 选中时调用;
        /// </summary>
        public event Action<MapItemUI> OnPitch;

        public TerrainMapFile Map { get; private set; }
        public int Index { get; private set; }

        MapDescription mapDescription
        {
            get { return Map.Description; }
        }

        public string MapName
        {
            get { return mapMame.text; }
            private set { mapMame.text = value; }
        }

        public string MapData
        {
            get { return mapData.text; }
            private set { mapData.text = value; }
        }

        public bool IsOn
        {
            get { return toggle.isOn; }
            set { toggle.isOn = value; }
        }


        void Awake()
        {
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        void OnToggleValueChanged(bool isOn)
        {
            if (IsOn && OnPitch != null)
                OnPitch(this);
        }

        /// <summary>
        /// 初始化内部信息;
        /// </summary>
        public void Init(TerrainMapFile map, int index)
        {
            this.Map = map;
            this.Index = index;

            UpdateDescription();
        }

        /// <summary>
        /// 更新描述;
        /// </summary>
        public void UpdateDescription()
        {
            MapName = mapDescription.Name;
            MapData = new DateTime(mapDescription.SaveTime).ToString(Localization.Language);
        }

    }

}
