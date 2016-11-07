using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                str += "  世界坐标 :" + raycastHit.point.ToString();
            }
            else
            {
                str += "  世界坐标 :" + "无法确定!";
            }
            return str;
        }


    }

}
