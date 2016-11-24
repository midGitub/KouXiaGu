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
        [SerializeField]
        private bool Road;

        private WorldMapData worldMap;


        private void Awake()
        {
            worldMap = GameObject.FindObjectOfType<WorldMapData>();
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
            ShortVector2 mapPoint = WorldConvert.PlaneToHexPair(planePoint);
            WorldNode node;

            string LandformType = "Null";
            string Road = "Null";
            int RoadMask = 0;
            ShortVector2 mapBlockAddress = worldMap.worldMap.BlockMap.MapPointToAddress(mapPoint);
            string mapBlockName = mapBlockAddress.ToString();
            try
            {
                node = worldMap.Map[mapPoint];
                LandformType = node.Topography.ToString();
                Road = node.Road.ToString();
                //RoadMask = GetMask(mapPoint);
            }
            catch (BlockNotFoundException)
            {
                node = new WorldNode();
            }
            catch (KeyNotFoundException)
            {
                node = new WorldNode();
            }

            string str = "";

            str += "地图类型:" + LandformType
                + ";道路:" + Road
                + ";存在道路:" + RoadMask
                + ";地图块:" + mapBlockAddress
                + ";地图块编号:" + mapBlockName;

            return str;
        }

        private void AddMapNode(IList<long> down)
        {
            var planePoint = WorldConvert.MouseToPlane();
            ShortVector2 mapPoint = WorldConvert.PlaneToHexPair(planePoint);
            WorldNode node;
            try
            {
                node = worldMap.Map[mapPoint];
                node.Topography = Landform;
                node.Road = Road;
                worldMap.Map[mapPoint] = node;

                Debug.Log(GetMask(mapPoint));
            }
            catch (KeyNotFoundException)
            {
                node = new WorldNode();
                node.Topography = Landform;
                node.Road = Road;
                worldMap.Map.Add(mapPoint, node);
            }
            //Debug.Log(mapPoint + "Landform赋值为:" + Landform);
        }

        int GetMask(ShortVector2 mapPoint)
        {
            return (int)worldMap.Map.GetHexDirectionMask(mapPoint, node => node.Road);
        }


    }

}
