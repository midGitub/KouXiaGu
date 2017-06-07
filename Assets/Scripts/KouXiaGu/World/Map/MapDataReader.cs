using KouXiaGu.Grids;
using KouXiaGu.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 随机地图创建;
    /// </summary>
    class RandomMapDataCreater
    {
        public RandomMapDataCreater(int mapSize)
        {
            MapSize = mapSize;
        }

        public int MapSize { get; set; }

        public MapData Read(BasicResource info)
        {
            int[] landformArray = info.Terrain.Landform.Keys.ToArray();
            int[] roadArray = info.Terrain.Road.Keys.ToArray();
            int[] buildArray = info.Terrain.Building.Keys.ToArray();
            Dictionary<CubicHexCoord, MapNode> map = new Dictionary<CubicHexCoord, MapNode>();
            var points = CubicHexCoord.Range(CubicHexCoord.Self, MapSize);

            IdentifierGenerator landform = new IdentifierGenerator();
            IdentifierGenerator road = new IdentifierGenerator();
            IdentifierGenerator building = new IdentifierGenerator();

            foreach (var point in points)
            {
                MapNode node = new MapNode();

                node.Landform = new LandformNode()
                {
                    ID = landform.GetNewEffectiveID(),
                    LandformType = Random(landformArray),
                    Angle = RandomAngle(),
                };

                if (RandomBool())
                {
                    node.Road = new RoadNode()
                    {
                        ID = road.GetNewEffectiveID(),
                        RoadType = Random(roadArray),
                    };
                }

                if (false)
                {
                    BuildingNode buildingNode = new BuildingNode();
                    buildingNode.Update(building, Random(buildArray), RandomAngle());
                    node.Building = buildingNode;
                }

                map.Add(point, node);
            }

            MapData data = new MapData()
            {
                Map = map,
                Landform = landform,
                Road = road,
                Building = building,
            };

            return data;
        }

        static readonly System.Random random = new System.Random();

        bool RandomBool()
        {
            int i = random.Next(0, 2);
            return i == 0;
        }

        T Random<T>(T[] array)
        {
            int index = random.Next(0, array.Length);
            return array[index];
        }

        int RandomAngle()
        {
            return random.Next(0, 360);
        }
    }
}
