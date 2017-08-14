using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.OperationRecord;
using UnityEngine;
using UnityEngine.UI;
using KouXiaGu.World.Resources;
using KouXiaGu.Globalization;

namespace KouXiaGu.World.Map.MapEdit
{

    [DisallowMultipleComponent]
    public class UIChangeRoad : UIMapEditHandler
    {
        const string messageFormat = "Road: [{0}, {1}({2})]";
        const string strUnknown = "Unknown";
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
            idInputField.onEndEdit.AddListener(UpdateNameField);
            roadExistToggle.onValueChanged.AddListener(OnRoadExistToggleValueChanged);
        }

        void Start()
        {
            OnRoadExistToggleValueChanged(roadExistToggle.isOn);

            var data = GameInitializer.GameData;
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
            this.roadType = roadType;
            if (roadType == 0)
            {
                roadExistToggle.isOn = false;
                idInputField.text = roadType.ToString();
                idInputField.interactable = false;
                nameField.text = strUnknown;
                Title.SetMessage(GetMessage(strUnknown));
            }
            else
            {
                roadExistToggle.isOn = true;
                idInputField.text = roadType.ToString();
                idInputField.interactable = true;
                nameField.text = GetRoadName(roadType);
                Title.SetMessage(GetMessage(nameField.text));
            }
        }

        void UpdateNameField(string text)
        {
            roadType = Convert.ToInt32(text);
            SetInfo(roadType);
        }

        void OnRoadExistToggleValueChanged(bool isOn)
        {
            if (isOn)
            {
                idInputField.text = roadType.ToString();
                idInputField.interactable = true;
            }
            else
            {
                idInputField.text = 0.ToString();
                idInputField.interactable = false;
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
            var resource = GameInitializer.GameData;
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
                    var roadInfo = new NodeRoadInfo(roadType);
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
