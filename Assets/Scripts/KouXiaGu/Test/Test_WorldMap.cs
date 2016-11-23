using System;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.World2D.Map;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using KouXiaGu.World2D;

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
            worldMap = GameObject.FindObjectOfType<WorldMap>();
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
            ShortVector2 mapBlockAddress = worldMap.worldMap.BlockMap.PlanePointToAddress(mapPoint);
            string mapBlockName = mapBlockAddress.ToString();
            try
            {
                node = worldMap.Map[mapPoint];
                LandformType = node.Topography.ToString();
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
                node.Topography = Landform;
                worldMap.Map[mapPoint] = node;
            }
            catch (KeyNotFoundException)
            {
                node = new WorldNode();
                node.Topography = Landform;
                worldMap.Map.Add(mapPoint, node);
            }
            Debug.Log(mapPoint + "Landform赋值为:" + Landform);
        }


    }

}
