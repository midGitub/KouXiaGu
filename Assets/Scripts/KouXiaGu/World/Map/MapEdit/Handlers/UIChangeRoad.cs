using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.OperationRecord;
using KouXiaGu.UI;
using UnityEngine;
using UnityEngine.UI;
using KouXiaGu.World.Resources;
using KouXiaGu.Globalization;

namespace KouXiaGu.World.Map.MapEdit
{

    /// <summary>
    /// 改变道路类型;
    /// </summary>
    [DisallowMultipleComponent]
    public class UIChangeRoad : UIMapEditHandler
    {
        const string messageFormat = "Road: [{0}, {1}({2})]";
        int roadType;
        public Toggle roadExistToggle;
        public InputField idInputField;
        public InputField nameField;

        bool existRoad
        {
            get { return roadExistToggle.isOn; }
        }

        void Awake()
        {
            idInputField.onEndEdit.AddListener(OnIdInputFieldValueChanged);
            roadExistToggle.onValueChanged.AddListener(OnRoadExistToggleValueChanged);
        }

        void Start()
        {
            OnRoadExistToggleValueChanged(roadExistToggle.isOn);

            var data = OGameInitializer.GameData;
            if (data != null)
            {
                var firset = data.Terrain.Road.FirstOrDefault().Value;
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

        public void SetInfo(int roadType)
        {
            idInputField.text = roadType.ToString();
            SetInfo_internal(roadType);
        }

        void SetInfo_internal(int roadType)
        {
            this.roadType = roadType;
            nameField.SetValue(GetRoadName(roadType));
            Title.SetMessage(GetMessage(nameField.text));

            if (roadType == 0)
            {
                roadExistToggle.SetValue(false);
            }
            else
            {
                roadExistToggle.SetValue(true);
            }
        }

        void OnIdInputFieldValueChanged(string text)
        {
            roadType = Convert.ToInt32(text);
            SetInfo_internal(roadType);
        }

        void OnRoadExistToggleValueChanged(bool isOn)
        {
            if (isOn)
            {
                idInputField.interactable = true;
                nameField.interactable = true;
            }
            else
            {
                idInputField.interactable = false;
                nameField.interactable = false;
            }
            Title.SetMessage(GetMessage());
        }

        public override string GetMessage()
        {
            string name = GetRoadName(roadType);
            return GetMessage(name);
        }

        string GetMessage(string roadName)
        {
            return string.Format(messageFormat, existRoad, roadName, roadType);
        }

        public override bool Contrast(IMapEditHandler handler)
        {
            return handler is UIChangeRoad;
        }

        public string GetRoadName(int roadType)
        {
            var resource = OGameInitializer.GameData;
            if (resource != null)
            {
                RoadInfo info;
                if (resource.Terrain.Road.TryGetValue(roadType, out info))
                {
                    return Localization.GetLocalizationText(info.Name);
                }
            }
            return "Unknown";
        }

        public override IVoidable Execute(IEnumerable<EditMapNode> nodes)
        {
            IWorldComplete world = WorldSceneManager.World;
            if (world != null)
            {
                if (world.BasicData.BasicResource.Terrain.Road.ContainsKey(roadType))
                {
                    MapData map = world.WorldData.MapData.data;
                    var roadInfo = roadExistToggle.isOn ? new NodeRoadInfo(roadType) : new NodeRoadInfo(0);
                    foreach (var node in nodes)
                    {
                        node.Value.Road = node.Value.Road.Update(map, roadInfo);
                    }
                }
            }
            return null;
        }
    }
}
