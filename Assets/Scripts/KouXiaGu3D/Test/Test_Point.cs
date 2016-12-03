using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.Test
{


    public class Test_Point : MonoBehaviour
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
            str += GetPlanePoint(mousePosition);

            return str;
        }

        private string GetScreenPoint(Vector2 mousePosition)
        {
            string str = "";

            str += "视窗坐标 :" + mousePosition;

            return str;
        }

        private string GetPlanePoint(Vector3 mousePosition)
        {
                Vector3 pixelPoint = MouseConvert.MouseToPixel();
                ShortVector2 offsetPoint = HexGrids.PixelToOffset(pixelPoint);
                string str = "";

                str += "六边形中心坐标 :" + HexGrids.OffsetToPixel(offsetPoint)
                    + "地图坐标 :" + offsetPoint
                    + "平面坐标 :" + pixelPoint;

                return str;
        }

    }


}
