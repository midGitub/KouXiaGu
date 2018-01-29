using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using JiongXiaGu.Unity.Resources;

namespace JiongXiaGu.Unity.UI
{

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Toggle))]
    public class UIModificationItem : MonoBehaviour
    {
        private UIModificationItem()
        {
        }

        private const string DefaultModificationName = "Unkown";

        [SerializeField]
        private UIModificationController controller;
        [SerializeField]
        private Text modificationNameTextObject;
        private Toggle toggleObject;

        public ModificationDescription Description { get; private set; }
        public Toggle ToggleObject => toggleObject;

        public UIModificationController Controller
        {
            get { return controller; }
            set { controller = value; }
        }

        private void Awake()
        {
            toggleObject = GetComponent<Toggle>();
            toggleObject.onValueChanged.AddListener(OnToggleValueChanged);
        }

        private void OnToggleValueChanged(bool isOn)
        {
            if (isOn)
            {
                controller.OnSelected(this);
            }
        }

        public void SetDescription(ModificationDescription description)
        {
            Description = description;
            modificationNameTextObject.text = description.Name;
        }
    }
}
