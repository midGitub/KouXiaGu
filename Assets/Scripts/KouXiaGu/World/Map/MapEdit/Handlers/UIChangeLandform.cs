using System;
using System.Collections.Generic;
using KouXiaGu.OperationRecord;
using KouXiaGu.World.Resources;
using KouXiaGu.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.World.Map.MapEdit
{

    /// <summary>
    /// 改变地形类型;
    /// </summary>
    [DisallowMultipleComponent]
    public class UIChangeLandform : UIMapEditHandler
    {
        const string messageFormat = "Landform: [{0}({1})]";
        int LandformID;
        public Slider angleSlider;
        public InputField idInputField;
        public InputField nameField;

        void Awake()
        {
            idInputField.onEndEdit.AddListener(UpdateNameField);
        }

        void UpdateNameField(string text)
        {
            LandformID = Convert.ToInt32(text);
            nameField.text = GetLandformName(LandformID);
            Title.SetMessage(GetMessage());
        }

        public override string GetMessage()
        {
            return string.Format(messageFormat, GetLandformName(LandformID), LandformID);
        }

        string GetLandformName(int id)
        {
            if (GameInitializer.GameDataInitialize.IsCompleted)
            {
                IGameResource resource = GameInitializer.GameDataInitialize.Result;
                LandformInfo info;
                if(resource.Terrain.Landform.TryGetValue(id, out info))
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
                   node.Value.Landform = node.Value.Landform.Update(map, new NodeLandformInfo(LandformID, angleSlider.value));
                }
            }
            return null;
        }
    }
}
