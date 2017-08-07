using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.OperationRecord;
using KouXiaGu.World.Resources;
using KouXiaGu.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.World.Map.MapEdit
{

    [DisallowMultipleComponent]
    public class UIChangeLandform : UIMapEditHandler
    {
        const string messageFormat = "Change landform to [{0}({1})]";
        int LandformID;
        int tempLandformID;
        public InputField idInputField;
        public InputField nameField;
        public Button applyButton;

        void Awake()
        {
            idInputField.onEndEdit.AddListener(UpdateNameField);
            applyButton.onClick.AddListener(OnApplyDown);
        }

        void UpdateNameField(string text)
        {
            tempLandformID = Convert.ToInt32(text);
            nameField.text = GetLandformName(tempLandformID);
        }

        void OnApplyDown()
        {
            LandformID = tempLandformID;
            Title.SetMessage(GetMessage());
        }

        public override string GetMessage()
        {
            return string.Format(messageFormat, GetLandformName(LandformID), LandformID);
        }

        string GetLandformName(int id)
        {
            if (GameInitializer.Instance.GameDataInitialize.IsCompleted)
            {
                IGameResource resource = GameInitializer.Instance.GameDataInitialize.Result;
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
            throw new NotImplementedException();
        }
    }
}
