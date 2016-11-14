using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Map;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.Test
{

    public class MapTest : MonoBehaviour
    {
        private Text textObject;

        [SerializeField]
        private BuildWorldHexMap buildWorldHexMap;

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

            str += GetMapNodeInfo();

            return str;
        }

        /// <summary>
        /// 获取到鼠标指向节点的信息;
        /// </summary>
        private string GetMapNodeInfo()
        {
            string str = "";



            return str;
        }

    }

}
