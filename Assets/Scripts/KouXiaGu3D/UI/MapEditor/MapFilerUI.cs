using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.UI
{

    [DisallowMultipleComponent]
    public class MapFilerUI : MonoBehaviour
    {
        MapFilerUI() { }

        [SerializeField]
        Transform mapItemPrefabParent;

        [SerializeField]
        MapItemUI mapItemPrefab;

        void Awake()
        {

        }

        [ContextMenu("Create")]
        void Create()
        {
            Instantiate(mapItemPrefab, mapItemPrefabParent);
        }

    }

}
