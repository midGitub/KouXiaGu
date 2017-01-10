using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using KouXiaGu.Terrain3D;

namespace KouXiaGu.UI
{

    [DisallowMultipleComponent]
    public class CreateMapUI : MonoBehaviour
    {
        CreateMapUI() { }

        [SerializeField]
        Button btnCreate;

        [SerializeField]
        InputField fieldId;

        [SerializeField]
        InputField fieldName;

        [SerializeField]
        InputField fieldVersion;

        [SerializeField]
        InputField fieldSummary;


        public string MapId
        {
            get { return fieldId.text; }
        }

        public string MapName
        {
            get { return fieldName.text; }
        }

        public string MapVersion
        {
            get { return fieldVersion.text; }
        }
        public string MapSummary
        {
            get { return fieldSummary.text; }
        }


        void Awake()
        {
            btnCreate.onClick.AddListener(CreateMap);
        }

        void CreateMap()
        {
            MapDescription description = new MapDescription()
            {
                Id = MapId,
                Name = MapName,
                Version = MapVersion,
                Summary = MapSummary,
                SaveTime = DateTime.Now.Ticks,
            };
            MapFiler.CreateNewMap(description);
        }

    }

}
