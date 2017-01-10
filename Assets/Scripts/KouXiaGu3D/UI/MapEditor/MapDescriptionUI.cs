using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Terrain3D;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.UI
{

    [Serializable]
    public struct MapDescriptionUI
    {

        public InputField fieldId;
        public InputField fieldName;
        public InputField fieldVersion;
        public InputField fieldSummary;


        public string Id
        {
            get { return fieldId.text; }
            set { fieldId.text = value; }
        }

        public string Name
        {
            get { return fieldName.text; }
            set { fieldName.text = value; }
        }

        public string Version
        {
            get { return fieldVersion.text; }
            set { fieldVersion.text = value; }
        }

        public string Summary
        {
            get { return fieldSummary.text; }
            set { fieldSummary.text = value; }
        }

        public void Set(MapDescription description)
        {
            Id = description.Id;
            Name = description.Name;
            Version = description.Version;
            Summary = description.Summary;
        }

        /// <summary>
        /// 获取到默认设置的描述;
        /// </summary>
        public MapDescription Get()
        {
            MapDescription description = new MapDescription()
            {
                Id = Id,
                Name = Name,
                Version = Version,
                Summary = Summary,
                SaveTime = DateTime.Now.Ticks,
            };
            return description;
        }

        /// <summary>
        /// 设置所有的UI是否允许交互;
        /// </summary>
        public void SetInteractable(bool b)
        {
            fieldId.interactable = b;
            fieldName.interactable = b;
            fieldVersion.interactable = b;
            fieldSummary.interactable = b;
        }

    }

}
