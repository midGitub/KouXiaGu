using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Grids;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace KouXiaGu.Terrain3D.Navigation
{

    /// <summary>
    /// 鼠标点击控制导航点;
    /// </summary>
    [RequireComponent(typeof(IMovable))]
    public class Navigator_Mouse : Navigator
    {

        [SerializeField]
        PathFindingCost cost;

        [SerializeField]
        int searchRadius;

        IMovable character;

        void Awake()
        {
            character = GetComponent<IMovable>();
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetMouseButtonDown(0) &&
                EventSystem.current != null &&
                !EventSystem.current.IsPointerOverGameObject())
            {
                CubicHexCoord starting = Position.GetTerrainCubic();
                CubicHexCoord destination = LandformTrigger.MouseRayPointOrDefault().GetTerrainCubic();
                HexRadiusRange searchRange = new HexRadiusRange(searchRadius, starting);
                this.NavigateTo(starting, destination, cost, searchRange, character);
            }
        }

    }

}
