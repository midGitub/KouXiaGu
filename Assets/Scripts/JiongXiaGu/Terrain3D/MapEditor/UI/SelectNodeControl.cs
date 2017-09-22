using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JiongXiaGu.Grids;
using JiongXiaGu.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using JiongXiaGu.Terrain3D;

namespace JiongXiaGu.Terrain3D.MapEditor
{

    [DisallowMultipleComponent]
    public class SelectNodeControl : MonoBehaviour
    {
        SelectNodeControl()
        {
        }

        [SerializeField]
        SelectNodeList selectList;
        [SerializeField]
        Toggle activateMouseSelectToggle;
        SelectablePanel panel;

        void Awake()
        {
            panel = GetComponentInParent<SelectablePanel>();
        }

        void Update()
        {
            if (activateMouseSelectToggle.isOn && panel.IsFocus)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    Vector3 mousePoint;
                    if (LandformRay.Instance.TryGetMouseRayPoint(out mousePoint))
                    {
                        CubicHexCoord pos = mousePoint.GetTerrainCubic();
                        Add(pos);
                    }
                }
                if (Input.GetKeyDown(KeyCode.Mouse1) && !EventSystem.current.IsPointerOverGameObject())
                {
                    Vector3 mousePoint;
                    if (LandformRay.Instance.TryGetMouseRayPoint(out mousePoint))
                    {
                        CubicHexCoord pos = mousePoint.GetTerrainCubic();
                        Remove(pos);
                    }
                }
            }
        }

        void Add(CubicHexCoord position)
        {
            if (!selectList.Contains(position))
            {
                selectList.Add(position);
            }
        }

        void Remove(CubicHexCoord position)
        {
            selectList.Remove(position);
        }
    }
}
