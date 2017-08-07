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
        const string messageFormat = "Road: [{0}]";
        int roadType;
        int tempRoadType;
        public Toggle roadExistToggle;
        public InputField idInputField;
        public InputField nameField;
        public Button applyButton;

        bool existRoad
        {
            get { return roadExistToggle.isOn; }
        }

        void Awake()
        {
            idInputField.onEndEdit.AddListener(UpdateNameField);
            applyButton.onClick.AddListener(OnApplyDown);
        }

        void UpdateNameField(string text)
        {
            tempRoadType = Convert.ToInt32(text);
            nameField.text = GetRoadName(tempRoadType);
        }

        void OnApplyDown()
        {
            roadType = tempRoadType;
            Title.SetMessage(GetMessage());
        }

        public override string GetMessage()
        {
            return string.Format(messageFormat, existRoad);
        }

        public override bool Contrast(IMapEditHandler handler)
        {
            return handler is UIChangeRoad;
        }

        public string GetRoadName(int roadType)
        {
            if (GameInitializer.Instance.GameDataInitialize.IsCompleted)
            {
                IGameResource resource = GameInitializer.Instance.GameDataInitialize.Result;
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
                MapData map = world.WorldData.MapData.data;
                foreach (var node in nodes)
                {
                    node.Value.Road = node.Value.Road.Update(map, new NodeRoadInfo(roadType));
                }
            }
            return null;
        }
    }
}
