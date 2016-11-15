﻿using KouXiaGu.Map;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.Test
{

    /// <summary>
    /// 游戏视窗位置测试;
    /// </summary>
    [SerializeField]
    public class PositionTest : MonoBehaviour
    {

        private Text textObject;

        private void Awake()
        {
            textObject = GetComponent<Text>();
        }

        private void Start()
        {
            this.ObserveEveryValueChanged(_ => UnityEngine.Input.mousePosition).
                SubscribeToText(textObject, TextUpdate);
        }

        private string TextUpdate(Vector3 mousePosition)
        {
            string str = "";

            str += GetScreenPoint(mousePosition);
            str += GetWorldPoint(mousePosition);
            str += GetMapPoint(mousePosition);

            return str;
        }

        private string GetScreenPoint(Vector3 mousePosition)
        {
            string str = "";

            str += "  视窗坐标 :" + mousePosition;

            return str;
        }

        private string GetWorldPoint(Vector3 mousePosition)
        {
            string str = "";

            str += "  世界坐标 :" + PositionConvert.MouseToPlanePoint();
            return str;
        }

        private string GetMapPoint(Vector3 mousePosition)
        {
            string str = "";

            str += "地图坐标 :" + GameMap.GetInstance.MouseToMapPoint();

            return str;
        }

    }

}
