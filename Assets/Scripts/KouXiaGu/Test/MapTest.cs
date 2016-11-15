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
        private GameHexMap buildWorldHexMap;

        private void Awake()
        {
            textObject = GetComponent<Text>();
        }

        private void Start()
        {
            this.ObserveEveryValueChanged(_ => UnityEngine.Input.mousePosition).
                SubscribeToText(textObject, TextUpdate);

            var clickStream = Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0));

            clickStream.Buffer(clickStream.Throttle(TimeSpan.FromMilliseconds(250)))
                .Where(xs => xs.Count >= 2)
                .Subscribe(AddMapNode);
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
            IntVector2 mapPosition = buildWorldHexMap.MouseToMapPoint();

            string str = "";

            MapNode node = buildWorldHexMap.MapCollection.GetOrDefault(mapPosition);
            str += "存在节点:" + (node != null);

            return str;
        }

        private void AddMapNode(IList<long> down)
        {
            IntVector2 mapPosition = buildWorldHexMap.MouseToMapPoint();
            Debug.Log("加入到!" + mapPosition);
            buildWorldHexMap.MapCollection.AddOrReplace(mapPosition, new MapNode());
        }

    }

}
