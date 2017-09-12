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

        public MapData Read(IGameResource info)
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
                    LandformType = RandomXiaGu.Default.Get(landformArray),
                    Angle = RandomXiaGu.Default.Angle(),
                };

                if (RandomXiaGu.Default.Boolean())
                {
                    node.Road = new RoadNode()
                    {
                        ID = road.GetNewEffectiveID(),
                        RoadType = RandomXiaGu.Default.Get(roadArray),
                    };
                }

                //if (RandomBool())
                //{
                //    BuildingNode buildingNode = new BuildingNode();
                //    buildingNode.Update(building, Random(buildArray), RandomAngle());
                //    node.Building = buildingNode;
                //}

                map.Add(point, node);
            }

            MapData data = new MapData()
            {
                Data = map,
                Landform = landform,
                Road = road,
                Building = building,
            };

            return data;
        }
    }
}
