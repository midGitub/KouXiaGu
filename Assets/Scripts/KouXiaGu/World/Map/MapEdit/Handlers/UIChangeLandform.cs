using System;
using System.Collections.Generic;
using KouXiaGu.OperationRecord;
using KouXiaGu.World.Resources;
using KouXiaGu.Globalization;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace KouXiaGu.World.Map.MapEdit
{

    /// <summary>
    /// 改变地形类型;
    /// </summary>
    [DisallowMultipleComponent]
    public class UIChangeLandform : UIMapEditHandler
    {
        const string messageFormat = "Landform: [{0}({1})]";
        int landformType;
        public Slider angleSlider;
        public InputField idInputField;
        public InputField nameField;

        void Awake()
        {
            idInputField.onEndEdit.AddListener(OnIdInputFieldValueChanged);
        }

        void Start()
        {
            var data = GameInitializer.GameData;
            if (data != null)
            {
                var firset = data.Terrain.Landform.FirstOrDefault().Value;
                if (firset != null)
                {
                    SetInfo(firset.ID);
                }
            }
            else
            {
                SetInfo(0);
            }
        }

        void OnIdInputFieldValueChanged(string text)
        {
            var landformType = Convert.ToInt32(text);
            SetInfo_internal(landformType);
        }

        public void SetInfo(int landformType)
        {
            idInputField.text = landformType.ToString();
            SetInfo_internal(landformType);
        }

        void SetInfo_internal(int landformType)
        {
            this.landformType = landformType;
            nameField.text = GetLandformName(landformType);
            Title.SetMessage(GetMessage(nameField.text));
        }

        public override string GetMessage()
        {
            string name = GetLandformName(landformType);
            return GetMessage(name);
        }

        string GetMessage(string landformName)
        {
            return string.Format(messageFormat, landformName, landformType);
        }

        string GetLandformName(int id)
        {
            var resource = GameInitializer.GameData;
            if (resource != null)
            {
                LandformInfo info;
                if (resource.Terrain.Landform.TryGetValue(id, out info))
                {
                    return Localization.GetLocalizationText(info.Name);
                }
            }
            return "Unknown";
        }

        public override bool Contrast(IMapEditHandler handler)
        {
            return handler is UIChangeLandform;
        }

        public override IVoidable Execute(IEnumerable<EditMapNode> nodes)
        {
            IWorldComplete world = WorldSceneManager.World;
            if (world != null)
            {
                MapData map = world.WorldData.MapData.data;
                foreach (var node in nodes)
                {
                   node.Value.Landform = node.Value.Landform.Update(map, new NodeLandformInfo(landformType, angleSlider.value));
                }
            }
            return null;
        }
    }
}
