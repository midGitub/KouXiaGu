using System;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.World2D;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace KouXiaGu.Test
{

    public class Test_WorldMap : MonoBehaviour
    {
        [SerializeField]
        private Text textObject;

        [SerializeField]
        private int Landform;

        private WorldMap worldMap;


        private void Awake()
        {
            worldMap = GameController.FindInstance<WorldMap>();
            textObject = textObject ?? GetComponent<Text>();
        }

        private void Start()
        {
            textObject.ObserveEveryValueChanged(_ => WorldConvert.MouseToPlane()).
                SubscribeToText(textObject, ReadMap);

            var clickStream = Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0));

            clickStream.Buffer(clickStream.Throttle(TimeSpan.FromMilliseconds(250)))
                .Where(xs => xs.Count >= 2)
                .Subscribe(AddMapNode);
        }

        private string ReadMap(Vector2 planePoint)
        {
            IntVector2 mapPoint = WorldConvert.PlaneToHexPair(planePoint);
            WorldNode node;

            string LandformType;
            ShortVector2 mapBlockAddress = worldMap.wnWorldMap.GetAddress(mapPoint);
            string mapBlockName = worldMap.wnWorldMap.GetBlockName(mapBlockAddress);
            try
            {
                node = worldMap.Map[mapPoint];
                LandformType = node.Landform.ToString();
            }
            catch (BlockNotFoundException)
            {
                node = new WorldNode();
                LandformType = "不存在";
            }
            catch (KeyNotFoundException)
            {
                node = new WorldNode();
                LandformType = "不存在";
            }

            string str = "";

            str += "地图类型:" + LandformType
                + ";地图块:" + mapBlockAddress
                + ";地图块编号:" + mapBlockName;

            return str;
        }

        private void AddMapNode(IList<long> down)
        {
            var planePoint = WorldConvert.MouseToPlane();
            IntVector2 mapPoint = WorldConvert.PlaneToHexPair(planePoint);
            WorldNode node;
            try
            {
                node = worldMap.Map[mapPoint];
            }
            catch (KeyNotFoundException)
            {
                node = new WorldNode();
                worldMap.Map.Add(mapPoint, node);
            }
            node.Landform = Landform;
            Debug.Log(mapPoint + "Landform赋值为:" + Landform);
        }


    }

}
