using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UniRx;
using UnityEngine.EventSystems;

namespace KouXiaGu.World2D.Navigation
{

    [DisallowMultipleComponent]
    public class NavMouseTo : Navigate
    {
        [SerializeField]
        ShortVector2 maximumRange = new ShortVector2(100,100);

        NavController navController;

        void Awake()
        {
            navController = NavController.GetInstance;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q) && !EventSystem.current.IsPointerOverGameObject())
            {
                NavToMouseMapPoint();
            }
        }

        void NavToMouseMapPoint()
        {
            ShortVector2 starting = WorldConvert.PlaneToHexPair(transform.position);
            ShortVector2 mouseMapPoint = WorldConvert.MouseToHexPair();
            NavPath navPath = navController.NavigateTo(starting, mouseMapPoint, maximumRange, null);
            StartFollow(navPath);
        }

    }

}
