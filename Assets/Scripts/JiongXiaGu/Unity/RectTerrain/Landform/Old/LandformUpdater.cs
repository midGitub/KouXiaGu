//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using JiongXiaGu.Grids;
//using JiongXiaGu.Unity.RectMaps;

//namespace JiongXiaGu.Unity.RectTerrain
//{

//    public class LandformUpdater : TerrainUpdater<RectCoord, LandformChunkRenderer>
//    {
//        public LandformUpdater(LandformBuilder builder, TerrainGuiderGroup<RectCoord> guiderGroup, WorldMap map) : base(builder, guiderGroup)
//        {
//            if (map == null)
//                throw new ArgumentNullException(nameof(map));

//            worldMap = map;
//            mapChangedRecorder = new ObserverEventBuffer<DictionaryEvent<RectCoord, MapNode>>();
//            unsubscriber = map.Map.Subscribe(mapChangedRecorder);
//        }

//        WorldMap worldMap;
//        IDisposable unsubscriber;
//        ObserverEventBuffer<DictionaryEvent<RectCoord, MapNode>> mapChangedRecorder;

//        IDictionary<RectCoord, MapNode> map
//        {
//            get { return worldMap.Map; }
//        }

//        protected override void GetPointsToUpdate(ref ICollection<RectCoord> needUpdatePoints)
//        {
//            base.GetPointsToUpdate(ref needUpdatePoints);

//            DictionaryEvent<RectCoord, MapNode> recorde;
//            while (mapChangedRecorder.TryDequeue(out recorde))
//            {
//                MapNode node;
//                if (map.TryGetValue(recorde.Key, out node))
//                {
//                    if (HasChanged(node, recorde.OriginalValue))
//                    {
//                        needUpdatePoints.Add(recorde.Key);
//                    }
//                }
//            }
//        }

//        bool HasChanged(MapNode node, MapNode original)
//        {
//            return  node.Landform != original.Landform 
//                || node.Road != original.Road;
//        }
//    }
//}
